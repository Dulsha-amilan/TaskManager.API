using System.Threading.Tasks;
using TaskManager.API.Models;

namespace TaskManager.API.Services
{
    public interface IAuthService
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> RegisterAsync(string username, string password);
        string ComputeHash(string password);
    }
}
