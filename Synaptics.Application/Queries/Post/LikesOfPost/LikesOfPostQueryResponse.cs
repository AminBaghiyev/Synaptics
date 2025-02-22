namespace Synaptics.Application.Queries.Post.LikesOfPost;

public record LikesOfPostQueryResponse
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string ProfilePhotoPath { get; set; }
    public bool IsFollow { get; set; }
}
