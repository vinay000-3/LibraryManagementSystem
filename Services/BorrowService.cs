using LibraryManagementSystem.Data;
using LibraryManagementSystem.DTOs.Borrow;
using LibraryManagementSystem.DTOs.Return;
using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Enums;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.DTOs.ReturnWorkflow;
using Microsoft.AspNetCore.Http;

namespace LibraryManagementSystem.Services
{
public class BorrowService : IBorrowService
{
    private readonly LibraryDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BorrowService(
        LibraryDbContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
public async Task<BorrowBookResponseDto> BorrowBookAsync(BorrowBookRequestDto request)
{
    // 1. Get Logged-in User Id from JWT
    var userId = _httpContextAccessor.HttpContext?.User
        .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    if (string.IsNullOrEmpty(userId))
        throw new Exception("User is not authenticated.");

    // 2. Get User
    var user = await _context.Users
        .Include(x => x.MembershipPlan)
        .FirstOrDefaultAsync(x => x.UserId == userId);

    if (user == null)
        throw new Exception("User not found.");

    if (user.MembershipPlan == null)
        throw new Exception("Membership plan not found.");

    if (user.RegistrationStatus != RegistrationStatus.Approved)
        throw new Exception("User is not approved.");

    if (user.MembershipEndDate < DateTime.Now)
        throw new Exception("Membership has expired.");

    // 3. Get Book
    var book = await _context.Books
        .FirstOrDefaultAsync(x => x.BookId == request.BookId);

    if (book == null)
        throw new Exception("Book not found.");

    if (book.AvailableCopies <= 0)
        throw new Exception("Book is not available.");

    if (book.BookStatus != BookStatus.Available)
        throw new Exception("Book is currently unavailable for borrowing.");

    // 4. Borrow Limit
    int borrowedBooks = await _context.BorrowBooks.CountAsync(x =>
        x.UserId == userId &&
        x.BorrowStatus == BorrowStatus.Borrowed);

    if (borrowedBooks >= user.MembershipPlan.MaximumBooksAllowed)
        throw new Exception("Borrow limit reached.");

    // 5. Already Borrowed
    bool alreadyBorrowed = await _context.BorrowBooks.AnyAsync(x =>
        x.UserId == userId &&
        x.BookId == request.BookId &&
        x.BorrowStatus == BorrowStatus.Borrowed);

    if (alreadyBorrowed)
        throw new Exception("Book already borrowed.");

    // 6. Due Date
    DateTime dueDate = DateTime.Now.AddDays(user.MembershipPlan.BorrowDurationDays);

    // 7. Generate Borrow Id
    string borrowId = "BR0001";

    var lastBorrow = await _context.BorrowBooks
        .OrderByDescending(x => x.BorrowId)
        .FirstOrDefaultAsync();

    if (lastBorrow != null)
    {
        int number = int.Parse(lastBorrow.BorrowId.Substring(2));
        borrowId = "BR" + (number + 1).ToString("D4");
    }

    // 8. Create Borrow Record
    BorrowBook borrow = new BorrowBook
    {
        BorrowId = borrowId,
        UserId = userId,
        BookId = request.BookId,
        BorrowDate = DateTime.Now,
        DueDate = dueDate,
        BorrowStatus = BorrowStatus.Borrowed,
        ReturnStatus = ReturnStatus.None
    };

    // 9. Update Inventory
    book.AvailableCopies--;
    book.BorrowedCopies++;

    _context.BorrowBooks.Add(borrow);

    await _context.SaveChangesAsync();

    // 10. Return Response
       return new BorrowBookResponseDto
    {
        BorrowId = borrowId,
        UserName = user.FullName,
        BookTitle = book.Title,
        BorrowDate = borrow.BorrowDate,
        DueDate = dueDate,
        Message = "Book borrowed successfully."
    };
}

public async Task<ReturnBookResponseDto> ReturnBookAsync(ReturnBookRequestDto request)
{
    var userId = _httpContextAccessor.HttpContext?.User
        .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    if (string.IsNullOrEmpty(userId))
        throw new Exception("User is not authenticated.");

    var borrow = await _context.BorrowBooks
        .Include(x => x.User)
        .Include(x => x.Book)
        .FirstOrDefaultAsync(x =>
            x.BorrowId == request.BorrowId &&
            x.BorrowStatus == BorrowStatus.Borrowed);

    if (borrow == null)
        throw new Exception("Borrow record not found.");

    if (borrow.User == null || borrow.Book == null)
        throw new Exception("Related user or book not found.");

    if (borrow.UserId != userId)
        throw new Exception("You can return only your own borrowed books.");

    if (borrow.ReturnStatus == ReturnStatus.Completed)
        throw new Exception("Book has already been returned.");

    if (borrow.ReturnStatus != ReturnStatus.None)
        throw new Exception("Return request has already been submitted.");

    // Submit return request
    borrow.ReturnStatus = ReturnStatus.PendingLibrarian;

    await _context.SaveChangesAsync();

    return new ReturnBookResponseDto
    {
        BorrowId = borrow.BorrowId,
        UserName = borrow.User.FullName,
        BookTitle = borrow.Book.Title,
        ReturnDate = DateTime.Now,
        IsLateReturn = false,
        LateDays = 0,
        FineAmount = 0,
        Message = "Return request submitted successfully."
    };
}

public async Task<LibrarianReviewResponseDto> ReviewReturnByLibrarianAsync(
    LibrarianReviewRequestDto request)
{
    var employeeId = _httpContextAccessor.HttpContext?.User
        .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    if (string.IsNullOrEmpty(employeeId))
        throw new Exception("Employee is not authenticated.");

    var borrow = await _context.BorrowBooks
        .Include(x => x.Book)
        .FirstOrDefaultAsync(x =>
            x.BorrowId == request.BorrowId &&
            x.ReturnStatus == ReturnStatus.PendingLibrarian);

    if (borrow == null)
        throw new Exception("This return request has already been reviewed by the librarian.");

    if (borrow.Book == null)
        throw new Exception("Book not found.");

    int lateDays = 0;
    decimal lateFine = 0;

    if (DateTime.Now.Date > borrow.DueDate.Date)
    {
        lateDays = (DateTime.Now.Date - borrow.DueDate.Date).Days;
        lateFine = lateDays * 10;
    }

    borrow.LateFine = lateFine;
    borrow.LateFinePaid = request.LateFinePaid;
    borrow.LibrarianId = employeeId;
    borrow.ReturnStatus = ReturnStatus.PendingReturnVerificationOfficer;

    await _context.SaveChangesAsync();

    return new LibrarianReviewResponseDto
    {
        BorrowId = borrow.BorrowId,
        LateDays = lateDays,
        LateFine = lateFine,
        Message = "Return forwarded to Return Verification Officer."
    };
}

public async Task<ReturnVerificationResponseDto> VerifyReturnAsync(
    ReturnVerificationRequestDto request)
{
    var employeeId = _httpContextAccessor.HttpContext?.User
        .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    if (string.IsNullOrEmpty(employeeId))
        throw new Exception("Employee is not authenticated.");

    var borrow = await _context.BorrowBooks
        .Include(x => x.Book)
        .FirstOrDefaultAsync(x =>
            x.BorrowId == request.BorrowId &&
            x.ReturnStatus == ReturnStatus.PendingReturnVerificationOfficer);

    if (borrow == null)
        throw new Exception("Return request not found.");

    if (borrow.Book == null)
        throw new Exception("Book not found.");

    // Damage Fine
    if (request.IsBookDamaged)
    {
        if (!request.DamageFine.HasValue || request.DamageFine.Value <= 0)
            throw new Exception("Please enter a valid damage fine.");

        borrow.DamageFine = request.DamageFine.Value;
        borrow.DamageFinePaid = request.DamageFinePaid;
    }
    else
    {
        borrow.DamageFine = 0;
        borrow.DamageFinePaid = false;
    }

    // Update Return Details
    borrow.LateFinePaid = request.LateFinePaid;
    borrow.ReturnVerificationOfficerId = employeeId;
    borrow.ReturnDate = DateTime.Now;
    borrow.BorrowStatus = BorrowStatus.Returned;
    borrow.ReturnStatus = ReturnStatus.Completed;

    // Update Inventory
    borrow.Book.AvailableCopies++;
    borrow.Book.BorrowedCopies--;

    await _context.SaveChangesAsync();

    return new ReturnVerificationResponseDto
    {
        BorrowId = borrow.BorrowId,
        DamageFine = borrow.DamageFine,
        Message = "Book returned successfully."
    };
}}}