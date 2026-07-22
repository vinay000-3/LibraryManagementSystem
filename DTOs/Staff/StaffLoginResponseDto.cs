namespace LibraryManagementSystem.DTOs.Staff
{
    public class StaffLoginResponseDto
    {
        public string EmployeeId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}