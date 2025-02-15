using Microsoft.AspNetCore.Http;
using Synaptics.Application.DTOs;

namespace Synaptics.Application.Interfaces;

public interface IAppUserService
{
    Task<string> RegisterAsync(RegisterAppUserDTO dto);
    Task<string> LoginAsync(LoginAppUserDTO dto);
    Task<string> ChangeProfilePhotoAsync(string username, IFormFile photo);
    Task<string> ChangeCoverPhotoAsync(string username, IFormFile photo);
    Task ChangeInfoAsync(string username, ChangeAppUserInfoDTO dto);
    Task<GetAppUserProfileDTO> GetProfileAsync(string username);
    Task<ICollection<SearchAppUserDTO>> SearchUserAsync(string query);
}
