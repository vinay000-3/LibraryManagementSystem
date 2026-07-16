using System.ComponentModel.DataAnnotations;
using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.DTOs
{
    public class AdminRegisterUserRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        [RegularExpression(@"^[A-Za-z ]+$")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[0-9]{10}$")]
        public string MobileNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string Address { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public string MembershipPlanId { get; set; } = string.Empty;

        [Required]
        public decimal PaidMembershipFee { get; set; }

        [Required]
        public UserRole UserRole { get; set; }
    }
}