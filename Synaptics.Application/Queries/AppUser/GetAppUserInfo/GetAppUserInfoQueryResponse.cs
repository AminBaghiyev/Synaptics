using Synaptics.Domain.Enums;

namespace Synaptics.Application.Queries.AppUser.GetAppUserInfo;

public record GetAppUserInfoQueryResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public Gender Gender { get; set; }
    public string Biography { get; set; }
    public string SelfDescription { get; set; }
}
