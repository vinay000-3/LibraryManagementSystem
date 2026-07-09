using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookCategoryController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BookCategoryController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Book Category Controller is Working");
        }
        [HttpPost("create")]
public async Task<IActionResult> Create(CreateBookCategoryRequest request)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    // Remove leading/trailing spaces
    request.CategoryName = request.CategoryName.Trim();

    // Check duplicate category name
    var existingCategory = await _context.BookCategories
        .FirstOrDefaultAsync(x => x.CategoryName.ToLower() == request.CategoryName.ToLower());

    if (existingCategory != null)
    {
        return BadRequest("Category already exists.");
    }

    // Generate Category Id
    string categoryId = "BC0001";

    var lastCategory = await _context.BookCategories
        .OrderByDescending(x => x.CategoryId)
        .FirstOrDefaultAsync();

    if (lastCategory != null)
    {
        int number = int.Parse(lastCategory.CategoryId.Substring(2));
        categoryId = "BC" + (number + 1).ToString("D4");
    }

    // Create object
    var category = new BookCategory
    {
        CategoryId = categoryId,
        CategoryName = request.CategoryName,
        CreatedDate = DateTime.Now
    };

    _context.BookCategories.Add(category);

    await _context.SaveChangesAsync();

    return Ok(new
    {
        Success = true,
        Message = "Book Category Created Successfully.",
        CategoryId = categoryId
    });
}
[HttpGet]
public async Task<IActionResult> GetAllCategories()
{
    var categories = await _context.BookCategories
        .OrderBy(x => x.CategoryName)
        .ToListAsync();

    return Ok(categories);
}
[HttpGet("{categoryName}")]
public async Task<IActionResult> GetCategoryByName(string categoryName)
{
    // Remove extra spaces
    categoryName = categoryName.Trim();

    var category = await _context.BookCategories
        .FirstOrDefaultAsync(x =>
            x.CategoryName.ToLower() == categoryName.ToLower());

    if (category == null)
    {
        return NotFound("Category not found.");
    }

    return Ok(category);
}
[HttpPut("{id}")]
public async Task<IActionResult> UpdateCategory(string id, UpdateBookCategoryRequest request)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    request.CategoryName = request.CategoryName.Trim();

    // Find category
    var category = await _context.BookCategories
        .FirstOrDefaultAsync(x => x.CategoryId == id);

    if (category == null)
    {
        return NotFound("Category not found.");
    }

    // Check duplicate name (ignore current category)
    var existingCategory = await _context.BookCategories
        .FirstOrDefaultAsync(x =>
            x.CategoryName.ToLower() == request.CategoryName.ToLower()
            && x.CategoryId != id);

    if (existingCategory != null)
    {
        return BadRequest("Category already exists.");
    }

    category.CategoryName = request.CategoryName;

    await _context.SaveChangesAsync();

    return Ok(new
    {
        Success = true,
        Message = "Category Updated Successfully."
    });
}
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteCategory(string id)
{
    var category = await _context.BookCategories
        .FirstOrDefaultAsync(x => x.CategoryId == id);

    if (category == null)
    {
        return NotFound("Category not found.");
    }

    _context.BookCategories.Remove(category);

    await _context.SaveChangesAsync();

    return Ok(new
    {
        Success = true,
        Message = "Category Deleted Successfully."
    });
}
    }
}