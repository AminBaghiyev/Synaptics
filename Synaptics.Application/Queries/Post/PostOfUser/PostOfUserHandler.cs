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

namespace Synaptics.Application.Queries.Post.PostOfUser;

public class PostOfUserHandler : IRequestHandler<PostOfUserQuery, Response>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IMapper _mapper;

    public PostOfUserHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<Entities.AppUser> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Response> Handle(PostOfUserQuery request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");

        bool isMy = false;
        bool isFriend = false;
        bool isLiked = false;

        Entities.Post? post = await _unitOfWork.PostRepository.GetOneAsync(e =>
            e.Id == request.Id &&
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

            isMy = post.UserId == currentUser.Id;
            isFriend = isMy || await _unitOfWork.UserRelationRepository.IsFriendAsync(currentUser.Id, post.UserId);
            isLiked = await _unitOfWork.PostLikeRepository.GetOneAsync(e => e.UserId == currentUser.Id && e.PostId == request.Id) is not null;
        }

        if ((post.Visibility == PostVisibility.Friends && !isFriend) || (post.Visibility == PostVisibility.Private && !isMy))
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.PostNotExists
            };

        return new Response
        {
            StatusCode = HttpStatusCode.OK,
            Data = isMy ?
                _mapper.Map<PostOfCurrentUserQueryResponse>(post) :
                _mapper.Map<PostOfUserQueryResponse>(post)
        };
    }
}
