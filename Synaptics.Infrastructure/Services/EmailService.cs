using Microsoft.Extensions.Configuration;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Infrastructure.Utilities;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Synaptics.Infrastructure.Services;

public class EmailService : IEmailService
{
    readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendChangePasswordAsync(string to, string token, string username, string fullname)
    {
        SmtpClient smtp = new(_configuration["Email:Host"], Convert.ToInt32(_configuration["Email:Port"]))
        {
            Credentials = new NetworkCredential(_configuration["Email:Login"], _configuration["Email:Passcode"]),
            EnableSsl = true
        };

        string body = await RazorViewRenderer.RenderViewToStringAsync("ChangePasswordTemplate",
            new {
                UserName = username,
                ResetUrl = $"{_configuration["ASPNETCORE_URLS"].Split(";")[0]}/user/reset-password?token={HttpUtility.UrlEncode(token)}&username={HttpUtility.UrlEncode(username)}"
            }
        );

        MailAddress from = new(_configuration["Email:Login"], "Synaptics");
        MailAddress destination = new(to, fullname);

        MailMessage message = new(from, destination)
        {
            Subject = "Password Reset Request",
            Body = body,
            IsBodyHtml = true
        };

        await smtp.SendMailAsync(message);
    }
}
