using LibraryManagementSystem.DTOs.Borrow;

using LibraryManagementSystem.DTOs.Return;

using LibraryManagementSystem.DTOs.ReturnWorkflow;

namespace LibraryManagementSystem.Interfaces


{
    public interface IBorrowService
    {
        Task<BorrowBookResponseDto> BorrowBookAsync(BorrowBookRequestDto request);
        Task<ReturnBookResponseDto> ReturnBookAsync(ReturnBookRequestDto request);

        Task<LibrarianReviewResponseDto> ReviewReturnByLibrarianAsync(
    LibrarianReviewRequestDto request);

Task<ReturnVerificationResponseDto> VerifyReturnAsync(
    ReturnVerificationRequestDto request);
    }
}