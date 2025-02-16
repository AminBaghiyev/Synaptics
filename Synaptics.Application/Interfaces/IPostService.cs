using Synaptics.Application.DTOs;

namespace Synaptics.Application.Interfaces;

public interface IPostService
{
    Task<PostItemOfCurrentUserDTO> GetByIdOfCurrentUserAsync(long id, string username);
    Task<UpdatePostDTO> GetByIdOfCurrentUserForUpdateAsync(long id, string username);
    Task<PostItemDTO> GetByIdAsync(long id, string username);
    Task<ICollection<PostItemOfCurrentUserDTO>> GetAllOfCurrentUserAsync(string username, int count = 10, int page = 0);
    Task<ICollection<PostItemDTO>> GetAllAsync(string username, int count = 10, int page = 0);
    Task CreateAsync(CreatePostDTO dto, string username);
    Task UpdateAsync(UpdatePostDTO dto, string username);
    Task RecoverAsync(long id, string username);
    Task SoftDeleteAsync(long id, string username);
    Task HardDeleteAsync(long id, string username);
    Task<int> SaveChangesAsync();
}
