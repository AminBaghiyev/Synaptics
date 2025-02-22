using AutoMapper;
using Synaptics.Application.Queries.UserRelation.Followers;
using Synaptics.Application.Queries.UserRelation.Followings;
using Synaptics.Domain.Entities;

namespace Synaptics.Application.Profiles;

public class UserRelationProfile : Profile
{
    public UserRelationProfile()
    {
        CreateMap<UserRelation, FollowersQueryResponse>()
            .ForMember(desc => desc.UserName, opt => opt.MapFrom(src => src.Follower.UserName))
            .ForMember(desc => desc.FullName, opt => opt.MapFrom(src => $"{src.Follower.FirstName} {src.Follower.LastName}"))
            .ForMember(desc => desc.ProfilePhotoPath, opt => opt.MapFrom(src => src.Follower.ProfilePhotoPath))
            .ForMember(dest => dest.IsFollow, opt => opt.Ignore())
            .AfterMap((src, dest, context) =>
            {
                if (context.Items.TryGetValue("currentUserFollowingIds", out var followingIdsObj) && followingIdsObj is HashSet<string> currentUserFollowingIds)
                {
                    dest.IsFollow = currentUserFollowingIds.Contains(src.FollowerId);
                }
                else
                {
                    dest.IsFollow = false;
                }
            });

        CreateMap<UserRelation, FollowingsQueryResponse>()
            .ForMember(desc => desc.UserName, opt => opt.MapFrom(src => src.Following.UserName))
            .ForMember(desc => desc.FullName, opt => opt.MapFrom(src => $"{src.Following.FirstName} {src.Following.LastName}"))
            .ForMember(desc => desc.ProfilePhotoPath, opt => opt.MapFrom(src => src.Following.ProfilePhotoPath))
            .ForMember(dest => dest.IsFollow, opt => opt.Ignore())
            .AfterMap((src, dest, context) =>
            {
                if (context.Items.TryGetValue("currentUserFollowingIds", out var followingIdsObj) && followingIdsObj is HashSet<string> currentUserFollowingIds)
                {
                    dest.IsFollow = currentUserFollowingIds.Contains(src.FollowingId);
                }
                else
                {
                    dest.IsFollow = false;
                }
            });
    }
}
