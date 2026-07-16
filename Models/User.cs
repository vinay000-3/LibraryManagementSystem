using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(10, MinimumLength = 10)]
        [Phone]
        public string MobileNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string Address { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        // Membership Plan
        [Required]
        public string MembershipPlanId { get; set; } = string.Empty;

        [ForeignKey(nameof(MembershipPlanId))]
        public MembershipPlan? MembershipPlan { get; set; }

        [Required]
        public decimal PaidMembershipFee { get; set; }

        public DateTime MembershipStartDate { get; set; }

        public DateTime MembershipEndDate { get; set; }

        // Registration
        public RegistrationStatus RegistrationStatus { get; set; }

        public UserRole UserRole { get; set; }

        // Account Status
        public bool IsActive { get; set; } = true;

        // Audit Fields
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }
    }
}