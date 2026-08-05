using LibraryManagementSystem.DTOs.Damage;

namespace LibraryManagementSystem.Interfaces
{
    public interface IDamageService
    {
        Task<List<DamagedBookResponseDto>> GetPendingDamagedBooksAsync();

        Task<string> SendForRepairAsync(string damageRecordId);

        Task<string> DisposeBookAsync(string damageRecordId);

        Task<string> RepairCompletedAsync(string damageRecordId);
    }
}