using AutoMapper;
using Synaptics.Application.Commands.AppUser.ChangeAppUserInfo;
using Synaptics.Application.Commands.AppUser.LoginAppUser;
using Synaptics.Application.Commands.AppUser.RegisterAppUser;
using Synaptics.Application.Queries.AppUser.AISearchAppUser;
using Synaptics.Application.Queries.AppUser.GetAppUserInfo;
using Synaptics.Application.Queries.AppUser.GetAppUserProfile;
using Synaptics.Application.Queries.AppUser.SearchAppUser;
using Synaptics.Domain.Entities;

namespace Synaptics.Application.Profiles;

public class AppUserProfile : Profile
{
    public AppUserProfile()
    {
        CreateMap<RegisterAppUserCommand, AppUser>()
            .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
            .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpper()));

        CreateMap<LoginAppUserCommand, AppUser>()
            .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpper()));

        CreateMap<AppUser, SearchAppUserQueryResponse>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        CreateMap<AppUser, AISearchAppUserQueryResponse>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        CreateMap<ChangeAppUserInfoCommand, AppUser>();
        CreateMap<AppUser, GetAppUserInfoQueryResponse>();

        CreateMap<AppUser, GetAppUserProfileQueryResponse>();
    }
}
