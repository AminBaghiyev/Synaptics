using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using Synaptics.Application.Interfaces.Repositories;
using Synaptics.Domain.Entities;
using Synaptics.Domain.Enums;

namespace Synaptics.Persistence.Services;

public class PostCommentService : IPostCommentService
{
    readonly UserManager<AppUser> _userManager;
    readonly IPostCommentRepository _repository;
    readonly IPostRepository _postRepository;
    readonly ICommentLikeRepository _likeRepository;
    readonly IUserRelationRepository _relationRepository;
    readonly IMapper _mapper;

    public PostCommentService(IPostCommentRepository repository, IMapper mapper, UserManager<AppUser> userManager, IUserRelationRepository relationRepository, IPostRepository postRepository, ICommentLikeRepository likeRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _userManager = userManager;
        _relationRepository = relationRepository;
        _postRepository = postRepository;
        _likeRepository = likeRepository;
    }

    public async Task<ICollection<PostCommentItemDTO>> GetAllAsync(long postId, string? current = null, int count = 30, int page = 0)
    {
        Post post = await _postRepository.GetOneAsync(e =>
            e.Id == postId &&
            !e.IsDeleted, includes: ["User"]
        ) ?? throw new ExternalException("Post not found!");

        AppUser currentUser = new();
        bool isFriend = false;
        bool isMy = false;

        if (current is not null)
        {
            currentUser = await _userManager.FindByNameAsync(current) ?? throw new ExternalException("User not found!");
            isFriend = await _relationRepository.IsFriendAsync(currentUser.Id, post.UserId);
            isMy = post.UserId == currentUser.Id;
        }

        if (post.Visibility == PostVisibility.Friends && !(isFriend || isMy))
            throw new ExternalException("Post not found!");

        if (post.Visibility == PostVisibility.Private && !isMy)
            throw new ExternalException("Post not found!");

        ICollection<PostCommentItemDTO> comments = _mapper.Map<ICollection<PostCommentItemDTO>>(
            await _repository.GetAllAsync(e =>
                e.PostId == postId && e.ParentId == null && !e.IsDeleted,
                page, count, false, "LikeCount", ["User"]
                )
            );

        if (current is not null)
        {
            ICollection<CommentLike> commentsLikes = await _likeRepository.GetAllAsync(e =>
                e.UserId == currentUser.Id && e.Comment.PostId == postId && e.Comment.ParentId == null,
                count: 0,
                includes: ["Comment"]
            );

            foreach (var comment in comments)
            {
                comment.IsLiked = commentsLikes.Any(cl => cl.CommentId == comment.Id);
            }
        }

        return comments;
    }

    public async Task<ICollection<PostCommentItemDTO>> GetAllRepliesAsync(long parentId, string? current = null, int count = 30, int page = 0)
    {
        PostComment comment = await _repository.GetOneAsync(e =>
            e.Id == parentId &&
            !e.IsDeleted, includes: ["User", "Post"]
        ) ?? throw new ExternalException("Comment not found!");

        if (comment.Post.IsDeleted)
            throw new ExternalException("Comment not found!");

        AppUser currentUser = new();
        bool isFriend = false;
        bool isMy = false;

        if (current is not null)
        {
            currentUser = await _userManager.FindByNameAsync(current) ?? throw new ExternalException("User not found!");
            isFriend = await _relationRepository.IsFriendAsync(currentUser.Id, comment.Post.UserId);
            isMy = comment.Post.UserId == currentUser.Id;
        }

        if (comment.Post.Visibility == PostVisibility.Friends && !(isFriend || isMy))
            throw new ExternalException("Comment not found!");

        if (comment.Post.Visibility == PostVisibility.Private && !isMy)
            throw new ExternalException("Comment not found!");

        ICollection<PostCommentItemDTO> replies = _mapper.Map<ICollection<PostCommentItemDTO>>(
            await _repository.GetAllAsync(e =>
                e.ParentId == comment.Id && !e.IsDeleted,
                page, count, false, "LikeCount", ["User"]
                )
            );

        if (current is not null)
        {
            ICollection<CommentLike> repliesLikes = await _likeRepository.GetAllAsync(e =>
                e.UserId == currentUser.Id && e.Comment.ParentId == parentId,
                count: 0,
                includes: ["Comment"]
            );

            foreach (var reply in replies)
            {
                reply.IsLiked = repliesLikes.Any(cl => cl.CommentId == reply.Id);
            }
        }

        return replies;
    }

    public async Task<UpdatePostCommentDTO> GetByIdForUpdateAsync(long id, string current)
    {
        PostComment comment = await _repository.GetOneAsync(e => e.Id == id && !e.IsDeleted, includes: ["User", "Post"]) ?? throw new ExternalException("Comment not found");
        if (comment.Post.IsDeleted)
            throw new ExternalException("Comment not found!");

        if (comment.User.UserName != current)
            throw new ExternalException("You have no access");
        
        return _mapper.Map<UpdatePostCommentDTO>(comment);
    }

    public async Task<UpdatePostCommentReplyDTO> GetReplyByIdForUpdateAsync(long id, string current)
    {
        PostComment comment = await _repository.GetOneAsync(e => e.Id == id && !e.IsDeleted, includes: ["User", "Post", "Parent"]) ?? throw new ExternalException("Comment not found");
        if (comment.Post.IsDeleted || comment.Parent.IsDeleted)
            throw new ExternalException("Comment not found!");

        if (comment.User.UserName != current)
            throw new ExternalException("You have no access");

        return _mapper.Map<UpdatePostCommentReplyDTO>(comment);
    }

    public async Task LikeAsync(long id, string current)
    {
        AppUser currentUser = await _userManager.FindByNameAsync(current) ?? throw new ExternalException("User not found!");
        PostComment comment = await _repository.GetOneAsync(e =>
            e.Id == id &&
            !e.IsDeleted, true, ["Post", "Parent"]) ?? throw new ExternalException("Comment not found!");

        if (comment.Post.IsDeleted || (comment.Parent is not null && comment.Parent.IsDeleted))
            throw new ExternalException("Comment not found!");

        bool isFriend = await _relationRepository.IsFriendAsync(currentUser.Id, comment.Post.UserId);
        bool isMy = comment.Post.UserId == currentUser.Id;

        if (comment.Post.Visibility == PostVisibility.Friends && !(isFriend || isMy))
            throw new ExternalException("Comment not found!");

        if (comment.Post.Visibility == PostVisibility.Private && !isMy)
            throw new ExternalException("Comment not found!");

        if (await _likeRepository.GetOneAsync(e => e.UserId == currentUser.Id && e.CommentId == id) is not null)
            throw new ExternalException("You already liked this post");

        await _likeRepository.CreateAsync(new CommentLike { UserId = currentUser.Id, CommentId = comment.Id });
        comment.LikeCount++;
    }

    public async Task UnlikeAsync(long id, string current)
    {
        AppUser currentUser = await _userManager.FindByNameAsync(current) ?? throw new ExternalException("User not found!");
        CommentLike like = await _likeRepository.GetOneAsync(e => e.UserId == currentUser.Id && e.CommentId == id) ?? throw new ExternalException("You have no likes for this comment");
        PostComment comment = await _repository.GetOneAsync(e =>
            e.Id == id &&
            !e.IsDeleted, true, ["Post", "Parent"]) ?? throw new ExternalException("Comment not found!");

        if (comment.Post.IsDeleted || (comment.Parent is not null && comment.Parent.IsDeleted))
            throw new ExternalException("Comment not found!");

        _likeRepository.Delete(like);
        comment.LikeCount--;
    }

    public async Task CreateAsync(CreatePostCommentDTO dto, string current)
    {
        AppUser currentUser = await _userManager.FindByNameAsync(current) ?? throw new ExternalException("User not found!");
        PostComment comment = _mapper.Map<PostComment>(dto);
        Post post = await _postRepository.GetOneAsync(e =>
            e.Id == comment.PostId &&
            !e.IsDeleted, true) ?? throw new ExternalException("Post not found!");

        bool isFriend = await _relationRepository.IsFriendAsync(currentUser.Id, post.UserId);
        bool isMy = post.UserId == currentUser.Id;

        if (post.Visibility == PostVisibility.Friends && !(isFriend || isMy))
            throw new ExternalException("Post not found!");

        if (post.Visibility == PostVisibility.Private && !isMy)
            throw new ExternalException("Post not found!");

        post.CommentCount++;
        comment.UserId = currentUser.Id;
        await _repository.CreateAsync(comment);
    }

    public async Task CreateReplyAsync(CreatePostCommentReplyDTO dto, string current)
    {
        AppUser currentUser = await _userManager.FindByNameAsync(current) ?? throw new ExternalException("User not found!");
        PostComment reply = _mapper.Map<PostComment>(dto);
        PostComment comment = await _repository.GetOneAsync(e =>
            e.Id == reply.ParentId &&
            !e.IsDeleted, true, "Post") ?? throw new ExternalException("Comment not found!");

        if (comment.Post.IsDeleted)
            throw new ExternalException("Comment not found!");

        bool isFriend = await _relationRepository.IsFriendAsync(currentUser.Id, comment.Post.UserId);
        bool isMy = comment.Post.UserId == currentUser.Id;

        if (comment.Post.Visibility == PostVisibility.Friends && !(isFriend || isMy))
            throw new ExternalException("Comment not found!");

        if (comment.Post.Visibility == PostVisibility.Private && !isMy)
            throw new ExternalException("Comment not found!");

        comment.ReplyCount++;
        comment.Post.CommentCount++;
        reply.UserId = currentUser.Id;
        reply.PostId = comment.PostId;
        await _repository.CreateAsync(reply);
    }

    public async Task UpdateAsync(UpdatePostCommentDTO dto, string current)
    {
        AppUser currentUser = await _userManager.FindByNameAsync(current) ?? throw new ExternalException("User not found!");
        PostComment comment = await _repository.GetOneAsync(e => e.Id == dto.Id && !e.IsDeleted, includes: ["User", "Post"]) ?? throw new ExternalException("Comment not found!");
        if (comment.Post.IsDeleted)
            throw new ExternalException("Comment not found!");

        if (currentUser.Id != comment.User.Id)
            throw new ExternalException("You have no access");

        PostComment editedComment = _mapper.Map(dto, comment);

        _repository.Update(editedComment);
    }

    public async Task UpdateReplyAsync(UpdatePostCommentReplyDTO dto, string current)
    {
        AppUser currentUser = await _userManager.FindByNameAsync(current) ?? throw new ExternalException("User not found!");
        PostComment reply = await _repository.GetOneAsync(e => e.Id == dto.Id && !e.IsDeleted, includes: ["User", "Post", "Parent"]) ?? throw new ExternalException("Comment not found!");
        if (reply.Post.IsDeleted || reply.Parent.IsDeleted)
            throw new ExternalException("Comment not found!");

        if (currentUser.Id != reply.User.Id)
            throw new ExternalException("You have no access");

        PostComment editedReply = _mapper.Map(dto, reply);

        _repository.Update(editedReply);
    }

    public async Task SoftDeleteAsync(long id, string username)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        PostComment comment = await _repository.GetOneAsync(e => e.Id == id && !e.IsDeleted, true, ["User", "Post", "Parent"]) ?? throw new ExternalException("Comment not found!");
        if (comment.Post.IsDeleted || (comment.Parent is not null && comment.Parent.IsDeleted))
            throw new ExternalException("Comment not found!");

        bool isPermitted =
            comment.UserId == user.Id ||
            comment.Post.UserId == user.Id;

        if (!isPermitted) throw new ExternalException("You have no access");

        comment.Post.CommentCount--;
        if (comment.Parent is not null) comment.Parent.ReplyCount--;

        _repository.SoftDelete(comment);
    }

    public async Task<int> SaveChangesAsync() => await _repository.SaveChangesAsync();
}
