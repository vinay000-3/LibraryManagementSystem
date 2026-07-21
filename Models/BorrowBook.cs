using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.Models
{
    public class BorrowBook
    {
        [Key]
        public string BorrowId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [Required]
        public string BookId { get; set; } = string.Empty;

        [ForeignKey(nameof(BookId))]
        public Book? Book { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Required]
        public BorrowStatus BorrowStatus { get; set; }

        [Required]
        public ReturnStatus ReturnStatus { get; set; } = ReturnStatus.None;

        public BookCondition? BookCondition { get; set; }

        public int NumberOfRenewals { get; set; }

        public bool IsRenewed { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}