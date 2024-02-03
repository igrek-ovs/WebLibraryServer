using testWabApi1.Data;
using testWabApi1.Interfaces;
using testWabApi1.Models;

namespace testWabApi1.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly LibraryDbContext _dbContext;

        public RefreshTokenRepository(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddRefreshToken(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Add(refreshToken);
            
            _dbContext.SaveChanges();
        }

        public void RemoveRefreshToken(string userId)
        {
            var token = _dbContext.RefreshTokens.SingleOrDefault(rt => rt.UserId == userId);

            if (token != null)
            {
                _dbContext.RefreshTokens.Remove(token);
                
                _dbContext.SaveChanges();
            }
        }

        public RefreshToken GetRefreshTokenByUserId(string userId)
        {
            return _dbContext.RefreshTokens.SingleOrDefault(rt => rt.UserId == userId);
        }
    }
}
