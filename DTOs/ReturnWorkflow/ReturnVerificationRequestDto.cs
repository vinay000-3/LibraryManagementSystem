using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.DTOs.ReturnWorkflow
{
    public class ReturnVerificationRequestDto
    {
        public string BorrowId { get; set; } = string.Empty;

        public BookCondition BookCondition { get; set; }

        public bool DamageFinePaid { get; set; }
    }
}