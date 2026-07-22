using LibraryManagementSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class LibraryEmployee
    {
        [Key]
        public string EmployeeId { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public StaffRole Role { get; set; }

        [Required]
        public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}