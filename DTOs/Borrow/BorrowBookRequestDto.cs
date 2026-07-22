using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs.Borrow
{
    public class BorrowBookRequestDto
    {

        [Required]
        public string BookId { get; set; } = string.Empty;
    }
}