namespace LibraryManagementSystem.DTOs.UserResponseDto
{
    public class UserResponseDto
    {
        public string UserId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public string MembershipPlan { get; set; } = string.Empty;

        public decimal MembershipFee { get; set; }

        public DateTime MembershipStartDate { get; set; }

        public DateTime MembershipEndDate { get; set; }

        public string RegistrationStatus { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}

