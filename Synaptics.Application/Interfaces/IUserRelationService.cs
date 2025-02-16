using Synaptics.Application.DTOs;

namespace Synaptics.Application.Interfaces;

public interface IUserRelationService
{
    Task<ICollection<FollowerDTO>> GetFollowersAsync(string username, string? current = null, int page = 0);
    Task<ICollection<FollowingDTO>> GetFollowingsAsync(string username, string? current= null, int page = 0);
    Task FollowAsync(string username, string following);
    Task UnfollowAsync(string username, string unfollowing);
    Task RemoveAsync(string username, string follower);
    Task<int> SaveChangesAsync();
}
