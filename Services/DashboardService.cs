using LibraryManagementSystem.Data;
using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.DTOs.Dashboard;
using LibraryManagementSystem.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace LibraryManagementSystem.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly LibraryDbContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;  

        public DashboardService(LibraryDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
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

public async Task<UserDashboardResponseDto> GetUserDashboardAsync()
{
    var userId = _httpContextAccessor.HttpContext?.User
        .FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (string.IsNullOrEmpty(userId))
        throw new Exception("User is not authenticated.");

    var user = await _context.Users
        .Include(x => x.MembershipPlan)
        .FirstOrDefaultAsync(x => x.UserId == userId);

    if (user == null)
        throw new Exception("User not found.");

    var borrowRecords = await _context.BorrowBooks
        .Include(x => x.Book)
        .Where(x => x.UserId == userId)
        .ToListAsync();

    var dashboard = new UserDashboardResponseDto
    {
        UserId = user.UserId,
        FullName = user.FullName,
        MembershipPlan = user.MembershipPlan!.MembershipName,
        MembershipStatus = user.RegistrationStatus.ToString(),
        MembershipStartDate = user.MembershipStartDate,
        MembershipEndDate = user.MembershipEndDate,

        CurrentlyBorrowedBooks = borrowRecords.Count(x =>
            x.BorrowStatus == BorrowStatus.Borrowed),

        TotalBorrowedBooks = borrowRecords.Count,

        TotalReturnedBooks = borrowRecords.Count(x =>
            x.BorrowStatus == BorrowStatus.Returned),

        OverdueBooks = borrowRecords.Count(x =>
            x.BorrowStatus == BorrowStatus.Borrowed &&
            x.DueDate.Date < DateTime.Now.Date),

        TotalLateFine = borrowRecords.Sum(x => x.LateFine),

        TotalDamageFine = borrowRecords.Sum(x => x.DamageFine),

        PendingFine = borrowRecords
            .Where(x => !x.LateFinePaid || !x.DamageFinePaid)
            .Sum(x => x.LateFine + x.DamageFine)
    };
        dashboard.CurrentBorrowedBooks = borrowRecords
        .Where(x => x.BorrowStatus == BorrowStatus.Borrowed)
        .Select(x => new UserBorrowedBookDto
        {
            BorrowId = x.BorrowId,
            BookTitle = x.Book!.Title,
            BorrowDate = x.BorrowDate,
            DueDate = x.DueDate,
            DaysRemaining = (x.DueDate.Date - DateTime.Now.Date).Days
        })
        .ToList();

    dashboard.RecentActivities = borrowRecords
        .OrderByDescending(x => x.BorrowDate)
        .Take(5)
        .Select(x => new UserActivityDto
        {
            BookTitle = x.Book!.Title,
            Action = x.BorrowStatus.ToString(),
            Date = x.ReturnDate ?? x.BorrowDate
        })
        .ToList();

    return dashboard;
}
    }
}