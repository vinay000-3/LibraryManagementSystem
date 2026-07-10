using LibraryManagementSystem.Data;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Enums;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BookController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Book Controller is Working");
        }
        [HttpPost("create")]
public async Task<IActionResult> Create(CreateBookRequest request)
{
    // Validate Request
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    
    request.Title = request.Title.Trim();
    request.Author = request.Author.Trim();
    request.Publisher = request.Publisher.Trim();
    request.ShelfLocation = request.ShelfLocation.Trim();

    if (!string.IsNullOrWhiteSpace(request.Description))
    {
        request.Description = request.Description.Trim();
    }

    
    var category = await _context.BookCategories
        .FirstOrDefaultAsync(x => x.CategoryId == request.CategoryId);

    if (category == null)
    {
        return BadRequest("Selected Book Category does not exist.");
    }

    
    var existingBook = await _context.Books
        .FirstOrDefaultAsync(x =>
            x.Title.ToLower() == request.Title.ToLower()
            && x.Author.ToLower() == request.Author.ToLower());

    if (existingBook != null)
    {
        return BadRequest("Book already exists.");
    }

    
    string bookId = "BK0001";

    var lastBook = await _context.Books
        .OrderByDescending(x => x.BookId)
        .FirstOrDefaultAsync();

    if (lastBook != null)
    {
        int number = int.Parse(lastBook.BookId.Substring(2));
        bookId = "BK" + (number + 1).ToString("D4");
    }

    
    var book = new Book
    {
        BookId = bookId,
        Title = request.Title,
        Author = request.Author,
        Publisher = request.Publisher,
        CategoryId = request.CategoryId,
        Language = request.Language,
        Edition = request.Edition,
        PublishedYear = request.PublishedYear,
        ShelfLocation = request.ShelfLocation,
        Price = request.Price,
        TotalCopies = request.TotalCopies,

        AvailableCopies = request.TotalCopies,
        BorrowedCopies = 0,
        ReservedCopies = 0,
        BookStatus = BookStatus.Available,

        Description = request.Description,
        CreatedDate = DateTime.Now
    };

    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    return Ok(new
    {
        Success = true,
        Message = "Book Added Successfully.",
        BookId = bookId
    });
}
    }
}