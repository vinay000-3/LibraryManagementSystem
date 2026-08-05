using LibraryManagementSystem.Data;
using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.DTOs.Dashboard;
using LibraryManagementSystem.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly LibraryDbContext _context;

        public DashboardService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<AdminDashboardResponseDto> GetAdminDashboardAsync()
{
    var dashboard = new AdminDashboardResponseDto();

    // Books
    dashboard.TotalBooks = await _context.Books.CountAsync();
    dashboard.TotalCopies = await _context.Books.SumAsync(x => x.TotalCopies);
    dashboard.AvailableCopies = await _context.Books.SumAsync(x => x.AvailableCopies);
    dashboard.BorrowedCopies = await _context.Books.SumAsync(x => x.BorrowedCopies);
    dashboard.ReservedCopies = await _context.Books.SumAsync(x => x.ReservedCopies);
    dashboard.DamagedCopies = await _context.Books.SumAsync(x => x.DamagedCopies);

    // Users
    dashboard.TotalUsers = await _context.Users.CountAsync();
    dashboard.ApprovedUsers = await _context.Users
        .CountAsync(x => x.RegistrationStatus == RegistrationStatus.Approved);

    dashboard.PendingUsers = await _context.Users
        .CountAsync(x => x.RegistrationStatus == RegistrationStatus.Pending);

    dashboard.RejectedUsers = await _context.Users
        .CountAsync(x => x.RegistrationStatus == RegistrationStatus.Rejected);

    // Employees
    dashboard.TotalEmployees = await _context.LibraryEmployees.CountAsync();

    dashboard.Librarians = await _context.LibraryEmployees
        .CountAsync(x => x.Role == StaffRole.Librarian);

    dashboard.ReturnVerificationOfficers = await _context.LibraryEmployees
        .CountAsync(x => x.Role == StaffRole.ReturnVerificationOfficer);

    // Damage
    dashboard.PendingDamageReports = await _context.BookDamageRecords
        .CountAsync(x => x.DamageStatus == DamageStatus.PendingAdminDecision);

    dashboard.BooksUnderRepair = await _context.BookDamageRecords
        .CountAsync(x => x.DamageStatus == DamageStatus.UnderRepair);

    dashboard.DisposedBooks = await _context.BookDamageRecords
        .CountAsync(x => x.DamageStatus == DamageStatus.Disposed);

    // Borrow & Return
    dashboard.BorrowedBooks = await _context.BorrowBooks
        .CountAsync(x => x.BorrowStatus == BorrowStatus.Borrowed);

    dashboard.ReturnedBooks = await _context.BorrowBooks
        .CountAsync(x => x.BorrowStatus == BorrowStatus.Returned);

    // Fine
    dashboard.TotalLateFineCollected = await _context.BorrowBooks
        .SumAsync(x => x.LateFine);

    dashboard.TotalDamageFineCollected = await _context.BorrowBooks
        .SumAsync(x => x.DamageFine);


    // Today's Activity
    dashboard.BooksBorrowedToday = await _context.BorrowBooks
    .CountAsync(x => x.BorrowDate.Date == DateTime.Today);

dashboard.BooksReturnedToday = await _context.BorrowBooks
    .CountAsync(x =>
        x.ReturnDate.HasValue &&
        x.ReturnDate.Value.Date == DateTime.Today);

dashboard.NewUsersToday = await _context.Users
    .CountAsync(x => x.CreatedDate.Date == DateTime.Today);

dashboard.FineCollectedToday = await _context.BorrowBooks
    .Where(x =>
        x.ReturnDate.HasValue &&
        x.ReturnDate.Value.Date == DateTime.Today)
    .SumAsync(x => x.LateFine + x.DamageFine);

    // Alerts
    dashboard.OverdueBooks = await _context.BorrowBooks
    .CountAsync(x =>
        x.BorrowStatus == BorrowStatus.Borrowed &&
        x.DueDate < DateTime.Now);

dashboard.MembershipsExpiringSoon = await _context.Users
    .CountAsync(x =>
        x.MembershipEndDate <= DateTime.Now.AddDays(7));

dashboard.LowStockBooks = await _context.Books
    .CountAsync(x => x.AvailableCopies <= 2);

    // Recent Activities
    dashboard.RecentActivities = await _context.BorrowBooks
    .Include(x => x.User)
    .Include(x => x.Book)
    .OrderByDescending(x => x.BorrowDate)
    .Take(5)
    .Select(x => new RecentActivityDto
    {
        Activity = x.BorrowStatus == BorrowStatus.Borrowed
            ? "Book Borrowed"
            : "Book Returned",

        UserName = x.User!.FullName,
        BookTitle = x.Book!.Title,
        Date = x.BorrowStatus == BorrowStatus.Borrowed
            ? x.BorrowDate
            : x.ReturnDate ?? x.BorrowDate
    })
    .ToListAsync();


    return dashboard;
}
    }
}