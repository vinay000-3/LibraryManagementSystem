using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs
{
    public class UpdateBookCategoryRequest
    {
        [Required(ErrorMessage = "Category Name is required.")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "Category Name should be between 2 and 50 characters.")]
        [RegularExpression(@"^[A-Za-z\s&-]+$",
            ErrorMessage = "Category Name can contain only letters, spaces, '&' and '-' characters.")]
        public string CategoryName { get; set; } = string.Empty;
    }
}