using testWabApi1.DTO;
using testWabApi1.Models;

namespace testWabApi1.Interfaces
{
    public interface IBookRepository
    {
        ICollection<BookDto> GetBooks();
        ICollection<BookDto> GetBooksOnPage(int page);
        bool CreateBook(Book book);
        bool UpdateBook(int bookId, Book book);
        bool DeleteBook(Book book);
        int CalculateBooks();
        Book GetBook(int id);
        bool Save();
        void UpdateImagePath(int bookId, string imagePath);
    }
}
