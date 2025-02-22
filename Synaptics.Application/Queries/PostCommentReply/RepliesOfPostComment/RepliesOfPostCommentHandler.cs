using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Synaptics.Application.Common;
using Synaptics.Application.Interfaces;
using Entities = Synaptics.Domain.Entities;
using Synaptics.Domain.Enums;
using System.Net;
using System.Security.Claims;

namespace Synaptics.Application.Queries.PostCommentReply.RepliesOfPostComment;

public class RepliesOfPostCommentHandler : IRequestHandler<RepliesOfPostCommentQuery, Response>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IMapper _mapper;

    public RepliesOfPostCommentHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<Entities.AppUser> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Response> Handle(RepliesOfPostCommentQuery request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");

        Entities.PostComment? comment = await _unitOfWork.PostCommentRepository.GetOneAsync(e =>
            e.Id == request.ParentId &&
            !e.IsDeleted &&
            !e.Post.IsDeleted &&
            (e.ParentId == null || !e.Parent.IsDeleted), includes: ["Post", "Parent"]
        );
        if (comment is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.CommentNotExists
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
            isMy = comment.Post.UserId == currentUser.Id;
            isFriend = isMy || await _unitOfWork.UserRelationRepository.IsFriendAsync(currentUser.Id, comment.Post.UserId);
        }

        if ((comment.Post.Visibility == PostVisibility.Friends && !isFriend) || (comment.Post.Visibility == PostVisibility.Private && !isMy))
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.CommentNotExists
            };

        ICollection<RepliesOfPostCommentQueryResponse> replies = _mapper.Map<ICollection<RepliesOfPostCommentQueryResponse>>(
            await _unitOfWork.PostCommentRepository.GetAllAsync(e =>
                e.ParentId == comment.Id && !e.IsDeleted,
                request.Page, 6, false, "CreatedAt", ["User"]
                )
            );

        if (currentUserId is not null)
        {
            HashSet<long> likedReplyIds = (await _unitOfWork.CommentLikeRepository.GetAllAsQueryable(e =>
                e.UserId == currentUserId && e.Comment.ParentId == comment.Id,
                count: 0,
                includes: ["Comment"]
            ).Select(cl => cl.CommentId).ToListAsync(cancellationToken: cancellationToken)).ToHashSet();

            foreach (var reply in replies)
            {
                reply.IsLiked = likedReplyIds.Contains(reply.Id);
            }
        }

        return new Response
        {
            StatusCode = HttpStatusCode.OK,
            Data = replies
        };
    }
}
