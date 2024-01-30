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

        public ICollection<BookDto> GetBooks()
        {
            var books = _libraryDbContext.Books
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
        .ToList();

            return books;
        }

        public bool CreateBook(Book book)
        {
            _libraryDbContext.Add(book);
            return Save();
        }

        public bool Save()
        {
            var saved = _libraryDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateBook(int bookId, Book book)
        {
            var existingBook = _libraryDbContext.Books.Include(b => b.Author).FirstOrDefault(b => b.Id == bookId);
            if (book.AuthorId > 0)
            {
                var existingAuthor = _libraryDbContext.Authors.Find(book.AuthorId);

                if (existingAuthor != null)
                {
                    existingBook.Author = existingAuthor;
                }
                else
                {
                    Console.WriteLine("Author not found");
                }
            }
            existingBook.Title = book.Title;
            existingBook.Genre = book.Genre;
            existingBook.ImagePath = book.ImagePath;
            return Save();
        }
        public bool DeleteBook(Book book)
        {
            var existingBook = _libraryDbContext.Books.FirstOrDefault(b => b.Id == book.Id);

            if (existingBook == null)
            {
                return false;
            }

            _libraryDbContext.Books.Remove(existingBook);
            return Save();
        }

        public Book GetBook(int id) => _libraryDbContext.Books.FirstOrDefault(b => b.Id == id);

        public ICollection<BookDto> GetBooksOnPage(int page)
        {
            int pageSize = 3;

            var books = _libraryDbContext.Books
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
                .ToList();

            return books;
        }

        public int CalculateBooks()
        {
            return _libraryDbContext.Books.Count();
        }

        public void UpdateImagePath(int bookId, string imagePath)
        {
            var book = _libraryDbContext.Books.FirstOrDefault(b => b.Id == bookId);

            if (book != null)
            {
                book.ImagePath = imagePath;
                _libraryDbContext.SaveChanges();
            }
        }
    }
}
