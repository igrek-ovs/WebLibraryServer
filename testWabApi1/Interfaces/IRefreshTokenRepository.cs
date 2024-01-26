using testWabApi1.Models;

namespace testWabApi1.Interfaces
{
    public interface IRefreshTokenRepository
    {
        void AddRefreshToken(RefreshToken refreshToken);
        void RemoveRefreshToken(string userId);
        RefreshToken GetRefreshTokenByUserId(string userId);
    }
}
