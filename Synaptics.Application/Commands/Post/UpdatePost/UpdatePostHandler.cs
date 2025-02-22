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

namespace Synaptics.Application.Commands.Post.UpdatePost;

public class UpdatePostHandler : IRequestHandler<UpdatePostCommand, Response>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IMapper _mapper;

    public UpdatePostHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<Entities.AppUser> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Response> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
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

            Entities.Post? post = await _unitOfWork.PostRepository.GetOneAsync(e => e.Id == request.Id && !e.IsDeleted);
            if (post is null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    MessageCode = MessageCode.PostNotExists
                };
            }
            if (post.UserId != user.Id)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new Response
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    MessageCode = MessageCode.YouArentAllowedEditPost
                };
            }

            Entities.Post editedPost = _mapper.Map(request, post);

            _unitOfWork.PostRepository.Update(editedPost);
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
