using AutoMapper;
using Synaptics.Application.DTOs;
using Synaptics.Domain.Entities;

namespace Synaptics.Application.Profiles;

public class AppUserProfile : Profile
{
    public AppUserProfile()
    {
        CreateMap<RegisterAppUserDTO, AppUser>()
            .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
            .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpper()))
            .ReverseMap();

        CreateMap<LoginAppUserDTO, AppUser>()
            .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpper()))
            .ReverseMap();

        CreateMap<SearchAppUserDTO, AppUser>()
            .ReverseMap()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        CreateMap<AISearchAppUserDTO, AppUser>()
            .ReverseMap()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        CreateMap<ChangeAppUserInfoDTO, AppUser>()
            .ReverseMap();

        CreateMap<GetAppUserProfileDTO, AppUser>()
            .ReverseMap();
    }
}
