using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TaskManager.API.Models;

namespace TaskManager.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Check if there are any users
            if (context.Users.Any())
            {
                return; // DB has been seeded
            }

            // Create a default user
            var user = new User
            {
                Username = "admin",
                PasswordHash = ComputeHash("admin123")
            };

            context.Users.Add(user);
            context.SaveChanges();
        }

        private static string ComputeHash(string password)
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