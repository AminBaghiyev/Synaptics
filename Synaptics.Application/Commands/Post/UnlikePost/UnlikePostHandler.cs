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

namespace Synaptics.Application.Commands.Post.UnlikePost;

public class UnlikePostHandler : IRequestHandler<UnlikePostCommand, Response>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<Entities.AppUser> _userManager;

    public UnlikePostHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<Entities.AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
    }

    public async Task<Response> Handle(UnlikePostCommand request, CancellationToken cancellationToken)
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
            Entities.AppUser? currentUser = await _userManager.FindByNameAsync(username);
            if (currentUser is null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    MessageCode = MessageCode.UserNotExists
                };
            }

            Entities.Post? post = await _unitOfWork.PostRepository.GetOneAsync(e =>
                e.Id == request.PostId &&
                !e.IsDeleted, true);
            if (post is null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    MessageCode = MessageCode.PostNotExists
                };
            }

            bool isMy = post.UserId == currentUser.Id;
            bool isFriend = isMy || await _unitOfWork.UserRelationRepository.IsFriendAsync(currentUser.Id, post.UserId);

            if ((post.Visibility == PostVisibility.Friends && !isFriend) ||
                (post.Visibility == PostVisibility.Private && !isMy))
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    MessageCode = MessageCode.PostNotExists
                };
            }

            PostLike? like = await _unitOfWork.PostLikeRepository.GetOneAsync(e => e.UserId == currentUser.Id && e.PostId == request.PostId);
            if (like is null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    MessageCode = MessageCode.YouArentLikedPost
                };
            }

            post.LikeCount--;
            _unitOfWork.PostLikeRepository.Delete(like);
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
