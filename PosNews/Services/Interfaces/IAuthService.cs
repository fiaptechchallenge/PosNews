using PosNews.Models;

namespace PosNews.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateTokenString(LoginUser user);
        Task<bool> Login(LoginUser user);
        Task<bool> RegisterUser(RegisterUser user);
    }
}