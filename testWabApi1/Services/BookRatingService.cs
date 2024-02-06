using Microsoft.EntityFrameworkCore;
using testWabApi1.Data;
using testWabApi1.Models;
using testWabApi1.Services.Interfaces;

namespace testWabApi1.Services
{
    public class BookRatingService : IBookRatingService
    {
        private readonly LibraryDbContext _libraryDbContext;

        public BookRatingService(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }

        public async Task AddOrUpdateRating(BookRating bookRating)
        {
            var existingRating = await _libraryDbContext.BookRatings
            .FirstOrDefaultAsync(r => r.UserId == bookRating.UserId && r.BookId == bookRating.BookId);

            if (existingRating != null)
            {
                existingRating.Rating = bookRating.Rating;
            }
            else
            {
                await _libraryDbContext.BookRatings.AddAsync(bookRating);
            }

            await _libraryDbContext.SaveChangesAsync();
        }

        public async Task<double> GetAverageRatingForBook(int bookId)
        {
            var ratingsForBook = await _libraryDbContext.BookRatings.Where(r => r.BookId == bookId).ToListAsync();

            if (ratingsForBook.Any())
            {
                double totalRating = ratingsForBook.Sum(r => r.Rating);
                return totalRating / ratingsForBook.Count;
            }
            else
            {
                return -1;
            }
        }

        public async Task<int> GetUserRatingForBook(string userId, int bookId)
        {
            var rating = await _libraryDbContext.BookRatings
            .FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId);

            if (rating != null)
            {
                return rating.Rating;
            }
            else
            {
                return -1;
            }
        }
    }
}
