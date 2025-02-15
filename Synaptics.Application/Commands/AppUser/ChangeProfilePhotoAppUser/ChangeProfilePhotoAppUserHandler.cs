using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Exceptions;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using Synaptics.Domain.Enums;
using System.Security.Claims;

namespace Synaptics.Application.Commands.AppUser.ChangeProfilePhotoAppUser;

public class ChangeProfilePhotoAppUserHandler : IRequestHandler<ChangeProfilePhotoAppUserCommand, string>
{
    readonly IAppUserService _userService;
    readonly IHttpContextAccessor _contextAccessor;
    readonly IFileService _fileService;

    public ChangeProfilePhotoAppUserHandler(IAppUserService userService, IHttpContextAccessor contextAccessor, IFileService fileService)
    {
        _userService = userService;
        _contextAccessor = contextAccessor;
        _fileService = fileService;
    }

    public async Task<string> Handle(ChangeProfilePhotoAppUserCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        if (!_fileService.CheckType(request.File.ProfilePhoto, "image")) throw new WrongFileTypeException("Profile photo must be image!");
        if (!_fileService.CheckSize(request.File.ProfilePhoto, 5, FileSizeTypes.Mb)) throw new WrongFileTypeException("Profile photo size cannot exceed 5 MB!");

        return await _userService.ChangeProfilePhotoAsync(username, request.File.ProfilePhoto);
    }
}