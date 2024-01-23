using Microsoft.AspNetCore.Mvc;
using testWabApi1.Models;
using testWabApi1.Services;

namespace testWabApi1.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
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
            if(await _authService.RegisterUser(loginUser))
            {
                return Ok("Registered");
            }
            return BadRequest("Smth went wrong during registration");
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUser loginUser)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(await _authService.Login(loginUser))
            {
                var tokenString = _authService.GenerateTokenString(loginUser);

                return Ok(tokenString);
            }
            return BadRequest();
        }
    }
}
