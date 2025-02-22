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

namespace Synaptics.Application.Queries.UserRelation.Followings;

public class FollowingsHandler : IRequestHandler<FollowingsQuery, Response>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IMapper _mapper;

    public FollowingsHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<Entities.AppUser> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Response> Handle(FollowingsQuery request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");

        Entities.AppUser? user = await _userManager.FindByNameAsync(request.UserName);
        if (user is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.UserNotExists
            };

        ICollection<Entities.UserRelation> followings = await _unitOfWork.UserRelationRepository.GetAllAsync(e => e.FollowerId == user.Id, request.Page, includes: ["Following"]);

        HashSet<string> currentUserFollowingIds = [];
        if (username is not null)
        {
            Entities.AppUser? currentUser = await _userManager.FindByNameAsync(username);
            if (currentUser is null)
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    MessageCode = MessageCode.UserNotExists
                };

            ICollection<Entities.UserRelation> followingRelations = await _unitOfWork.UserRelationRepository.GetAllAsync(e => e.FollowerId == currentUser.Id);
            currentUserFollowingIds = new HashSet<string>(followingRelations.Select(e => e.FollowingId));
        }

        return new Response
        {
            StatusCode = HttpStatusCode.OK,
            Data = _mapper.Map<ICollection<FollowingsQueryResponse>>(followings, opt =>
                {
                    opt.Items["currentUserFollowingIds"] = currentUserFollowingIds;
                })
        };
    }
}
