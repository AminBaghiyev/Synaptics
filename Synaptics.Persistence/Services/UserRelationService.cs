using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Synaptics.Application.DTOs;
using Synaptics.Application.Exceptions.Base;
using Synaptics.Application.Interfaces;
using Synaptics.Application.Interfaces.Repositories;
using Synaptics.Domain.Entities;

namespace Synaptics.Persistence.Services;

public class UserRelationService : IUserRelationService
{
    readonly UserManager<AppUser> _userManager;
    readonly IUserRelationRepository _repository;
    readonly IMapper _mapper;

    public UserRelationService(IUserRelationRepository repository, UserManager<AppUser> userManager, IMapper mapper)
    {
        _repository = repository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ICollection<FollowerDTO>> GetFollowersAsync(string username, string? current = null, int page = 0)
    {
        AppUser mainUser = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        ICollection<UserRelation> followers = await _repository.GetAllAsync(e => e.FollowingId == mainUser.Id, page, includes: ["Follower"]);

        HashSet<string> currentUserFollowingIds = [];
        if (current is not null)
        {
            AppUser currentUser = await _userManager.FindByNameAsync(current) ?? throw new ExternalException("User not found!");

            ICollection<UserRelation> followingRelations = await _repository.GetAllAsync(e => e.FollowerId == currentUser.Id);
            currentUserFollowingIds = new HashSet<string>(followingRelations.Select(e => e.FollowingId));
        }

        return _mapper.Map<ICollection<FollowerDTO>>(followers, opt =>
        {
            opt.Items["currentUserFollowingIds"] = currentUserFollowingIds;
        });
    }

    public async Task<ICollection<FollowingDTO>> GetFollowingsAsync(string username, string? current= null, int page = 0)
    {
        AppUser mainUser = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        ICollection<UserRelation> followings = await _repository.GetAllAsync(e => e.FollowerId == mainUser.Id, page, includes: ["Following"]);

        HashSet<string> currentUserFollowingIds = [];
        if (current is not null)
        {
            AppUser currentUser = await _userManager.FindByNameAsync(current) ?? throw new ExternalException("User not found!");

            ICollection<UserRelation> followingRelations = await _repository.GetAllAsync(e => e.FollowerId == currentUser.Id);
            currentUserFollowingIds = new HashSet<string>(followingRelations.Select(e => e.FollowingId));
        }

        return _mapper.Map<ICollection<FollowingDTO>>(followings, opt =>
        {
            opt.Items["currentUserFollowingIds"] = currentUserFollowingIds;
        });
    }

    public async Task FollowAsync(string username, string following)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        AppUser followTo = await _userManager.FindByNameAsync(following) ?? throw new ExternalException("User not found!");

        if (await _repository.GetOneAsync(e => e.FollowerId == user.Id && e.FollowingId == followTo.Id) is not null)
            throw new ExternalException("You are already following this user");

        await _repository.CreateAsync(new UserRelation
        {
            FollowerId = user.Id,
            FollowingId = followTo.Id
        });
    }

    public async Task UnfollowAsync(string username, string unfollowing)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        AppUser unfollowTo = await _userManager.FindByNameAsync(unfollowing) ?? throw new ExternalException("User not found!");

        UserRelation relation = await _repository.GetOneAsync(e => e.FollowerId == user.Id && e.FollowingId == unfollowTo.Id) ?? throw new ExternalException("You are not following this user");

        _repository.Delete(relation);
    }

    public async Task RemoveAsync(string username, string follower)
    {
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new ExternalException("User not found!");
        AppUser followFrom = await _userManager.FindByNameAsync(follower) ?? throw new ExternalException("User not found!");

        UserRelation relation = await _repository.GetOneAsync(e => e.FollowerId == followFrom.Id && e.FollowingId == user.Id) ?? throw new ExternalException("This user is not following you");

        _repository.Delete(relation);
    }

    public async Task<int> SaveChangesAsync() => await _repository.SaveChangesAsync();
}
