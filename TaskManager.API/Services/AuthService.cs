using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.Models;
using TaskManager.API.Repositories;

namespace TaskManager.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null)
                return null;

            var passwordHash = ComputeHash(password);

            if (user.PasswordHash != passwordHash)
                return null;

            return user;
        }

        public async Task<User> RegisterAsync(string username, string password)
        {
            if (await _userRepository.UsernameExistsAsync(username))
                return null;

            var user = new User
            {
                Username = username,
                PasswordHash = ComputeHash(password)
            };

            return await _userRepository.CreateUserAsync(user);
        }

        public string ComputeHash(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
