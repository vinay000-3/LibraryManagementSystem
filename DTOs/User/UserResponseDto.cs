namespace LibraryManagementSystem.DTOs.UserResponseDto
{
    public class UserResponseDto
    {
        public string UserId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public string MembershipPlan { get; set; }

        public decimal MembershipFee { get; set; }

        public DateTime MembershipStartDate { get; set; }

        public DateTime MembershipEndDate { get; set; }

        public string RegistrationStatus { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}

