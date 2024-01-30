using testWabApi1.DTO;
using testWabApi1.Models;

namespace testWabApi1.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(LoginUser loginUser);
        Task<bool> Login(LoginUser loginUser);
        string GenerateTokenString(LoginUser loginUser);
        RefreshToken GenerateRefreshToken(LoginUser loginUser);
        string RefreshAccessToken(RefreshTokenDto refreshToken);
    }
}