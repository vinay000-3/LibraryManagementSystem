namespace LibraryManagementSystem.DTOs.Employee
{
    public class EmployeeResponseDto
    {
        public string EmployeeId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}