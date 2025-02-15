using MediatR;
using Synaptics.Application.Exceptions;
using Synaptics.Application.Interfaces;
using Synaptics.Domain.Enums;

namespace Synaptics.Application.Commands.AppUser.RegisterAppUser;

public class RegisterAppUserHandler : IRequestHandler<RegisterAppUserCommand, string>
{
    readonly IFileService _fileService;
    readonly IAppUserService _appUserService;

    public RegisterAppUserHandler(IAppUserService appUserService, IFileService fileService)
    {
        _appUserService = appUserService;
        _fileService = fileService;
    }

    public async Task<string> Handle(RegisterAppUserCommand request, CancellationToken cancellationToken)
    {
        if (request.AppUser.ProfilePhoto is not null && !_fileService.CheckType(request.AppUser.ProfilePhoto, "image")) throw new WrongFileTypeException("Profile photo must be image!");
        if (request.AppUser.ProfilePhoto is not null && !_fileService.CheckSize(request.AppUser.ProfilePhoto, 5, FileSizeTypes.Mb)) throw new WrongFileTypeException("Profile photo size cannot exceed 5 MB!");
        if (request.AppUser.CoverPhoto is not null && !_fileService.CheckType(request.AppUser.CoverPhoto, "image")) throw new WrongFileTypeException("Cover photo must be image!");
        if (request.AppUser.CoverPhoto is not null && !_fileService.CheckSize(request.AppUser.CoverPhoto, 8, FileSizeTypes.Mb)) throw new WrongFileTypeException("Cover photo size cannot exceed 8 MB!");

        return await _appUserService.RegisterAsync(request.AppUser);
    }
}
