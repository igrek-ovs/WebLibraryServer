using testWabApi1.Models;

namespace testWabApi1.Services.Interfaces
{
    public interface IBookCommentService
    {
        Task AddComment(BookComment bookComment);

        Task RemoveCommentsOfBook(int bookId);

        Task<ICollection<BookComment>> GetAllCommentsForBook(int bookId);

        Task<ICollection<int>> GetAllUserCommentIdsForBook(int bookId, string userId);
    }
}
