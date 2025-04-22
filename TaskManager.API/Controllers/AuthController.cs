using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManager.API.DTOs;
using TaskManager.API.Services;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            var user = await _authService.AuthenticateAsync(loginDto.Username, loginDto.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });

            // In a real-world scenario, you'd generate a JWT token here
            // For simplicity, we'll just return the user ID and username
            return Ok(new
            {
                id = user.Id,
                username = user.Username
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDto)
        {
            var user = await _authService.RegisterAsync(registerDto.Username, registerDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username already exists" });

            return Ok(new
            {
                id = user.Id,
                username = user.Username
            });
        }
    }
}
