using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using testWabApi1.Data;
using testWabApi1.DTO;
using testWabApi1.Interfaces;
using testWabApi1.Models;

namespace testWabApi1.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IRefreshTokenRepository _refreshTokenManager;
        private readonly LibraryDbContext _libraryDbContext;
        private readonly IBlobService _blobService;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration config, IRefreshTokenRepository refreshTokenManager, LibraryDbContext libraryDbContext, IBlobService blobService)
        {
            _userManager = userManager;
            _config = config;
            _refreshTokenManager = refreshTokenManager;
            _libraryDbContext = libraryDbContext;
            _blobService = blobService;
        }

        public string GenerateTokenString(LoginUser loginUser)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, loginUser.UserName),
                new(ClaimTypes.Role, "Admin")
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            SigningCredentials signinCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);


            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:Audience").Value,
                signingCredentials: signinCred
                );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return tokenString;
        }

        public async Task<bool> Login(LoginUser loginUser)
        {
            var identityUser = await _userManager.FindByEmailAsync(loginUser.UserName);

            if (identityUser == null)
            {
                return false;
            }
            return await _userManager.CheckPasswordAsync(identityUser, loginUser.Password);
        }

        public RefreshToken GenerateRefreshToken(LoginUser loginUser)
        {
            var identityUser = _userManager.FindByEmailAsync(loginUser.UserName);

            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddMonths(1),
                Created = DateTime.Now,
                UserId = identityUser.Result.Id
            };

            _refreshTokenManager.RemoveRefreshToken(refreshToken.UserId);

            _refreshTokenManager.AddRefreshToken(refreshToken);

            return refreshToken;
        }

        public async Task<bool> RegisterUser(LoginUser loginUser)
        {
            var identityUser = new IdentityUser
            {
                UserName = loginUser.UserName,
                Email = loginUser.UserName
            };

            var result = await _userManager.CreateAsync(identityUser, loginUser.Password);

            return result.Succeeded;
        }

        public string RefreshAccessToken(RefreshTokenDto refreshToken)
        {
            var savedToken = _refreshTokenManager.GetRefreshTokenByUserId(refreshToken.UserId);

            if (savedToken.Token != refreshToken.Token)
                return null;

            if (savedToken.Expires < DateTime.Now)
                return null;

            var identityUser = _userManager.FindByIdAsync(refreshToken.UserId).Result;

            var user = new LoginUser
            {
                UserName = identityUser.UserName,
                Password = ""
            };

            return GenerateTokenString(user);
        }

        public async Task<bool> AddAvatarToUser(string userId, string imagePath)
        {
            var userAvatar = new UserAvatars
            {
                UserId = userId,
                ImagePath = imagePath
            };
            await _libraryDbContext.AddAsync(userAvatar);
            return await Save();

        }

        public async Task<string> GetAvatarOfUser(string userId)
        {
            var userAvatar = await _libraryDbContext.UserAvatars.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            return userAvatar?.ImagePath;
        }

        public async Task<bool> DeleteAvatarOfUser(string userId)
        {
            var userAvatar = await _libraryDbContext.UserAvatars.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            if (userAvatar == null)
            {
                return false;
            }
            await _blobService.DeleteBlobAsync(userAvatar.ImagePath);
            _libraryDbContext.UserAvatars.Remove(userAvatar);
            return await Save();
        }

        public async Task<string> GetUserName(string userId)
        {
            var identityUser = await _userManager.FindByIdAsync(userId);
            return identityUser.UserName;
        }

        private async Task<bool> Save()
        {
            var saved = await _libraryDbContext.SaveChangesAsync();
            return saved > 0;
        }
    }
}