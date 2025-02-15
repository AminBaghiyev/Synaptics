using MediatR;
using Microsoft.AspNetCore.Http;
using Synaptics.Application.Exceptions;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using Synaptics.Domain.Enums;
using System.Security.Claims;

namespace Synaptics.Application.Commands.AppUser.ChangeCoverPhotoAppUser;

public class ChangeCoverPhotoAppUserHandler : IRequestHandler<ChangeCoverPhotoAppUserCommand, string>
{
    readonly IAppUserService _userService;
    readonly IHttpContextAccessor _contextAccessor;
    readonly IFileService _fileService;

    public ChangeCoverPhotoAppUserHandler(IAppUserService userService, IHttpContextAccessor contextAccessor, IFileService fileService)
    {
        _userService = userService;
        _contextAccessor = contextAccessor;
        _fileService = fileService;
    }

    public async Task<string> Handle(ChangeCoverPhotoAppUserCommand request, CancellationToken cancellationToken)
    {
        string username = _contextAccessor.HttpContext?.User.FindFirstValue("username") ?? throw new ExternalException("Token not found!");

        if (!_fileService.CheckType(request.File.CoverPhoto, "image")) throw new WrongFileTypeException("Cover photo must be image!");
        if (!_fileService.CheckSize(request.File.CoverPhoto, 8, FileSizeTypes.Mb)) throw new WrongFileTypeException("Cover photo size cannot exceed 8 MB!");

        return await _userService.ChangeCoverPhotoAsync(username, request.File.CoverPhoto);
    }
}