using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.Common;
using Synaptics.Application.Interfaces;
using Entities = Synaptics.Domain.Entities;
using Synaptics.Domain.Enums;
using System.Net;
using System.Security.Claims;

namespace Synaptics.Application.Commands.PostComment.CreatePostComment;

public class CreatePostCommentHandler : IRequestHandler<CreatePostCommentCommand, Response>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IMapper _mapper;

    public CreatePostCommentHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<Entities.AppUser> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Response> Handle(CreatePostCommentCommand request, CancellationToken cancellationToken)
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

            Entities.PostComment comment = _mapper.Map<Entities.PostComment>(request);
            Entities.Post? post = await _unitOfWork.PostRepository.GetOneAsync(e =>
                e.Id == comment.PostId &&
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

            bool isMy = post.UserId == user.Id;
            bool isFriend = isMy || await _unitOfWork.UserRelationRepository.IsFriendAsync(user.Id, post.UserId);

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

            post.CommentCount++;
            comment.UserId = user.Id;
            await _unitOfWork.PostCommentRepository.CreateAsync(comment);
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
