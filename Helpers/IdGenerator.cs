namespace LibraryManagementSystem.Helpers
{
    public static class IdGenerator
    {
        // Common Method
        private static string GenerateId(string prefix, int count)
        {
            return $"{prefix}{(count + 1).ToString("D4")}";
        }

        // User
        public static string GenerateUserId(int count)
        {
            return GenerateId("USR", count);
        }

        // Membership Plan
        public static string GenerateMembershipPlanId(int count)
        {
            return GenerateId("MP", count);
        }

        // Book Category
        public static string GenerateBookCategoryId(int count)
        {
            return GenerateId("BC", count);
        }

        // Book
        public static string GenerateBookId(int count)
        {
            return GenerateId("BK", count);
        }

        // Book Copy
        public static string GenerateBookCopyId(int count)
        {
            return GenerateId("CP", count);
        }

        // Borrow
        public static string GenerateBorrowId(int count)
        {
            return GenerateId("BR", count);
        }

        // Return
        public static string GenerateReturnId(int count)
        {
            return GenerateId("RT", count);
        }

        // Reservation
        public static string GenerateReservationId(int count)
        {
            return GenerateId("RS", count);
        }

        // Fine
        public static string GenerateFineId(int count)
        {
            return GenerateId("FN", count);
        }
    }
}