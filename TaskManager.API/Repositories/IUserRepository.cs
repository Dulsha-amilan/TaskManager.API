using System.Threading.Tasks;
using TaskManager.API.Models;

namespace TaskManager.API.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> CreateUserAsync(User user);
        Task<bool> UsernameExistsAsync(string username);
    }
}
