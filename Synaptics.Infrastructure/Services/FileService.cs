using Microsoft.AspNetCore.Http;
using Synaptics.Application.Exceptions;
using Synaptics.Domain.Enums;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using Synaptics.Application.Interfaces.Services;

namespace Synaptics.Infrastructure.Services;

public class FileService : IFileService
{
    public async Task<string> SaveAsync(IFormFile file, string folder)
    {
        string uploadPath = Path.Combine(Path.GetFullPath("wwwroot"), "uploads", folder);

        if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

        string filename = Guid.NewGuid().ToString() + file.FileName;

        using (FileStream item = new(Path.Combine(uploadPath, filename), FileMode.Create))
        {
            await file.CopyToAsync(item);
        }

        return filename;
    }

    public async Task<string> SaveImageAsync(IFormFile file, string folder, int width = 400, int height = 400, int quality = 75)
    {
        if (!CheckType(file, "image")) throw new WrongFileTypeException("File must be image");

        string uploadPath = Path.Combine(Path.GetFullPath("wwwroot"), "uploads", folder);

        if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

        string extension = Path.GetExtension(file.FileName).ToLower();
        string filename = Guid.NewGuid().ToString() + extension;
        string filePath = Path.Combine(uploadPath, filename);

        using var image = await Image.LoadAsync(file.OpenReadStream());
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = new Size(width, height)
        }));

        if (extension == ".png") await image.SaveAsync(filePath, new PngEncoder());
        else if (extension == ".webp") await image.SaveAsync(filePath, new WebpEncoder { Quality = quality });
        else await image.SaveAsync(filePath, new JpegEncoder { Quality = quality });

        return filename;
    }

    public bool CheckSize(IFormFile file, int size, FileSizeTypes sizeType) => file.Length < size * ((int)sizeType);

    public bool CheckType(IFormFile file, string type) => file.ContentType.Contains(type);
}
