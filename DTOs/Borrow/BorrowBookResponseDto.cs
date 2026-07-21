namespace LibraryManagementSystem.DTOs.Borrow
{
    public class BorrowBookResponseDto
    {
        public string BorrowId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string BookTitle { get; set; } = string.Empty;

        public DateTime BorrowDate { get; set; }

        public DateTime DueDate { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}