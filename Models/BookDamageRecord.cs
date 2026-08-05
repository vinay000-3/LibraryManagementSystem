using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.Models
{
    public class BookDamageRecord
    {
        [Key]
        public string DamageRecordId { get; set; } = string.Empty;

        [Required]
        public string BorrowId { get; set; } = string.Empty;

        [ForeignKey(nameof(BorrowId))]
        public BorrowBook? BorrowBook { get; set; }

        [Required]
        public string BookId { get; set; } = string.Empty;

        [ForeignKey(nameof(BookId))]
        public Book? Book { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [Required]
        public string ReturnVerificationOfficerId { get; set; } = string.Empty;

        [ForeignKey(nameof(ReturnVerificationOfficerId))]
        public LibraryEmployee? ReturnVerificationOfficer { get; set; }

        [Required]
        public DamageLevel DamageLevel { get; set; }

        [Required]
        public DamageRecommendation Recommendation { get; set; }

        [Required]
        [StringLength(500)]
        public string DamageDescription { get; set; } = string.Empty;

        public decimal FineAmount { get; set; }

        public bool FineCollected { get; set; }

        public DamageStatus DamageStatus { get; set; }

        [StringLength(500)]
        public string? AdminRemarks { get; set; }

        public DateTime ReportedDate { get; set; } = DateTime.Now;

        public DateTime? RepairCompletedDate { get; set; }

        public DateTime? DisposedDate { get; set; }
    }
}