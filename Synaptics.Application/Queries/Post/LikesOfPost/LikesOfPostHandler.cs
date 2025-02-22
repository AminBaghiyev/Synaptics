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

namespace Synaptics.Application.Queries.Post.LikesOfPost;

public class LikesOfPostHandler : IRequestHandler<LikesOfPostQuery, Response>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IMapper _mapper;

    public LikesOfPostHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<Entities.AppUser> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Response> Handle(LikesOfPostQuery request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");

        bool isFriend = false;
        bool isMy = false;
        string? currentUserId = null;

        Entities.Post? post = await _unitOfWork.PostRepository.GetOneAsync(e =>
            e.Id == request.PostId &&
            !e.IsDeleted);
        if (post is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.PostNotExists
            };

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

        ICollection<LikesOfPostQueryResponse> likes = _mapper.Map<ICollection<LikesOfPostQueryResponse>>(await _unitOfWork.PostLikeRepository.GetAllAsync(e => e.Post.Id == request.PostId, request.Page, 30, includes: ["User"]));

        if (currentUserId is not null)
        {
            foreach (LikesOfPostQueryResponse like in likes)
            {
                like.IsFollow = await _unitOfWork.UserRelationRepository.GetOneAsync(e => e.FollowerId == currentUserId && e.FollowingId == like.UserId) is not null;
            }
        }

        return new Response
        {
            StatusCode = HttpStatusCode.OK,
            Data = likes
        };
    }
}
