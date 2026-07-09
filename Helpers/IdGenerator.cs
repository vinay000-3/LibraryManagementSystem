namespace LibraryManagementSystem.Helpers
{
    public static class IdGenerator
    {
        public static string GenerateUserId(int count)
        {
            return $"USR{(count + 1).ToString("D4")}";
        }

        public static string GenerateBookId(int count)
        {
            return $"BK{(count + 1).ToString("D4")}";
        }

        public static string GenerateBookCopyId(int count)
        {
            return $"CP{(count + 1).ToString("D4")}";
        }

        public static string GenerateBorrowId(int count)
        {
            return $"BR{(count + 1).ToString("D4")}";
        }

        public static string GenerateReturnId(int count)
        {
            return $"RT{(count + 1).ToString("D4")}";
        }

        public static string GenerateReservationId(int count)
        {
            return $"RS{(count + 1).ToString("D4")}";
        }

        public static string GenerateFineId(int count)
        {
            return $"FN{(count + 1).ToString("D4")}";
        }
    }
}