using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Synaptics.Application.Common;
using Synaptics.Application.Interfaces;
using Synaptics.Domain.Enums;
using System.Net;
using System.Security.Claims;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Queries.PostComment.CommentsOfPost;

public class CommentsOfPostHandler : IRequestHandler<CommentsOfPostQuery, Response>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IMapper _mapper;

    public CommentsOfPostHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<Entities.AppUser> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Response> Handle(CommentsOfPostQuery request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");

        string[] includes = username is not null ? ["User"] : [];
        Entities.Post? post = await _unitOfWork.PostRepository.GetOneAsync(e =>
            e.Id == request.PostId &&
            !e.IsDeleted, includes: includes
        );
        if (post is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.PostNotExists
            };

        string? currentUserId = null;
        bool isFriend = false;
        bool isMy = false;

        if (username is not null)
        {
            Entities.AppUser? currentUser = await _userManager.FindByNameAsync(username);
            if (currentUser is null)
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    MessageCode = MessageCode.UserNotExists
                };

            currentUserId = currentUser.Id;
            isMy = post.UserId == currentUser.Id;
            isFriend = isMy || await _unitOfWork.UserRelationRepository.IsFriendAsync(currentUser.Id, post.UserId);
        }

        if ((post.Visibility == PostVisibility.Friends && !isFriend) || (post.Visibility == PostVisibility.Private && !isMy))
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.PostNotExists
            };

        ICollection<CommentsOfPostQueryResponse> comments = _mapper.Map<ICollection<CommentsOfPostQueryResponse>>(
            await _unitOfWork.PostCommentRepository.GetAllAsync(e =>
                e.PostId == request.PostId && e.ParentId == null && !e.IsDeleted,
                request.Page, 30, false, "LikeCount", ["User"]
                )
            );

        if (currentUserId is not null)
        {
            HashSet<long> likedCommentIds = (await _unitOfWork.CommentLikeRepository.GetAllAsQueryable(e =>
                e.UserId == currentUserId && e.Comment.PostId == request.PostId && e.Comment.ParentId == null,
                count: 0,
                includes: ["Comment"]
            ).Select(cl => cl.CommentId).ToListAsync(cancellationToken: cancellationToken)).ToHashSet();

            foreach (var comment in comments)
            {
                comment.IsLiked = likedCommentIds.Contains(comment.Id);
            }
        }

        return new Response
        {
            StatusCode = HttpStatusCode.OK,
            Data = comments
        };
    }
}
