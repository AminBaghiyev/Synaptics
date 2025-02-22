using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.Common;
using Synaptics.Application.Interfaces;
using Synaptics.Domain.Entities;
using Synaptics.Domain.Enums;
using System.Net;
using System.Security.Claims;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Commands.PostComment.UnlikePostComment;

public class UnlikePostCommentHandler : IRequestHandler<UnlikePostCommentCommand, Response>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<Entities.AppUser> _userManager;

    public UnlikePostCommentHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<Entities.AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
    }

    public async Task<Response> Handle(UnlikePostCommentCommand request, CancellationToken cancellationToken)
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
                e.ParentId == null, true, ["Post"]);
            if (comment is null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    MessageCode = MessageCode.CommentNotExists
                };
            }

            CommentLike? like = await _unitOfWork.CommentLikeRepository.GetOneAsync(e => e.UserId == user.Id && e.CommentId == request.Id);
            if (like is null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    MessageCode = MessageCode.YouArentLikedComment
                };
            }

            comment.LikeCount--;
            _unitOfWork.CommentLikeRepository.Delete(like);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();

            return new Response
            {
                StatusCode = HttpStatusCode.Created
            };
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
