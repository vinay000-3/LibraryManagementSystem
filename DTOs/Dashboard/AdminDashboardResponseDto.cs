namespace LibraryManagementSystem.DTOs.Dashboard
{
    public class AdminDashboardResponseDto
    {
        // Books
        public int TotalBooks { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public int BorrowedCopies { get; set; }
        public int ReservedCopies { get; set; }
        public int DamagedCopies { get; set; }

        // Users
        public int TotalUsers { get; set; }
        public int ApprovedUsers { get; set; }
        public int PendingUsers { get; set; }
        public int RejectedUsers { get; set; }

        // Employees
        public int TotalEmployees { get; set; }
        public int Librarians { get; set; }
        public int ReturnVerificationOfficers { get; set; }

        // Damage
        public int PendingDamageReports { get; set; }
        public int BooksUnderRepair { get; set; }
        public int DisposedBooks { get; set; }

        // Borrow
        public int BorrowedBooks { get; set; }
        public int ReturnedBooks { get; set; }

        // Fine
        public decimal TotalLateFineCollected { get; set; }
        public decimal TotalDamageFineCollected { get; set; }

        // Today's Activity
        public int BooksBorrowedToday { get; set; }
        public int BooksReturnedToday { get; set; }
        public int NewUsersToday { get; set; }
        public decimal FineCollectedToday { get; set; }

        // Alerts
        public int OverdueBooks { get; set; }
        public int MembershipsExpiringSoon { get; set; }
        public int LowStockBooks { get; set; }

        // Recent Activities
        public List<RecentActivityDto> RecentActivities { get; set; } = new();
    }
}