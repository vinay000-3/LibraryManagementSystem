using LibraryManagementSystem.DTOs.UserResponseDto;
using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetUsersAsync(RegistrationStatus? status);

        Task<UserResponseDto> GetUserByIdAsync(string userId);

        Task<List<UserResponseDto>> SearchUsersAsync(string keyword);
    }
}