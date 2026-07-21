using LibraryManagementSystem.DTOs.Borrow;

using LibraryManagementSystem.DTOs.Return;

namespace LibraryManagementSystem.Interfaces
{
    public interface IBorrowService
    {
        Task<BorrowBookResponseDto> BorrowBookAsync(BorrowBookRequestDto request);
        Task<ReturnBookResponseDto> ReturnBookAsync(ReturnBookRequestDto request);
    }
}