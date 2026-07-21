using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs.Borrow
{
    public class BorrowBookRequestDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string BookId { get; set; } = string.Empty;
    }
}