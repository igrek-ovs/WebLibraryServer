using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using testWabApi1.Models;

namespace testWabApi1.Data
{
    public class LibraryDbContext : IdentityDbContext<IdentityUser>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        
        public DbSet<Author> Authors { get; set; }
        
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<UserAvatars> UserAvatars { get; set; }

        public DbSet<BookRating> BookRatings { get; set; }

        public DbSet<BookComment> BookComments { get; set; }

        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    }
}
