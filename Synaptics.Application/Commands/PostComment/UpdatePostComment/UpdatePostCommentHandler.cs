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

namespace Synaptics.Application.Commands.PostComment.UpdatePostComment;

public class UpdatePostCommentHandler : IRequestHandler<UpdatePostCommentCommand, Response>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IMapper _mapper;

    public UpdatePostCommentHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<Entities.AppUser> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Response> Handle(UpdatePostCommentCommand request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");
        if (username is null)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.TokenNotFound
            };

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            Entities.AppUser? user = await _userManager.FindByNameAsync(username);
            if (user is null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    MessageCode = MessageCode.UserNotExists
                };
            }

            Entities.PostComment? comment = await _unitOfWork.PostCommentRepository.GetOneAsync(e =>
                e.Id == request.Id &&
                !e.IsDeleted &&
                !e.Post.IsDeleted &&
                e.ParentId == null, includes: ["Post"]);
            if (comment is null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    MessageCode = MessageCode.CommentNotExists
                };
            }

            bool isMy = comment.Post.UserId == user.Id;
            bool isFriend = isMy || await _unitOfWork.UserRelationRepository.IsFriendAsync(user.Id, comment.Post.UserId);

            if ((comment.Post.Visibility == PostVisibility.Friends && !isFriend) ||
                (comment.Post.Visibility == PostVisibility.Private && !isMy))
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    MessageCode = MessageCode.PostNotExists
                };
            }
            if (user.Id != comment.UserId)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new Response
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    MessageCode = MessageCode.YouArentAllowedEditComment
                };
            }

            Entities.PostComment editedComment = _mapper.Map(request, comment);
            _unitOfWork.PostCommentRepository.Update(editedComment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();

            return new Response
            {
                StatusCode = HttpStatusCode.OK
            };
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
