namespace LibraryManagementSystem.Helpers
{
    public static class IdGenerator
    {
        // Common Method
        private static string GenerateNextId(string prefix, string? lastId)
        {
            if (string.IsNullOrWhiteSpace(lastId))
            {
                return $"{prefix}0001";
            }

            int lastNumber = int.Parse(lastId.Substring(prefix.Length));
            return $"{prefix}{(lastNumber + 1):D4}";
        }

        // User
        public static string GenerateUserId(string? lastUserId)
        {
            return GenerateNextId("USR", lastUserId);
        }

        // Membership Plan
        public static string GenerateMembershipPlanId(string? lastMembershipPlanId)
        {
            return GenerateNextId("MP", lastMembershipPlanId);
        }

        // Book Category
        public static string GenerateBookCategoryId(string? lastBookCategoryId)
        {
            return GenerateNextId("BC", lastBookCategoryId);
        }

        // Book
        public static string GenerateBookId(string? lastBookId)
        {
            return GenerateNextId("BK", lastBookId);
        }

        // Book Copy
        public static string GenerateBookCopyId(string? lastBookCopyId)
        {
            return GenerateNextId("CP", lastBookCopyId);
        }

        // Borrow
        public static string GenerateBorrowId(string? lastBorrowId)
        {
            return GenerateNextId("BR", lastBorrowId);
        }

        // Return
        public static string GenerateReturnId(string? lastReturnId)
        {
            return GenerateNextId("RT", lastReturnId);
        }

        // Reservation
        public static string GenerateReservationId(string? lastReservationId)
        {
            return GenerateNextId("RS", lastReservationId);
        }

        // Fine
        public static string GenerateFineId(string? lastFineId)
        {
            return GenerateNextId("FN", lastFineId);
        }
    }
}