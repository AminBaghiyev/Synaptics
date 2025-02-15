using Microsoft.AspNetCore.Http;
using Synaptics.Domain.Enums;

namespace Synaptics.Application.DTOs;

public record RegisterAppUserDTO
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string CheckPassword { get; set; }
    public IFormFile? ProfilePhoto { get; set; }
    public IFormFile? CoverPhoto { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; }
    public string Biography { get; set; }
    public string SelfDescription { get; set; }
}
