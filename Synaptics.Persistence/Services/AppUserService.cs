using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using Synaptics.Domain.Entities;
using Synaptics.Persistence.Exceptions;
using System.Security.Claims;

namespace Synaptics.Persistence.Services;

public class AppUserService : IAppUserService
{
    readonly IFileService _fileService;
    readonly IJWTTokenService _jwtTokenService;
    readonly UserManager<AppUser> _userManager;
    readonly IMapper _mapper;

    public AppUserService(UserManager<AppUser> userManager, IMapper mapper, IJWTTokenService jwtTokenService, IFileService fileService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _jwtTokenService = jwtTokenService;
        _fileService = fileService;
    }

    public async Task<string> LoginAsync(LoginAppUserDTO dto)
    {
        AppUser user = await _userManager.FindByNameAsync(dto.UserName) ?? throw new AppUserCredentialsWrongException();

        bool res = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!res) throw new AppUserCredentialsWrongException();

        IEnumerable<Claim> claims =
        [
            new("sub", user.Id),
            new("firstname", user.FirstName),
            new("lastname", user.LastName),
            new("username", user.UserName),
            new("email", user.Email),
            new("gender", user.Gender.ToString())
        ];

        return _jwtTokenService.GenerateToken(claims);
    }

    public async Task<string> RegisterAsync(RegisterAppUserDTO dto)
    {
        AppUser user = _mapper.Map<AppUser>(dto);
        
        if (await _userManager.FindByEmailAsync(user.NormalizedEmail) is not null)
            throw new AppUserExistsException("User with this email already exists!");

        if (await _userManager.FindByNameAsync(user.NormalizedUserName) is not null)
            throw new AppUserExistsException("User with this username already exists!");

        IdentityResult res = await _userManager.CreateAsync(user, dto.Password);

        if (!res.Succeeded) throw new AppUserRegistrationFailedException();

        if (dto.ProfilePhoto is not null) user.ProfilePhotoPath = await _fileService.SaveImageAsync(dto.ProfilePhoto, "profile_photos");
        if (dto.CoverPhoto is not null) user.CoverPhotoPath = await _fileService.SaveImageAsync(dto.CoverPhoto, "cover_photos", 1500, 500);

        IEnumerable<Claim> claims =
        [
            new("sub", user.Id),
            new("firstname", user.FirstName),
            new("lastname", user.LastName),
            new("username", user.UserName),
            new("email", user.Email),
            new("gender", user.Gender.ToString())
        ];

        return _jwtTokenService.GenerateToken(claims);
    }

    public async Task<string> ChangeProfilePhotoAsync(string username, IFormFile photo)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");

        if (user.ProfilePhotoPath is not null) File.Delete(Path.Combine(Path.GetFullPath("wwwroot"), "uploads", "profile_photos", user.ProfilePhotoPath));
        user.ProfilePhotoPath = await _fileService.SaveImageAsync(photo, "profile_photos");

        IdentityResult res = await _userManager.UpdateAsync(user);

        if (!res.Succeeded) throw new Exception();

        return user.ProfilePhotoPath;
    }

    public async Task<string> ChangeCoverPhotoAsync(string username, IFormFile photo)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");

        if (user.CoverPhotoPath is not null) File.Delete(Path.Combine(Path.GetFullPath("wwwroot"), "uploads", "cover_photos", user.CoverPhotoPath));
        user.CoverPhotoPath = await _fileService.SaveImageAsync(photo, "cover_photos", 1500, 500);

        IdentityResult res = await _userManager.UpdateAsync(user);

        if (!res.Succeeded) throw new Exception();

        return user.CoverPhotoPath;
    }

    public async Task ChangeInfoAsync(string username, ChangeAppUserInfoDTO dto)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");

        _mapper.Map(dto, user);

        IdentityResult res = await _userManager.UpdateAsync(user);

        if (!res.Succeeded) throw new Exception();
    }

    public async Task<GetAppUserProfileDTO> GetProfileAsync(string username)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");

        return _mapper.Map<GetAppUserProfileDTO>(user);
    }

    public async Task<ICollection<SearchAppUserDTO>> SearchUserAsync(string query)
    {
        string normalizedQuery = query.ToLower();

        ICollection<AppUser> users = await _userManager.Users
            .Where(u =>
                u.FirstName.ToLower().Contains(normalizedQuery) ||
                u.LastName.ToLower().Contains(normalizedQuery) ||
                u.UserName.ToLower().Contains(normalizedQuery))
            .ToListAsync();

        return _mapper.Map<List<SearchAppUserDTO>>(users);
    }
}
