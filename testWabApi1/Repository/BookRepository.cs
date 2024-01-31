using Microsoft.EntityFrameworkCore;
using testWabApi1.Data;
using testWabApi1.DTO;
using testWabApi1.Interfaces;
using testWabApi1.Models;

namespace testWabApi1.Repository
{
    public class BookRepository : IBookRepository
    {

        private readonly LibraryDbContext _libraryDbContext;
        public BookRepository(LibraryDbContext context)
        {
            _libraryDbContext = context;
        }

        public async Task<ICollection<BookDto>> GetBooks()
        {
            var books = await _libraryDbContext.Books
        .Include(b => b.Author)
        .Select(b => new BookDto
        {
            Id = b.Id,
            Title = b.Title,
            Genre = b.Genre,
            AuthorId = b.AuthorId,
            AuthorName = b.Author != null ? b.Author.Name : "Unknown Author",
            ImagePath = b.ImagePath != null ? b.ImagePath : "No image"
        })
        .ToListAsync();

            return books;
        }

        public async Task<bool> CreateBook(Book book)
        {
            await _libraryDbContext.AddAsync(book);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _libraryDbContext.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateBook(int bookId, Book book)
        {
            var existingBook = await _libraryDbContext.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == bookId);
            if (book.AuthorId > 0)
            {
                var existingAuthor = await _libraryDbContext.Authors.FindAsync(book.AuthorId);
                existingBook.Author = existingAuthor;
            }

            existingBook.Title = book.Title;
            existingBook.Genre = book.Genre;
            existingBook.ImagePath = book.ImagePath;
            return await Save();
        }
        public async Task<bool> DeleteBook(Book book)
        {
            var existingBook = await _libraryDbContext.Books.FirstOrDefaultAsync(b => b.Id == book.Id);

            if (existingBook == null)
            {
                return false;
            }

            _libraryDbContext.Books.Remove(existingBook);
            return await Save();
        }

        public async Task<Book> GetBook(int id)
        {
            return await _libraryDbContext.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<ICollection<BookDto>> GetBooksOnPage(int page)
        {
            int pageSize = 3;

            var books =await _libraryDbContext.Books
                .Include(b => b.Author)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Genre = b.Genre,
                    AuthorId = b.AuthorId,
                    AuthorName = b.Author != null ? b.Author.Name : "Unknown Author",
                    ImagePath = b.ImagePath != null ? b.ImagePath : "No image"
                })
                .ToListAsync();

            return books;
        }

        public async Task<int> CalculateBooks()
        {
            return await _libraryDbContext.Books.CountAsync();
        }

        public async Task UpdateImagePath(int bookId, string imagePath)
        {
            var book = await _libraryDbContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);

            if (book != null)
            {
                book.ImagePath = imagePath;
                await _libraryDbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> IsBookExist(string title)
        {
            return await _libraryDbContext.Books.AnyAsync(b => b.Title.ToUpper().Trim() == title.ToUpper().Trim());   
        }
    }
}
