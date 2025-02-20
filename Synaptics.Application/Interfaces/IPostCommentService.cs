using Synaptics.Application.DTOs;

namespace Synaptics.Application.Interfaces;

public interface IPostCommentService
{
    Task<ICollection<PostCommentItemDTO>> GetAllAsync(long postId, string? current = null, int count = 30, int page = 0);
    Task<ICollection<PostCommentItemDTO>> GetAllRepliesAsync(long parentId, string? current = null, int count = 30, int page = 0);
    Task<UpdatePostCommentDTO> GetByIdForUpdateAsync(long id, string current);
    Task<UpdatePostCommentReplyDTO> GetReplyByIdForUpdateAsync(long id, string current);
    Task LikeAsync(long id, string current);
    Task UnlikeAsync(long id, string current);
    Task CreateAsync(CreatePostCommentDTO dto, string current);
    Task CreateReplyAsync(CreatePostCommentReplyDTO dto, string current);
    Task UpdateAsync(UpdatePostCommentDTO dto, string current);
    Task UpdateReplyAsync(UpdatePostCommentReplyDTO dto, string current);
    Task SoftDeleteAsync(long id, string username);
    Task<int> SaveChangesAsync();
}
