using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs
{
    public class CreateMembershipPlanRequest
    {
        [Required]
        public string MembershipName { get; set; } = string.Empty;

        [Required]
[Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0.")]
public int DurationMonths { get; set; }

        [Required]
        public int MaximumBooksAllowed { get; set; }

        [Required]
        public int MaximumReservationsAllowed { get; set; }

        [Required]
        public decimal MaintenanceCost { get; set; }

        [Required]
        public decimal StaffSalary { get; set; }

        [Required]
        public decimal ElectricityCost { get; set; }

        public decimal MiscellaneousExpenses { get; set; }

        [Required]
        public int NumberOfBooks { get; set; }

        [Required]
        public int NumberOfBookCopies { get; set; }

        [Required]
[Range(1, int.MaxValue, ErrorMessage = "Expected Members must be at least 1.")]
public int ExpectedMembers { get; set; }

      [Required]
public decimal ProfitPercentage { get; set; }

[Required]
public decimal GSTPercentage { get; set; }

[Required]
[Range(0, 100, ErrorMessage = "Membership Discount Percentage must be between 0 and 100.")]
public decimal MembershipDiscountPercentage { get; set; }
    }
}