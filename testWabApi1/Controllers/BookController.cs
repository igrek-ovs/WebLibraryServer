using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using testWabApi1.Interfaces;
using testWabApi1.Models;
using testWabApi1.Services;

namespace testWabApi1.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
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
        public IActionResult GetBooks()
        {
            var books = bookRepository.GetBooks();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(books);
        }

        [HttpGet("GetBooksOnPage/{pageNumber}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Book>))]
        public IActionResult GetBooksOnPage(int pageNumber)
        {
            var books = bookRepository.GetBooksOnPage(pageNumber);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(books);
        }

        [HttpGet("GetNumberOfPages")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult GetNumberOfPages()
        {
            int totalBooks = bookRepository.CalculateBooks();

            int totalPages = (int)Math.Ceiling((double)totalBooks / 3);

            return Ok(totalPages);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateBook([FromBody] Book bookCreate)
        {
            if (bookCreate == null)
            {
                return BadRequest(ModelState);
            }

            var book = bookRepository.GetBooks()
                .Where(b => b.Title.Trim().ToUpper() == bookCreate.Title.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (book != null)
            {
                ModelState.AddModelError("", "Book exists elready");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (!bookRepository.CreateBook(bookCreate))
            {
                ModelState.AddModelError("", "Smth went wrong");
                return StatusCode(500, ModelState);
            }
            
            return Ok(bookCreate);
        }

        [HttpPost("upload-image")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var imgPath = await _blobService.UploadBlobAsync(file);
            return Ok(imgPath);
        }

        [HttpPut("{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateBook(int bookId, [FromBody] Book bookUpdate)
        {
            if (bookUpdate == null)
            {
                return BadRequest(ModelState);
            }

            if (bookId != bookUpdate.Id)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!bookRepository.UpdateBook(bookId, bookUpdate))
            {
                ModelState.AddModelError("", "Smth went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [Authorize]
        [HttpDelete("{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteBook(int bookId)
        {
            var bookToDelte = bookRepository.GetBook(bookId);

            if (bookToDelte == null)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!bookRepository.DeleteBook(bookToDelte))
            {
                ModelState.AddModelError("", "Smth went wrong");
                return StatusCode(500, ModelState);

            }
            return Ok("Deleted");
        }


        [HttpGet("{bookId}")]
        [ProducesResponseType(200, Type = typeof(Book))]
        public IActionResult GetBook(int bookId)
        {
            var book = bookRepository.GetBook(bookId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(book);
        }

    }

}
