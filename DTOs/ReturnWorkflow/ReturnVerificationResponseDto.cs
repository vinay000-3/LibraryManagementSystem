namespace LibraryManagementSystem.DTOs.ReturnWorkflow
{
    public class ReturnVerificationResponseDto
    {
        public string BorrowId { get; set; } = string.Empty;

        public decimal DamageFine { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}