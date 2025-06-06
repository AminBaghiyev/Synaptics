﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Synaptics.Application.Common;
using Synaptics.Application.Common.Results;
using Synaptics.Application.Exceptions;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Domain.Enums;
using System.Net;
using Entities = Synaptics.Domain.Entities;

namespace Synaptics.Application.Commands.AppUser.RegisterAppUser;

public class RegisterAppUserHandler : IRequestHandler<RegisterAppUserCommand, Response>
{
    readonly IFileService _fileService;
    readonly UserManager<Entities.AppUser> _userManager;
    readonly IQdrantService _qdrantService;
    readonly IPyBridgeService _pyBridgeService;
    readonly IJWTTokenService _jwtTokenService;
    readonly IRedisService _redisService;
    readonly IUserDeviceInfoService _userDeviceInfoService;
    readonly IMapper _mapper;

    public RegisterAppUserHandler(IFileService fileService, UserManager<Entities.AppUser> userManager, IQdrantService qdrantService, IPyBridgeService pyBridgeService, IMapper mapper, IJWTTokenService jwtTokenService, IRedisService redisService, IUserDeviceInfoService userDeviceInfoService)
    {
        _fileService = fileService;
        _userManager = userManager;
        _qdrantService = qdrantService;
        _pyBridgeService = pyBridgeService;
        _mapper = mapper;
        _jwtTokenService = jwtTokenService;
        _redisService = redisService;
        _userDeviceInfoService = userDeviceInfoService;
    }

    public async Task<Response> Handle(RegisterAppUserCommand request, CancellationToken cancellationToken)
    {
        if (request.ProfilePhoto is not null && !_fileService.CheckType(request.ProfilePhoto, "image")) throw new Exception("Profile photo must be image!");
        if (request.ProfilePhoto is not null && !_fileService.CheckSize(request.ProfilePhoto, 5, FileSizeTypes.Mb)) throw new WrongFileTypeException("Profile photo size cannot exceed 5 MB!");
        if (request.CoverPhoto is not null && !_fileService.CheckType(request.CoverPhoto, "image")) throw new WrongFileTypeException("Cover photo must be image!");
        if (request.CoverPhoto is not null && !_fileService.CheckSize(request.CoverPhoto, 8, FileSizeTypes.Mb)) throw new WrongFileTypeException("Cover photo size cannot exceed 8 MB!");

        Entities.AppUser user = _mapper.Map<Entities.AppUser>(request);

        if (await _userManager.FindByEmailAsync(user.Email.ToUpper()) is not null)
            return new Response
            {
                StatusCode = HttpStatusCode.BadRequest,
                MessageCode = MessageCode.UserExistsThisEmail
            };

        if (await _userManager.FindByNameAsync(user.UserName.ToUpper()) is not null)
            return new Response
            {
                StatusCode = HttpStatusCode.BadRequest,
                MessageCode = MessageCode.UserExistsThisUserName
            };

        (PyBridgeResult pyRes, float[] selfDescriptionEmbedding) = await _pyBridgeService.EmbeddingAsync(user.SelfDescription);

        if (!pyRes.Succeeded)
            return new Response
            {
                StatusCode = HttpStatusCode.InternalServerError,
                MessageCode = MessageCode.SomethingWrong
            };

        IdentityResult res = await _userManager.CreateAsync(user, request.Password);

        if (!res.Succeeded)
            return new Response
            {
                StatusCode = HttpStatusCode.InternalServerError,
                MessageCode = MessageCode.SomethingWrong
            };

        QdrantResult qdrantRes = await _qdrantService.AddDataAsync("users", Guid.Parse(user.Id), selfDescriptionEmbedding);

        if (!qdrantRes.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            return new Response
            {
                StatusCode = HttpStatusCode.InternalServerError,
                MessageCode = MessageCode.SomethingWrong
            };
        }

        if (request.ProfilePhoto is not null) user.ProfilePhotoPath = await _fileService.SaveImageAsync(request.ProfilePhoto, "profile_photos");
        if (request.CoverPhoto is not null) user.CoverPhotoPath = await _fileService.SaveImageAsync(request.CoverPhoto, "cover_photos", 1500, 500);

        string refreshToken = _jwtTokenService.GenerateRefreshToken();
        string deviceInfo = JsonConvert.SerializeObject(_userDeviceInfoService.GetDeviceInfo());

        await _redisService.SetHashAsync($"{user.UserName}:refresh_tokens", refreshToken, deviceInfo, TimeSpan.FromDays(7));

        return new Response
        {
            StatusCode = HttpStatusCode.Created,
            Data = new
            {
                AccessToken = _jwtTokenService.GenerateToken([new("username", user.UserName)], TimeSpan.FromMinutes(15)),
                RefreshToken = _jwtTokenService.GenerateToken([new("token", refreshToken)], TimeSpan.FromDays(7))
            }
        };
    }
}
