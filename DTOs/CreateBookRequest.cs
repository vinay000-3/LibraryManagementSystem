using System.ComponentModel.DataAnnotations;
using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.DTOs
{
    public class CreateBookRequest
    {
        [Required(ErrorMessage = "Book Title is required.")]
        [StringLength(150, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author Name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Publisher Name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Publisher { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required.")]
        public string CategoryId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Language is required.")]
        public BookLanguage Language { get; set; }

        [Required(ErrorMessage = "Edition is required.")]
        [StringLength(20)]
        public string Edition { get; set; } = string.Empty;

        [Range(1800, 2100, ErrorMessage = "Enter a valid Published Year.")]
        public int PublishedYear { get; set; }

        [Required(ErrorMessage = "Shelf Location is required.")]
        [StringLength(30)]
        public string ShelfLocation { get; set; } = string.Empty;

        [Range(1, 100000, ErrorMessage = "Price should be greater than zero.")]
        public decimal Price { get; set; }

        [Range(1, 1000, ErrorMessage = "Total Copies should be at least 1.")]
        public int TotalCopies { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}