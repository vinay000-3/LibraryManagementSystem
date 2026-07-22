using LibraryManagementSystem.DTOs.Borrow;
using LibraryManagementSystem.DTOs.Return;
using LibraryManagementSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.DTOs.ReturnWorkflow;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;

        public BorrowController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowBook(BorrowBookRequestDto request)
        {
            try
            {
                var result = await _borrowService.BorrowBookAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

[Authorize(Roles = "Member")]
[HttpPost("return-book")]
public async Task<IActionResult> ReturnBook(ReturnBookRequestDto request)
{
    try
    {
        var result = await _borrowService.ReturnBookAsync(request);
        return Ok(result);
    }
    catch (Exception ex)
    {
        return BadRequest(new
        {
            Success = false,
            Message = ex.Message
        });
    }
}
       
       [Authorize(Roles = "Librarian")]
[HttpPost("librarian/review-return")]
public async Task<IActionResult> ReviewReturnByLibrarian(LibrarianReviewRequestDto request)
{
    try
    {
        var result = await _borrowService.ReviewReturnByLibrarianAsync(request);
        return Ok(result);
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}
    }
}