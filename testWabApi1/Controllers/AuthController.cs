using Microsoft.AspNetCore.Mvc;
using testWabApi1.DTO;
using testWabApi1.Models;
using testWabApi1.Services;

namespace testWabApi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(LoginUser loginUser)
        {
            if (await _authService.RegisterUser(loginUser))
            {
                return Ok("Registered");
            }

            return BadRequest("Smth went wrong during registration");

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUser loginUser)
        {
            if (await _authService.Login(loginUser))
            {
                var tokenString = _authService.GenerateTokenString(loginUser);

                var refreshToken = _authService.GenerateRefreshToken(loginUser);

                var tokensResponse = new TokensResponse
                {
                    AccessToken = tokenString,
                    RefreshToken = refreshToken
                };

                return Ok(tokensResponse);
            }

            return BadRequest();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDto refreshToken)
        {
            var response = _authService.RefreshAccessToken(refreshToken);

            if (response != null)
            {
                return Ok(response);
            }

            return Unauthorized("Token is not valid");
        }

        //[Authorize]
        [HttpPost("add-avatar/{userId}")]
        public async Task<IActionResult> AddAvatarToUser(string userId, [FromBody] string imagePath)
        {
            var response = await _authService.AddAvatarToUser(userId, imagePath);
            return Ok(response);
        }

        //[Authorize]
        [HttpGet("get-user-avatar/{userId}")]
        public async Task<IActionResult> GetUserAvatar(string userId)
        {
            var response = await _authService.GetAvatarOfUser(userId);
            return Ok(response);
        }

        //[Authorize]
        [HttpDelete("delete-user-avatar/{userId}")]
        public async Task<IActionResult> DeleteUserAvatar(string userId)
        {
            var response = await _authService.DeleteAvatarOfUser(userId);
            return Ok(response);
        }

        //[Authorize]
        [HttpGet("get-user-name/{userId}")]
        public async Task<IActionResult> GetUserName(string userId)
        {
            var response = await _authService.GetUserName(userId);
            return Ok(response);
        }
    }
}
