using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs
{
    public class RejectUserRequest
    {
        [Required]
        [StringLength(300)]
        public string Reason { get; set; } = string.Empty;
    }
}