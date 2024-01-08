using Bulky_crud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace Bulky_crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly CategoryContext _userContext;
        public UserController(CategoryContext userContext)
        {
            _userContext = userContext;
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Username and password are required");
            }


            var existingUser = _userContext.Users.FirstOrDefault(u => u.Username == user.Username);
            if (existingUser != null)
            {
                return BadRequest("Username already exists");
            }


            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);


            user.Password = hashedPassword;


            _userContext.Users.Add(user);
            _userContext.SaveChanges();

            return Ok("Registration successful");
        }

        [HttpPost("login")]
        public IActionResult Login(User user)
        {
            var existingUser = _userContext.Users.FirstOrDefault(u => u.Username == user.Username);

            if (existingUser == null)
            {
                return Unauthorized("Invalid credentials");
            }


            if (!BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok("Login successful");
        }
    }
}
