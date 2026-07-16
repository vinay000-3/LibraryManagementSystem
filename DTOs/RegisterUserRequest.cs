using System.ComponentModel.DataAnnotations;
using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.DTOs
{
    public class RegisterUserRequest
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 100 characters.")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Full Name should contain only alphabets and spaces.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile Number is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile Number must contain exactly 10 digits.")]
        public string MobileNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(300, MinimumLength = 10)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Membership Plan is required.")]
        public string MembershipPlanId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Membership Fee is required.")]
        [Range(1, 100000, ErrorMessage = "Membership Fee must be greater than zero.")]
        public decimal PaidMembershipFee { get; set; }
    }
}