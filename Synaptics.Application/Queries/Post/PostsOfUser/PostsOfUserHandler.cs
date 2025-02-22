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

namespace Synaptics.Application.Queries.Post.PostsOfUser;

public class PostsOfUserHandler : IRequestHandler<PostsOfUserQuery, Response>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IMapper _mapper;

    public PostsOfUserHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<Entities.AppUser> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Response> Handle(PostsOfUserQuery request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");

        Entities.AppUser? user = await _userManager.FindByNameAsync(request.UserName);
        if (user is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.UserNotExists
            };

        bool isMy = false;
        bool isFriend = false;
        string? currentUserId = null;

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
            isMy = user.Id == currentUser.Id;
            isFriend = isMy || await _unitOfWork.UserRelationRepository.IsFriendAsync(currentUser.Id, user.Id);
        }

        ICollection<Entities.Post> posts = await _unitOfWork.PostRepository.GetAllAsync(e =>
            e.UserId == user.Id &&
            !e.IsDeleted &&
            (e.Visibility == PostVisibility.Public ||
            (isFriend && e.Visibility == PostVisibility.Friends) ||
            (isMy && e.Visibility == PostVisibility.Private)),
        request.Page, 20, false, "UpdatedAt");
        if (posts.Count == 0)
            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                MessageCode = MessageCode.UserHasNoPosts
            };

        if (isMy)
        {
            List<PostsOfCurrentUserQueryResponse> responsePosts = _mapper.Map<List<PostsOfCurrentUserQueryResponse>>(posts);

            HashSet<long> likedPostIds = (await _unitOfWork.PostLikeRepository.GetAllAsQueryable(
                e => e.UserId == currentUserId && e.Post.UserId == user.Id,
                count: 0,
                includes: ["Post"]
            ).Select(pl => pl.PostId).ToListAsync(cancellationToken: cancellationToken)).ToHashSet();

            responsePosts.ForEach(post => post.IsLiked = likedPostIds.Contains(post.Id));

            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Data = responsePosts
            };
        }
        else
        {
            List<PostsOfUserQueryResponse> responsePosts = _mapper.Map<List<PostsOfUserQueryResponse>>(posts);

            if (currentUserId is not null)
            {
                HashSet<long> likedPostIds = (await _unitOfWork.PostLikeRepository.GetAllAsQueryable(
                    e => e.UserId == currentUserId && e.Post.UserId == user.Id,
                    count: 0,
                    includes: ["Post"]
                ).Select(pl => pl.PostId).ToListAsync(cancellationToken: cancellationToken)).ToHashSet();

                responsePosts.ForEach(post => post.IsLiked = likedPostIds.Contains(post.Id));
            }

            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Data = responsePosts
            };
        }
    }
}
