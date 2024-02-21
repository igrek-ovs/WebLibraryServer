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

        Task<bool> AddAvatarToUser(string userId, string imagePath);

        Task<string> GetAvatarOfUser(string userId);

        Task<bool> DeleteAvatarOfUser(string userId);

        Task<string> GetUserName(string userId);
    }
}