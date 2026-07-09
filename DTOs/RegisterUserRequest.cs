using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs
{
    public class RegisterUserRequest
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Aadhaar must be 12 digits")]
        public string Aadhaar { get; set; } = string.Empty;

        [Required]
        public string MembershipPlanId { get; set; } = string.Empty;
    }
}