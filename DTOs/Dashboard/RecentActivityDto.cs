namespace LibraryManagementSystem.DTOs.Dashboard
{
    public class RecentActivityDto
    {
        public string Activity { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string BookTitle { get; set; } = string.Empty;

        public DateTime Date { get; set; }
    }
}