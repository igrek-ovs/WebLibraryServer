using Microsoft.AspNetCore.Mvc;
using testWabApi1.Models;
using testWabApi1.Services.Interfaces;

namespace testWabApi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookRatingController : Controller
    {
        private readonly IBookRatingService _bookRatingService;

        public BookRatingController(IBookRatingService bookRatingService)
        {
            _bookRatingService = bookRatingService;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddOrUpdateBookRating([FromBody] BookRating bookRating)
        {
            await _bookRatingService.AddOrUpdateRating(bookRating);
            return Ok(bookRating);
        }

        [HttpGet("get-rating-for-book/{bookId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetRatingOfBook(int bookId)
        {
            var response = await _bookRatingService.GetAverageRatingForBook(bookId);
            return Ok(response);
        }

        [HttpGet("user-rating")]
        public async Task<IActionResult> GetUserRatingForBook(string userId, int bookId)
        {
            var rating = await _bookRatingService.GetUserRatingForBook(userId, bookId);
            return Ok(rating);
        }
    }
}
