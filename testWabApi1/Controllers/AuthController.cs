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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var response = _authService.RefreshAccessToken(refreshToken);

            if (response != null)
            {
                return Ok(response);
            }
            
            return Unauthorized("Token is not valid");
        }
    }
}
