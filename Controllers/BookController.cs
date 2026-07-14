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
[HttpGet]
public async Task<IActionResult> GetAllBooks()
{
    var books = await _context.Books
        .Include(b => b.Category)
        .OrderBy(b => b.Title)
        .ToListAsync();

    return Ok(books);
}
[HttpGet("{id}")]
public async Task<IActionResult> GetBookById(string id)
{
    var book = await _context.Books
        .Include(b => b.Category)
        .FirstOrDefaultAsync(b => b.BookId == id);

    if (book == null)
    {
        return NotFound("Book not found.");
    }

    return Ok(book);
}
[HttpPut("{id}")]
public async Task<IActionResult> UpdateBook(string id, UpdateBookRequest request)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    // Find Book
    var book = await _context.Books
        .FirstOrDefaultAsync(b => b.BookId == id);

    if (book == null)
    {
        return NotFound("Book not found.");
    }

    // Check Category
    var category = await _context.BookCategories
        .FirstOrDefaultAsync(c => c.CategoryId == request.CategoryId);

    if (category == null)
    {
        return BadRequest("Selected Category does not exist.");
    }

    // Trim Strings
    request.Title = request.Title.Trim();
    request.Author = request.Author.Trim();
    request.Publisher = request.Publisher.Trim();
    request.ShelfLocation = request.ShelfLocation.Trim();

    if (!string.IsNullOrWhiteSpace(request.Description))
    {
        request.Description = request.Description.Trim();
    }

    // Check Duplicate Book
    var existingBook = await _context.Books
        .FirstOrDefaultAsync(b =>
            b.BookId != id &&
            b.Title.ToLower() == request.Title.ToLower() &&
            b.Author.ToLower() == request.Author.ToLower());

    if (existingBook != null)
    {
        return BadRequest("Another book with the same Title and Author already exists.");
    }

    // Calculate copy difference
    int difference = request.TotalCopies - book.TotalCopies;

    book.Title = request.Title;
    book.Author = request.Author;
    book.Publisher = request.Publisher;
    book.CategoryId = request.CategoryId;
    book.Language = request.Language;
    book.Edition = request.Edition;
    book.PublishedYear = request.PublishedYear;
    book.ShelfLocation = request.ShelfLocation;
    book.Price = request.Price;
    book.Description = request.Description;

    // Update inventory
    book.TotalCopies = request.TotalCopies;
    book.AvailableCopies += difference;

    if (book.AvailableCopies < 0)
    {
        return BadRequest("Total copies cannot be less than borrowed/reserved copies.");
    }

    book.BookStatus = book.AvailableCopies > 0
        ? BookStatus.Available
        : BookStatus.OutOfStock;

    await _context.SaveChangesAsync();

    return Ok(new
    {
        Success = true,
        Message = "Book Updated Successfully."
    });
}
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteBook(string id)
{
    var book = await _context.Books
        .FirstOrDefaultAsync(b => b.BookId == id);

    if (book == null)
    {
        return NotFound("Book not found.");
    }

    // Prevent deletion if copies are borrowed or reserved
    if (book.BorrowedCopies > 0 || book.ReservedCopies > 0)
    {
        return BadRequest("Book cannot be deleted because copies are currently borrowed or reserved.");
    }

    _context.Books.Remove(book);

    await _context.SaveChangesAsync();

    return Ok(new
    {
        Success = true,
        Message = "Book Deleted Successfully."
    });
}
[HttpGet("search")]
public async Task<IActionResult> SearchBooks(string keyword)
{
    // Check empty keyword
    if (string.IsNullOrWhiteSpace(keyword))
    {
        return BadRequest("Search keyword is required.");
    }

    keyword = keyword.Trim().ToLower();

    var books = await _context.Books
        .Include(b => b.Category)
        .Where(b =>
            b.Title.ToLower().Contains(keyword) ||
            b.Author.ToLower().Contains(keyword) ||
            b.Publisher.ToLower().Contains(keyword))
        .OrderBy(b => b.Title)
        .ToListAsync();

    if (!books.Any())
    {
        return NotFound("No books found.");
    }

    return Ok(books);
}
[HttpGet("available")]
public async Task<IActionResult> GetAvailableBooks()
{
    var books = await _context.Books
        .Include(b => b.Category)
        .Where(b => b.AvailableCopies > 0)
        .OrderBy(b => b.Title)
        .ToListAsync();

    if (!books.Any())
    {
        return NotFound("No available books found.");
    }

    return Ok(books);
}
[HttpGet("outofstock")]
public async Task<IActionResult> GetOutOfStockBooks()
{
    var books = await _context.Books
        .Include(b => b.Category)
        .Where(b => b.AvailableCopies == 0)
        .OrderBy(b => b.Title)
        .ToListAsync();

    if (!books.Any())
    {
        return NotFound("No out of stock books found.");
    }

    return Ok(books);
}

    }
}