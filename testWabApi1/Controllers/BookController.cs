using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using testWabApi1.Interfaces;
using testWabApi1.Models;
using testWabApi1.Services;

namespace testWabApi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BookController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly IBlobService _blobService;

        public BookController(IBookRepository bookRepository, IBlobService blobService)
        {
            this.bookRepository = bookRepository;
            _blobService = blobService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Book>))]
        public async Task<IActionResult> GetBooks()
        {
            var books = await bookRepository.GetBooks();
            return Ok(books);
        }

        [HttpGet("get-authors")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Book>))]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await bookRepository.GetAuthors();
            return Ok(authors);
        }

        [HttpGet("GetBooksOnPage/{pageNumber}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Book>))]
        public async Task<IActionResult> GetBooksOnPage(int pageNumber)
        {
            var books = await bookRepository.GetBooksOnPage(pageNumber);
            return Ok(books);
        }

        [HttpGet("GetNumberOfPages")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetNumberOfPages()
        {
            int totalBooks = await bookRepository.CalculateBooks();

            int totalPages = (int)Math.Ceiling((double)totalBooks / 3);

            return Ok(totalPages);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateBook([FromBody] Book bookCreate)
        {
            if (bookCreate == null)
            {
                return BadRequest(ModelState);
            }

            var flag = await bookRepository.IsBookExist(bookCreate.Title);

            if (flag)
            {
                return BadRequest("Book already exists");
            }

            await bookRepository.CreateBook(bookCreate);

            return Ok(bookCreate);
        }

        [HttpPost("upload-image")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var imgPath = await _blobService.UploadBlobAsync(file);
            return Ok(imgPath);
        }

        [HttpPut("{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdateBook(int bookId, [FromBody] Book bookUpdate)
        {
            var book = await bookRepository.GetBook(bookId);
            
            await _blobService.DeleteBlobAsync(book.ImagePath);
            
            var update = await bookRepository.UpdateBook(bookId, bookUpdate);
            
            return Ok(update);
        }

        [Authorize]
        [HttpDelete("{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            var bookToDelete = await bookRepository.GetBook(bookId);

            if (bookToDelete == null)
            {
                return BadRequest("Book doesnt exist");
            }

            var delete = await bookRepository.DeleteBook(bookToDelete);
            
            await _blobService.DeleteBlobAsync(bookToDelete.ImagePath);
            
            return Ok(delete);
        }

        [HttpGet("{bookId}")]
        [ProducesResponseType(200, Type = typeof(Book))]
        public async Task<IActionResult> GetBook(int bookId)
        {
            var book = await bookRepository.GetBook(bookId);
            return Ok(book);
        }

        [HttpGet("get-book-by-name/{title}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Book>))]
        public async Task<IActionResult> GetBooksByName(string title)
        {
            var books = await bookRepository.GetBooksByName(title);
            return Ok(books);
        }
    }

}
