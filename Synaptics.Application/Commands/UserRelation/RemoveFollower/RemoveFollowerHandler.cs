using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.Common;
using Synaptics.Application.Interfaces;
using Synaptics.Domain.Enums;
using System.Net;
using System.Security.Claims;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Commands.UserRelation.RemoveFollower;

public class RemoveFollowerHandler : IRequestHandler<RemoveFollowerCommand, Response>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<Entities.AppUser> _userManager;

    public RemoveFollowerHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<Entities.AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
    }

    public async Task<Response> Handle(RemoveFollowerCommand request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");
        if (username is null)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.TokenNotFound
            };

        if (username.ToUpper() == request.Follower.ToUpper())
            return new Response
            {
                StatusCode = HttpStatusCode.BadRequest,
                MessageCode = MessageCode.YouDoNotFollowYourselfAnyway
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


            Entities.AppUser? followFrom = await _userManager.FindByNameAsync(request.Follower);
            if (followFrom is null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    MessageCode = MessageCode.UserNotExists
                };
            }

            Entities.UserRelation? relation = await _unitOfWork.UserRelationRepository.GetOneAsync(e => e.FollowerId == followFrom.Id && e.FollowingId == user.Id);
            if (relation is null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    MessageCode = MessageCode.UserIsntFollowYou
                };
            }

            _unitOfWork.UserRelationRepository.Delete(relation);
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
