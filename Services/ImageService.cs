using LibraryManagementSystem.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace LibraryManagementSystem.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string?> SaveImageAsync(IFormFile? image, string folderName)
        {
            if (image == null || image.Length == 0)
            {
                return null;
            }

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };

string extension = Path.GetExtension(image.FileName).ToLower();

if (!allowedExtensions.Contains(extension))
{
    throw new Exception("Only JPG, JPEG and PNG images are allowed.");
}
            const long maxFileSize = 2 * 1024 * 1024; // 2 MB

if (image.Length > maxFileSize)
{
    throw new Exception("Image size should not exceed 2 MB.");
}
            string uploadPath = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                folderName);

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string fileName = Guid.NewGuid().ToString() +
                              Path.GetExtension(image.FileName);

            string filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/uploads/{folderName}/{fileName}";
        }
        public void DeleteImage(string? imagePath)
{
    if (string.IsNullOrWhiteSpace(imagePath))
    {
        return;
    }

    string fullPath = Path.Combine(
        _environment.WebRootPath,
        imagePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

    if (File.Exists(fullPath))
    {
        File.Delete(fullPath);
    }
}
    }
    
}