using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

public DbSet<User> Users { get; set; }

public DbSet<MembershipPlan> MembershipPlans { get; set; }

public DbSet<BookCategory> BookCategories { get; set; }
    }
}