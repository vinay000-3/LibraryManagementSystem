using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{12}$")]
        public string Aadhaar { get; set; } = string.Empty;

        public string MembershipPlanId { get; set; } = string.Empty;

        public decimal PaidMembershipFee { get; set; }

        public string Role { get; set; } = "Member";

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public string MembershipStatus { get; set; } = "Inactive";

        public string ApprovalStatus { get; set; } = "Pending";
    }
}