using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.Common;
using Synaptics.Application.Exceptions;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Domain.Enums;
using System.Net;
using System.Security.Claims;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Commands.AppUser.ChangeCoverPhotoAppUser;

public class ChangeCoverPhotoAppUserHandler : IRequestHandler<ChangeCoverPhotoAppUserCommand, Response>
{
    readonly IHttpContextAccessor _contextAccessor;
    readonly IFileService _fileService;
    readonly UserManager<Entities.AppUser> _userManager;

    public ChangeCoverPhotoAppUserHandler(IHttpContextAccessor contextAccessor, IFileService fileService, UserManager<Entities.AppUser> userManager)
    {
        _contextAccessor = contextAccessor;
        _fileService = fileService;
        _userManager = userManager;
    }

    public async Task<Response> Handle(ChangeCoverPhotoAppUserCommand request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");
        if (username is null)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.TokenNotFound
            };

        if (!_fileService.CheckType(request.CoverPhoto, "image")) throw new WrongFileTypeException("Cover photo must be image!");
        if (!_fileService.CheckSize(request.CoverPhoto, 8, FileSizeTypes.Mb)) throw new WrongFileTypeException("Cover photo size cannot exceed 8 MB!");

        Entities.AppUser? user = await _userManager.FindByNameAsync(username);
        if (user is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.UserNotExists
            };

        if (user.CoverPhotoPath is not null) File.Delete(Path.Combine(Path.GetFullPath("wwwroot"), "uploads", "cover_photos", user.CoverPhotoPath));
        user.CoverPhotoPath = await _fileService.SaveImageAsync(request.CoverPhoto, "cover_photos", 1500, 500);

        IdentityResult res = await _userManager.UpdateAsync(user);

        if (!res.Succeeded)
            return new Response
            {
                StatusCode = HttpStatusCode.InternalServerError,
                MessageCode = MessageCode.SomethingWrong
            };

        return new Response
        {
            StatusCode = HttpStatusCode.OK,
            Data = user.CoverPhotoPath
        };
    }
}