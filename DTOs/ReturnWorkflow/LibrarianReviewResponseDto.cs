namespace LibraryManagementSystem.DTOs.ReturnWorkflow
{
    public class LibrarianReviewResponseDto
    {
        public string BorrowId { get; set; } = string.Empty;

        public int LateDays { get; set; }

        public decimal LateFine { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}