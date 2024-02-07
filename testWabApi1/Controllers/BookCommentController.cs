using Microsoft.AspNetCore.Mvc;
using testWabApi1.Models;
using testWabApi1.Services;
using testWabApi1.Services.Interfaces;

namespace testWabApi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCommentController : Controller
    {
        private readonly IBookCommentService _bookCommentService;

        public BookCommentController(IBookCommentService bookService) 
        {
            _bookCommentService = bookService;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddComment(BookComment bookComment)
        {
            await _bookCommentService.AddComment(bookComment);
            return Ok(bookComment);
        }

        [HttpGet("get-comments-for-book/{bookId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCommentsOfBook(int bookId)
        {
            var response = await _bookCommentService.GetAllCommentsForBook(bookId);
            return Ok(response);
        }

        [HttpGet("get-ids-of-comments-for-user")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCommentsOfBook(int bookId, string userId)
        {
            var response = await _bookCommentService.GetAllUserCommentIdsForBook(bookId, userId);
            return Ok(response);
        }
    }
}
