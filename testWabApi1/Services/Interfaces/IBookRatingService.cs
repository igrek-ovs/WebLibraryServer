using testWabApi1.Models;

namespace testWabApi1.Services.Interfaces
{
    public interface IBookRatingService
    {
        Task AddOrUpdateRating(BookRating bookRating);

        Task<double> GetAverageRatingForBook(int bookId);

        Task<int> GetUserRatingForBook(string userId, int bookId);
    }
}
