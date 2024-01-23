using Microsoft.AspNetCore.Mvc;
using testWabApi1.Models;

namespace testWabApi1.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(LoginUser loginUser);
        Task<bool> Login(LoginUser loginUser);
        string GenerateTokenString(LoginUser loginUser);
    }
}