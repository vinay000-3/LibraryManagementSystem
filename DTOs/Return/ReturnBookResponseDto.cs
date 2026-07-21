public class ReturnBookResponseDto
{
    public string BorrowId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string BookTitle { get; set; } = string.Empty;
    public DateTime ReturnDate { get; set; }
    public bool IsLateReturn { get; set; }
    public int LateDays { get; set; }
    public decimal FineAmount { get; set; }
    public string Message { get; set; } = string.Empty;
}