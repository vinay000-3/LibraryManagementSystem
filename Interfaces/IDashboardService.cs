using LibraryManagementSystem.DTOs.Dashboard;

namespace LibraryManagementSystem.Interfaces
{
    public interface IDashboardService
    {
        Task<AdminDashboardResponseDto> GetAdminDashboardAsync();

        Task<UserDashboardResponseDto> GetUserDashboardAsync();
    }
}