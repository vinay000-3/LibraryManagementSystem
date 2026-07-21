using LibraryManagementSystem.Data;
using LibraryManagementSystem.DTOs.Borrow;
using LibraryManagementSystem.DTOs.Return;
using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly LibraryDbContext _context;

        public BorrowService(LibraryDbContext context)
        {
            _context = context;
        }
public async Task<BorrowBookResponseDto> BorrowBookAsync(BorrowBookRequestDto request)
{
    // 1. Get User
    var user = await _context.Users
        .Include(x => x.MembershipPlan)
        .FirstOrDefaultAsync(x => x.UserId == request.UserId);

    if (user == null)
    throw new Exception("User not found.");

if (user.MembershipPlan == null)
    throw new Exception("Membership plan not found.");

if (user.RegistrationStatus != RegistrationStatus.Approved)
    throw new Exception("User is not approved.");

    if (user.MembershipEndDate < DateTime.Now)
        throw new Exception("Membership has expired.");

    // 2. Get Book
    var book = await _context.Books
        .FirstOrDefaultAsync(x => x.BookId == request.BookId);

    if (book == null)
        throw new Exception("Book not found.");

    if (book.AvailableCopies <= 0)
        throw new Exception("Book is not available.");
        if (book.BookStatus != BookStatus.Available)
    throw new Exception("Book is currently unavailable for borrowing.");

    // 3. Borrow Limit
    int borrowedBooks = await _context.BorrowBooks.CountAsync(x =>
        x.UserId == request.UserId &&
        x.BorrowStatus == BorrowStatus.Borrowed);

    if (borrowedBooks >= user.MembershipPlan.MaximumBooksAllowed)
        throw new Exception("Borrow limit reached.");

    // 4. Already Borrowed
    bool alreadyBorrowed = await _context.BorrowBooks.AnyAsync(x =>
        x.UserId == request.UserId &&
        x.BookId == request.BookId &&
        x.BorrowStatus == BorrowStatus.Borrowed);

    if (alreadyBorrowed)
        throw new Exception("Book already borrowed.");

    // 5. Due Date
    DateTime dueDate = DateTime.Now.AddDays(user.MembershipPlan.BorrowDurationDays);

    
   // 6. Generate Borrow Id
string borrowId = "BR0001";

var lastBorrow = await _context.BorrowBooks
    .OrderByDescending(x => x.BorrowId)
    .FirstOrDefaultAsync();

if (lastBorrow != null)
{
    int number = int.Parse(lastBorrow.BorrowId.Substring(2));
    borrowId = "BR" + (number + 1).ToString("D4");
}

    BorrowBook borrow = new BorrowBook
    {
        BorrowId = borrowId,
        UserId = request.UserId,
        BookId = request.BookId,
        BorrowDate = DateTime.Now,
        DueDate = dueDate,
        BorrowStatus = BorrowStatus.Borrowed,
        ReturnStatus = ReturnStatus.None
    };

    // 7. Update Inventory
    book.AvailableCopies--;
    book.BorrowedCopies++;

    _context.BorrowBooks.Add(borrow);

    await _context.SaveChangesAsync();

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
    // 1. Get Borrow Record
    var borrow = await _context.BorrowBooks
        .Include(x => x.User)
        .Include(x => x.Book)
        .FirstOrDefaultAsync(x =>
            x.BorrowId == request.BorrowId &&
            x.BorrowStatus == BorrowStatus.Borrowed);

    if (borrow == null)
        throw new Exception("Borrow record not found or book already returned.");

        if (borrow.User == null || borrow.Book == null)
        throw new Exception("Related user or book not found.");

      // 2. Update Return Details
    borrow.ReturnDate = DateTime.Now;
    borrow.BorrowStatus = BorrowStatus.Returned;
    borrow.ReturnStatus = ReturnStatus.Completed;

    // 3. Update Book Inventory
    borrow.Book.AvailableCopies++;
    borrow.Book.BorrowedCopies--;

    // 4. Calculate Late Fine
bool isLateReturn = false;
int lateDays = 0;
decimal fineAmount = 0;

if (borrow.ReturnDate.Value.Date > borrow.DueDate.Date)
{
    isLateReturn = true;

    lateDays = (borrow.ReturnDate.Value.Date - borrow.DueDate.Date).Days;

    fineAmount = lateDays * 10;
}

// 5. Save Changes
await _context.SaveChangesAsync();

// 6. Return Response
return new ReturnBookResponseDto
{
    BorrowId = borrow.BorrowId,
    UserName = borrow.User.FullName,
    BookTitle = borrow.Book.Title,
    ReturnDate = borrow.ReturnDate.Value,
    IsLateReturn = isLateReturn,
    LateDays = lateDays,
    FineAmount = fineAmount,
    Message = "Book returned successfully."
};
}
    }
}