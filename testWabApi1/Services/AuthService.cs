using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using testWabApi1.Models;

namespace testWabApi1.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public string GenerateTokenString(LoginUser loginUser)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loginUser.UserName),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            SigningCredentials signinCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);


            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
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
    }
}