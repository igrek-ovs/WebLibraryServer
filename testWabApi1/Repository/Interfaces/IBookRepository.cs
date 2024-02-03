using testWabApi1.DTO;
using testWabApi1.Models;

namespace testWabApi1.Interfaces
{
    public interface IBookRepository
    {
        Task<ICollection<BookDto>> GetBooks();
        
        Task<ICollection<BookDto>> GetBooksOnPage(int page);
        
        Task<bool> CreateBook(Book book);
        
        Task<bool> UpdateBook(int bookId, Book book);
        
        Task<bool> DeleteBook(Book book);
        
        Task<int> CalculateBooks();
        
        Task<Book> GetBook(int id);
        
        Task<bool> Save();
        
        Task UpdateImagePath(int bookId, string imagePath);
        
        Task<bool> IsBookExist(string title);
        
        Task<ICollection<AuthorDto>> GetAuthors();
    }
}
