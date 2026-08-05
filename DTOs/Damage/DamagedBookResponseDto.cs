using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.DTOs.Damage
{
    public class DamagedBookResponseDto
    {
        public string DamageRecordId { get; set; } = string.Empty;

        public string BorrowId { get; set; } = string.Empty;

        public string BookId { get; set; } = string.Empty;

        public string BookTitle { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public DamageLevel DamageLevel { get; set; }

        public DamageRecommendation Recommendation { get; set; }

        public string DamageDescription { get; set; } = string.Empty;

        public decimal FineAmount { get; set; }

        public bool FineCollected { get; set; }

        public DamageStatus DamageStatus { get; set; }

        public DateTime ReportedDate { get; set; }
    }
}