namespace LibraryManagementSystem.DTOs.ReturnWorkflow
{
    public class LibrarianReviewRequestDto
    {
        public string BorrowId { get; set; } = string.Empty;

        public bool LateFinePaid { get; set; }
    }
}