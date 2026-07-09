using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        [Key]
        public string BookId { get; set; } = string.Empty;

        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Publisher { get; set; } = string.Empty;

        [Required]
        public string CategoryId { get; set; } = string.Empty;

        [ForeignKey(nameof(CategoryId))]
        public BookCategory? Category { get; set; }

        [Required]
        public BookLanguage Language { get; set; }

        [Required]
        [StringLength(20)]
        public string Edition { get; set; } = string.Empty;

        [Range(1800, 2100)]
        public int PublishedYear { get; set; }

        [Required]
        [StringLength(30)]
        public string ShelfLocation { get; set; } = string.Empty;

        [Range(1, 100000)]
        public decimal Price { get; set; }

        [Range(1, 1000)]
        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }

        public int BorrowedCopies { get; set; }

        public int ReservedCopies { get; set; }

        public BookStatus BookStatus { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}