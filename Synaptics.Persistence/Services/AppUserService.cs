using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using Synaptics.Domain.Entities;
using Synaptics.Domain.Results;
using Synaptics.Persistence.Exceptions;
using System.Security.Claims;

namespace Synaptics.Persistence.Services;

public class AppUserService : IAppUserService
{
    readonly IFileService _fileService;
    readonly IJWTTokenService _jwtTokenService;
    readonly UserManager<AppUser> _userManager;
    readonly IQdrantService _qdrantService;
    readonly IPyBridgeService _pyBridgeService;
    readonly IMapper _mapper;

    public AppUserService(UserManager<AppUser> userManager, IMapper mapper, IJWTTokenService jwtTokenService, IFileService fileService, IQdrantService qdrantService, IPyBridgeService pyBridgeService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _jwtTokenService = jwtTokenService;
        _fileService = fileService;
        _qdrantService = qdrantService;
        _pyBridgeService = pyBridgeService;
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

        (PyBridgeResult pyRes, float[] selfDescriptionEmbedding) = await _pyBridgeService.EmbeddingAsync(user.SelfDescription);

        if (!pyRes.Succeeded)
            throw new AppUserRegistrationFailedException();

        IdentityResult res = await _userManager.CreateAsync(user, dto.Password);

        if (!res.Succeeded) throw new AppUserRegistrationFailedException();

        QdrantResult qdrantRes = await _qdrantService.AddDataAsync("users", Guid.Parse(user.Id), selfDescriptionEmbedding);

        if (!qdrantRes.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            throw new AppUserRegistrationFailedException();
        }

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
        (PyBridgeResult pyRes, float[] selfDescriptionEmbedding) = await _pyBridgeService.EmbeddingAsync(dto.SelfDescription);

        if (!pyRes.Succeeded)
            throw new Exception(pyRes.ErrorMessage);

        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        AppUser oldUser = _mapper.Map<AppUser>(user);

        _mapper.Map(dto, user);

        IdentityResult res = await _userManager.UpdateAsync(user);

        if (!res.Succeeded) throw new Exception();

        QdrantResult qdrantRes = await _qdrantService.UpdateDataAsync("users", Guid.Parse(user.Id), selfDescriptionEmbedding);

        if (!qdrantRes.Succeeded)
        {
            await _userManager.UpdateAsync(oldUser);
            throw new Exception(qdrantRes.ErrorMessage);
        }
    }

    public async Task<GetAppUserProfileDTO> GetProfileAsync(string username)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");

        return _mapper.Map<GetAppUserProfileDTO>(user);
    }

    public async Task<ICollection<SearchAppUserDTO>> SearchUserAsync(string query, int limit = 20, int offset = 0)
    {
        string normalizedQuery = query.ToLower();

        ICollection<AppUser> users = await _userManager.Users
            .Where(u =>
                u.FirstName.ToLower().Contains(normalizedQuery) ||
                u.LastName.ToLower().Contains(normalizedQuery) ||
                u.UserName.ToLower().Contains(normalizedQuery))
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

        return _mapper.Map<List<SearchAppUserDTO>>(users);
    }

    public async Task<ICollection<AISearchAppUserDTO>> SearchUserWithAIAsync(string query, ulong limit = 10, ulong offset = 0)
    {
        (PyBridgeResult pyRes, float[] queryEmbedding) = await _pyBridgeService.EmbeddingAsync(query);

        if (!pyRes.Succeeded)
            throw new Exception(pyRes.ErrorMessage);

        (QdrantResult qdrantRes, ICollection<(string Id, float Score)> usersWithScores) = await _qdrantService.SearchCosineSimilarityAsync("users", queryEmbedding, limit, offset);

        if (!qdrantRes.Succeeded)
            throw new Exception(qdrantRes.ErrorMessage);

        Dictionary<string, AppUser> users = await _userManager.Users
            .Where(user => usersWithScores.Select(u => u.Id).Contains(user.Id))
            .ToDictionaryAsync(user => user.Id);

        return usersWithScores
            .Where(us => users.ContainsKey(us.Id))
            .OrderByDescending(us => us.Score)
            .Select(us => new AISearchAppUserDTO
            {
                UserName = users[us.Id].UserName,
                FullName = $"{users[us.Id].FirstName} {users[us.Id].LastName}",
                ProfilePhotoPath = users[us.Id].ProfilePhotoPath,
                SelfDescription = users[us.Id].SelfDescription,
                Score = us.Score
            })
            .ToList();
    }
}
