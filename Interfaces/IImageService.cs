using Microsoft.AspNetCore.Http;

namespace LibraryManagementSystem.Interfaces
{
   public interface IImageService
{
    Task<string?> SaveImageAsync(IFormFile? image, string folderName);

    void DeleteImage(string? imagePath);
}
}