namespace Synaptics.Application.Interfaces.Services;

public interface IEmailService
{
    Task SendChangePasswordAsync(string to, string token, string username, string fullname);
}
