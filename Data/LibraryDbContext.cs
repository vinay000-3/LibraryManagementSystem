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

        public DbSet<Book> Books { get; set; }

        public DbSet<BorrowBook> BorrowBooks { get; set; }

        public DbSet<LibraryEmployee> LibraryEmployees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BorrowBook>()
                .HasOne(b => b.Librarian)
                .WithMany()
                .HasForeignKey(b => b.LibrarianId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BorrowBook>()
                .HasOne(b => b.ReturnVerificationOfficer)
                .WithMany()
                .HasForeignKey(b => b.ReturnVerificationOfficerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}