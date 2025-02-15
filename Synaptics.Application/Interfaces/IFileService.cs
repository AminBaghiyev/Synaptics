using Microsoft.AspNetCore.Http;
using Synaptics.Domain.Enums;

namespace Synaptics.Application.Interfaces;

public interface IFileService
{
    Task<string> SaveAsync(IFormFile file, string folder);
    Task<string> SaveImageAsync(IFormFile file, string folder, int width = 400, int height = 400, int quality = 75);
    bool CheckSize(IFormFile file, int size, FileSizeTypes sizeType);
    bool CheckType(IFormFile file, string type);
}
