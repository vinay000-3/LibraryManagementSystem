using LibraryManagementSystem.Data;
using LibraryManagementSystem.DTOs.Damage;
using LibraryManagementSystem.Enums;
using LibraryManagementSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services
{
    public class DamageService : IDamageService
    {
        private readonly LibraryDbContext _context;

        public DamageService(LibraryDbContext context)
        {
            _context = context;
        }
public async Task<List<DamagedBookResponseDto>> GetPendingDamagedBooksAsync()
{
    return await _context.BookDamageRecords
        .Include(x => x.Book)
        .Include(x => x.User)
        .Where(x => x.DamageStatus == DamageStatus.PendingAdminDecision)
        .Select(x => new DamagedBookResponseDto
        {
            DamageRecordId = x.DamageRecordId,
            BorrowId = x.BorrowId,
            BookId = x.BookId,
            BookTitle = x.Book!.Title,
            UserId = x.UserId,
            UserName = x.User!.FullName,
            DamageLevel = x.DamageLevel,
            Recommendation = x.Recommendation,
            DamageDescription = x.DamageDescription,
            FineAmount = x.FineAmount,
            FineCollected = x.FineCollected,
            DamageStatus = x.DamageStatus,
            ReportedDate = x.ReportedDate
        })
        .ToListAsync();
}
    public async Task<string> SendForRepairAsync(string damageRecordId)
{
    var damage = await _context.BookDamageRecords
        .FirstOrDefaultAsync(x => x.DamageRecordId == damageRecordId);

    if (damage == null)
        throw new Exception("Damage record not found.");

    if (damage.DamageStatus != DamageStatus.PendingAdminDecision)
        throw new Exception("Invalid damage status.");

    damage.DamageStatus = DamageStatus.UnderRepair;

    await _context.SaveChangesAsync();

    return "Book sent for repair successfully.";

}

    public async Task<string> DisposeBookAsync(string damageRecordId)
{
    var damage = await _context.BookDamageRecords
        .Include(x => x.Book)
        .FirstOrDefaultAsync(x => x.DamageRecordId == damageRecordId);

    if (damage == null)
        throw new Exception("Damage record not found.");

    if (damage.Book == null)
        throw new Exception("Book not found.");

    damage.DamageStatus = DamageStatus.Disposed;
    damage.DisposedDate = DateTime.Now;

    damage.Book.TotalCopies--;
    damage.Book.DamagedCopies--;

    await _context.SaveChangesAsync();

    return "Book disposed successfully.";
}
public async Task<string> RepairCompletedAsync(string damageRecordId)
{
    var damage = await _context.BookDamageRecords
        .Include(x => x.Book)
        .FirstOrDefaultAsync(x => x.DamageRecordId == damageRecordId);

    if (damage == null)
        throw new Exception("Damage record not found.");

    if (damage.Book == null)
        throw new Exception("Book not found.");

    if (damage.DamageStatus != DamageStatus.UnderRepair)
        throw new Exception("Book is not under repair.");

    damage.DamageStatus = DamageStatus.Repaired;
    damage.RepairCompletedDate = DateTime.Now;

    damage.Book.DamagedCopies--;
    damage.Book.AvailableCopies++;

    await _context.SaveChangesAsync();

    return "Book marked as repaired and added back to inventory.";
}
    }
}