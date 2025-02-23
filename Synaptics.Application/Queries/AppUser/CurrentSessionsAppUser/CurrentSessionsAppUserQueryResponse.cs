namespace Synaptics.Application.Queries.AppUser.CurrentSessionsAppUser;

public record CurrentSessionsAppUserQueryResponse
{
    public string Token { get; set; }
    public string DeviceName { get; set; }
    public string OperatingSystem { get; set; }
    public string IpAddress { get; set; }
    public bool IsCurrent { get; set; }
}
