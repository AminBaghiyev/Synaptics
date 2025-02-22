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

namespace Synaptics.Application.Commands.AppUser.ChangeProfilePhotoAppUser;

public class ChangeProfilePhotoAppUserHandler : IRequestHandler<ChangeProfilePhotoAppUserCommand, Response>
{
    readonly IHttpContextAccessor _contextAccessor;
    readonly IFileService _fileService;
    readonly UserManager<Entities.AppUser> _userManager;

    public ChangeProfilePhotoAppUserHandler(IHttpContextAccessor contextAccessor, IFileService fileService, UserManager<Entities.AppUser> userManager)
    {
        _contextAccessor = contextAccessor;
        _fileService = fileService;
        _userManager = userManager;
    }

    public async Task<Response> Handle(ChangeProfilePhotoAppUserCommand request, CancellationToken cancellationToken)
    {
        string? username = _contextAccessor.HttpContext?.User.FindFirstValue("username");
        if (username is null)
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                MessageCode = MessageCode.TokenNotFound
            };

        if (!_fileService.CheckType(request.ProfilePhoto, "image")) throw new WrongFileTypeException("Profile photo must be image!");
        if (!_fileService.CheckSize(request.ProfilePhoto, 5, FileSizeTypes.Mb)) throw new WrongFileTypeException("Profile photo size cannot exceed 5 MB!");

        Entities.AppUser? user = await _userManager.FindByNameAsync(username.ToUpper());
        if (user is null)
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                MessageCode = MessageCode.UserNotExists
            };

        if (user.ProfilePhotoPath is not null) File.Delete(Path.Combine(Path.GetFullPath("wwwroot"), "uploads", "profile_photos", user.ProfilePhotoPath));
        user.ProfilePhotoPath = await _fileService.SaveImageAsync(request.ProfilePhoto, "profile_photos");

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
            Data = user.ProfilePhotoPath
        };
    }
}