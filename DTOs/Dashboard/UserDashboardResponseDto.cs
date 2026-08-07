namespace LibraryManagementSystem.DTOs.Dashboard
{
    public class UserDashboardResponseDto
    {
        // Profile
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string MembershipPlan { get; set; } = string.Empty;
        public string MembershipStatus { get; set; } = string.Empty;
        public DateTime MembershipStartDate { get; set; }
        public DateTime MembershipEndDate { get; set; }

        // Statistics
        public int CurrentlyBorrowedBooks { get; set; }
        public int TotalBorrowedBooks { get; set; }
        public int TotalReturnedBooks { get; set; }
        public int OverdueBooks { get; set; }

        // Fine Summary
        public decimal TotalLateFine { get; set; }
        public decimal TotalDamageFine { get; set; }
        public decimal PendingFine { get; set; }

        // Current Borrowed Books
        public List<UserBorrowedBookDto> CurrentBorrowedBooks { get; set; } = new();

        // Recent Activity
        public List<UserActivityDto> RecentActivities { get; set; } = new();
    }

    public class UserBorrowedBookDto
    {
        public string BorrowId { get; set; } = string.Empty;
        public string BookTitle { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysRemaining { get; set; }
    }

    public class UserActivityDto
    {
        public string BookTitle { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}