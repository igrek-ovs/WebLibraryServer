using Microsoft.EntityFrameworkCore;
using testWabApi1.Data;
using testWabApi1.Models;
using testWabApi1.Services.Interfaces;

namespace testWabApi1.Services
{
    public class BookCommentService : IBookCommentService
    {
        private readonly LibraryDbContext _libraryDbContext;

        public BookCommentService(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }

        public async Task AddComment(BookComment bookComment)
        {
            await _libraryDbContext.BookComments.AddAsync(bookComment);
            await _libraryDbContext.SaveChangesAsync();
        }

        public async Task RemoveCommentsOfBook(int bookId)
        {
            var commentsToRemove = await _libraryDbContext.BookComments
                .Where(c => c.BookId == bookId)
                .ToListAsync();

            _libraryDbContext.BookComments.RemoveRange(commentsToRemove);
            await _libraryDbContext.SaveChangesAsync();
        }

        public async Task<ICollection<BookComment>> GetAllCommentsForBook(int bookId)
        {
            var comments = await _libraryDbContext.BookComments.Where(b => b.BookId == bookId).ToListAsync();
            return comments;
        }

        public async Task<ICollection<int>> GetAllUserCommentIdsForBook(int bookId, string userId)
        {
            var commentIds = await _libraryDbContext.BookComments
                .Where(b => b.BookId == bookId && b.UserId == userId)
                .Select(c => c.Id)
                .ToListAsync();

            return commentIds;
        }
    }
}
