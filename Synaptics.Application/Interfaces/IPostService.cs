using Synaptics.Application.DTOs;

namespace Synaptics.Application.Interfaces;

public interface IPostService
{
    Task<PostItemOfCurrentUserDTO> GetByIdOfCurrentUserAsync(long id, string username);
    Task<UpdatePostDTO> GetByIdOfCurrentUserForUpdateAsync(long id, string username);
    Task<PostItemDTO> GetByIdAsync(long id, string username, string? current = null);
    Task<ICollection<PostItemOfCurrentUserDTO>> GetAllOfCurrentUserAsync(string username, int count = 10, int page = 0);
    Task<ICollection<PostItemDTO>> GetAllAsync(string username, string? current = null, int count = 10, int page = 0);
    Task<ICollection<PostLikeUserDTO>> GetLikesOfPostAsync(long id, string username, string? current = null, int count = 50, int page = 0);
    Task LikeAsync(long id, string username, string current);
    Task UnlikeAsync(long id, string username, string current);
    Task CreateAsync(CreatePostDTO dto, string username);
    Task UpdateAsync(UpdatePostDTO dto, string username);
    Task RecoverAsync(long id, string username);
    Task SoftDeleteAsync(long id, string username);
    Task HardDeleteAsync(long id, string username);
    Task<int> SaveChangesAsync();
}
