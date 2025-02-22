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

namespace Synaptics.Application.Queries.Post.PostForUpdate;

public class PostForUpdateHandler : IRequestHandler<PostForUpdateQuery, Response>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IMapper _mapper;

    public PostForUpdateHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<Entities.AppUser> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Response> Handle(PostForUpdateQuery request, CancellationToken cancellationToken)
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

        Entities.Post? post = await _unitOfWork.PostRepository.GetOneAsync(e => e.Id == request.Id && !e.IsDeleted);
        if (post is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.PostNotExists
            };
        if (post.UserId != user.Id)
            return new Response
            {
                StatusCode = HttpStatusCode.Forbidden,
                MessageCode = MessageCode.YouArentAllowedEditPost
            };

        return new Response
        {
            StatusCode = HttpStatusCode.OK,
            Data = _mapper.Map<PostForUpdateQueryResponse>(post)
        };
    }
}
