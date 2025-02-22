using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.Common;
using Synaptics.Application.Interfaces;
using Synaptics.Domain.Enums;
using System.Net;
using System.Security.Claims;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Queries.PostCommentReply.PostCommentReplyForUpdate;

public class PostCommentReplyForUpdateHandler : IRequestHandler<PostCommentReplyForUpdateQuery, Response>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IMapper _mapper;

    public PostCommentReplyForUpdateHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<Entities.AppUser> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Response> Handle(PostCommentReplyForUpdateQuery request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");
        if (username is null)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.TokenNotFound
            };

        Entities.AppUser? user = await _userManager.FindByNameAsync(username);
        if (user is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.UserNotExists
            };

        Entities.PostComment? comment = await _unitOfWork.PostCommentRepository.GetOneAsync(e =>
            e.Id == request.Id &&
            !e.IsDeleted &&
            !e.Post.IsDeleted &&
            !e.Parent.IsDeleted, includes: ["Post", "Parent"]);
        if (comment is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.CommentNotExists
            };

        bool isMy = comment.Post.UserId == user.Id;
        bool isFriend = isMy || await _unitOfWork.UserRelationRepository.IsFriendAsync(user.Id, comment.Post.UserId);

        if ((comment.Post.Visibility == PostVisibility.Friends && !isFriend) || (comment.Post.Visibility == PostVisibility.Private && !isMy))
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.PostNotExists
            };
        if (comment.UserId != user.Id)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.YouArentAllowedEditComment
            };

        return new Response
        {
            StatusCode = HttpStatusCode.OK,
            Data = _mapper.Map<PostCommentReplyForUpdateQueryResponse>(comment)
        };
    }
}
