using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class BookCategory
    {
        [Key]
        public string CategoryId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category Name is required.")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "Category Name should be between 2 and 50 characters.")]
        public string CategoryName { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}