using System.ComponentModel.DataAnnotations;
using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.DTOs.ReturnWorkflow
{

public class ReturnVerificationRequestDto
{
    public string BorrowId { get; set; } = string.Empty;
    public bool IsBookDamaged { get; set; }
    public decimal? DamageFine { get; set; }
    public bool LateFinePaid { get; set; }
    public bool DamageFinePaid { get; set; }

    public DamageLevel DamageLevel { get; set; }

public DamageRecommendation Recommendation { get; set; }

[StringLength(500)]
public string? DamageDescription { get; set; }
}}