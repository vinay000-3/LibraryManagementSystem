public class ReturnVerificationRequestDto
{
    public string BorrowId { get; set; } = string.Empty;
    public bool IsBookDamaged { get; set; }
    public decimal? DamageFine { get; set; }
    public bool LateFinePaid { get; set; }
    public bool DamageFinePaid { get; set; }
}