using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using Synaptics.Application.Interfaces.Repositories;
using Synaptics.Domain.Entities;
using Synaptics.Domain.Enums;

namespace Synaptics.Persistence.Services;

public class PostService : IPostService
{
    readonly UserManager<AppUser> _userManager;
    readonly IPostRepository _repository;
    readonly IPostLikeRepository _postLikeRepository;
    readonly IUserRelationRepository _relationRepository;
    readonly IMapper _mapper;

    public PostService(IPostRepository repository, IMapper mapper, UserManager<AppUser> userManager, IUserRelationRepository relationRepository, IPostLikeRepository postLikeRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _userManager = userManager;
        _relationRepository = relationRepository;
        _postLikeRepository = postLikeRepository;
    }

    public async Task<ICollection<PostItemDTO>> GetAllAsync(string username, string? current = null, int count = 10, int page = 0)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        AppUser currentUser = new();
        bool isFriend = false;

        if (current is not null)
        {
            currentUser = await _userManager.FindByNameAsync(current) ?? throw new ExternalException("User not found!");

            isFriend = await _relationRepository.IsFriendAsync(currentUser.Id, user.Id);
        }

        ICollection<PostItemDTO> posts = _mapper.Map<ICollection<PostItemDTO>>(await _repository.GetAllAsync(e =>
            e.UserId == user.Id &&
            !e.IsDeleted &&
            (e.Visibility == PostVisibility.Public || (isFriend && e.Visibility == PostVisibility.Friends)),
            page, count, false, "UpdatedAt"));

        if (current is not null)
        {
            ICollection<PostLike> postLikes = await _postLikeRepository.GetAllAsync(e =>
                e.UserId == currentUser.Id && e.Post.UserId == user.Id,
                count: 0,
                includes: ["Post"]
            );

            foreach (var post in posts)
            {
                post.IsLiked = postLikes.Any(pl => pl.PostId == post.Id);
            }
        }

        return posts;
    }
        
    public async Task<ICollection<PostItemOfCurrentUserDTO>> GetAllOfCurrentUserAsync(string username, int count = 10, int page = 0)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");

        ICollection<PostItemOfCurrentUserDTO> posts = _mapper.Map<ICollection<PostItemOfCurrentUserDTO>>(
            await _repository.GetAllAsync(e =>
                e.UserId == user.Id &&
                !e.IsDeleted, page, count, false, "UpdatedAt"));

        
        ICollection<PostLike> postLikes = await _postLikeRepository.GetAllAsync(e =>
            e.UserId == user.Id && e.Post.UserId == user.Id,
            count: 0,
            includes: ["Post"]
        );

        foreach (var post in posts)
        {
            post.IsLiked = postLikes.Any(pl => pl.PostId == post.Id);
        }
        

        return posts;
    }

    public async Task<UpdatePostDTO> GetByIdOfCurrentUserForUpdateAsync(long id, string username)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        
        return _mapper.Map<UpdatePostDTO>(await _repository.GetOneAsync(e => e.Id == id && !e.IsDeleted));
    }

    public async Task<PostItemDTO> GetByIdAsync(long id, string username, string? current = null)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        bool isFriend = false;
        bool isLiked = false;

        if (current is not null)
        {
            AppUser currentUser = await _userManager.FindByNameAsync(current) ?? throw new ExternalException("User not found!");

            isFriend = await _relationRepository.IsFriendAsync(currentUser.Id, user.Id);
            isLiked = await _postLikeRepository.GetOneAsync(e => e.UserId == currentUser.Id && e.PostId == id) is not null;
        }

        PostItemDTO post = _mapper.Map<PostItemDTO>(await _repository.GetOneAsync(e =>
            e.Id == id &&
            !e.IsDeleted &&
            (e.Visibility == PostVisibility.Public || (isFriend && e.Visibility == PostVisibility.Friends))) ?? throw new ExternalException("Post not found!"));

        post.IsLiked = isLiked;

        return post;
    }

    public async Task<PostItemOfCurrentUserDTO> GetByIdOfCurrentUserAsync(long id, string username)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");

        PostItemOfCurrentUserDTO post = _mapper.Map<PostItemOfCurrentUserDTO>(await _repository.GetOneAsync(e => e.Id == id) ?? throw new ExternalException("Post not found!"));

        post.IsLiked = await _postLikeRepository.GetOneAsync(e => e.UserId == user.Id && e.PostId == id) is not null;

        return post;
    }

    public async Task<ICollection<PostLikeUserDTO>> GetLikesOfPostAsync(long id, string username, string? current = null, int count = 50, int page = 0)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        bool isFriend = false;
        bool isMy = false;

        if (current is not null)
        {
            AppUser currentUser = await _userManager.FindByNameAsync(current) ?? throw new ExternalException("User not found!");

            isFriend = await _relationRepository.IsFriendAsync(currentUser.Id, user.Id);
            isMy = user.Id == currentUser.Id;
            current = currentUser.Id;
        }

        Post post = await _repository.GetOneAsync(e =>
            e.Id == id &&
            !e.IsDeleted &&
            (e.Visibility == PostVisibility.Public ||
            ((isFriend || isMy) && e.Visibility == PostVisibility.Friends) ||
            (isMy && e.Visibility == PostVisibility.Private))) ?? throw new ExternalException("Post not found!");

        ICollection<PostLikeUserDTO> likes = _mapper.Map<ICollection<PostLikeUserDTO>>(await _postLikeRepository.GetAllAsync(e => e.Post.Id == id, page, count, includes: ["User"]));

        if (current is not null)
        {
            foreach (PostLikeUserDTO like in likes)
            {
                like.IsFollow = await _relationRepository.GetOneAsync(e => e.FollowerId == current && e.FollowingId == like.Id) is not null;
            }
        }

        return likes;
    }

    public async Task LikeAsync(long id, string username, string current)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        AppUser currentUser = await _userManager.FindByNameAsync(current) ?? throw new ExternalException("User not found!");
        if (await _postLikeRepository.GetOneAsync(e => e.UserId == currentUser.Id && e.PostId == id) is not null)
            throw new ExternalException("You already liked this post");

        bool isFriend = await _relationRepository.IsFriendAsync(currentUser.Id, user.Id);
        bool isMy = user.Id == currentUser.Id;

        Post post = await _repository.GetOneAsync(e =>
            e.Id == id &&
            !e.IsDeleted &&
            (e.Visibility == PostVisibility.Public ||
            ((isFriend || isMy) && e.Visibility == PostVisibility.Friends) ||
            (isMy && e.Visibility == PostVisibility.Private)), true) ?? throw new ExternalException("Post not found!");
        
        await _postLikeRepository.CreateAsync(new PostLike { UserId = currentUser.Id, PostId = post.Id});
        post.LikeCount++;
    }

    public async Task UnlikeAsync(long id, string username, string current)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        AppUser currentUser = await _userManager.FindByNameAsync(current) ?? throw new ExternalException("User not found!");
        PostLike like = await _postLikeRepository.GetOneAsync(e => e.UserId == currentUser.Id && e.PostId == id) ?? throw new ExternalException("You have no likes for this post");

        bool isFriend = await _relationRepository.IsFriendAsync(currentUser.Id, user.Id);
        bool isMy = user.Id == currentUser.Id;

        Post post = await _repository.GetOneAsync(e =>
            e.Id == id &&
            !e.IsDeleted &&
            (e.Visibility == PostVisibility.Public ||
            ((isFriend || isMy) && e.Visibility == PostVisibility.Friends) ||
            (isMy && e.Visibility == PostVisibility.Private)), true) ?? throw new ExternalException("Post not found!");

        _postLikeRepository.Delete(like);
        post.LikeCount--;
    }

    public async Task CreateAsync(CreatePostDTO dto, string username)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        Post post = _mapper.Map<Post>(dto);
        post.User = user;

        await _repository.CreateAsync(post);
    }

    public async Task UpdateAsync(UpdatePostDTO dto, string username)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        Post post = await _repository.GetOneAsync(e => e.Id == dto.Id && !e.IsDeleted) ?? throw new ExternalException("Post not found!");
        Post editedPost = _mapper.Map(dto, post);

        _repository.Update(editedPost);
    }    

    public async Task RecoverAsync(long id, string username)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        Post post = await _repository.GetOneAsync(e => e.Id == id && e.IsDeleted) ?? throw new ExternalException("Post not found!");
        post.IsDeleted = false;

        _repository.Update(post);
    }

    public async Task SoftDeleteAsync(long id, string username)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        Post post = await _repository.GetOneAsync(e => e.Id == id && !e.IsDeleted) ?? throw new ExternalException("Post not found!");

        _repository.SoftDelete(post);
    }

    public async Task HardDeleteAsync(long id, string username)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        Post post = await _repository.GetOneAsync(e => e.Id == id && e.IsDeleted) ?? throw new ExternalException("Post not found!");

        _repository.HardDelete(post);
    }

    public async Task<int> SaveChangesAsync() => await _repository.SaveChangesAsync();
}
