using FullFridge.API.Context;
using FullFridge.API.Models;
using FullFridge.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullFridge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly FullFridgeContext _context;
        private readonly IUserService _userService;

        public UserController(FullFridgeContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        //POST: api/User/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register(User user)
        {
            if (await _userService.UserExists(user.Email))
            {
                return BadRequest("User already exists");
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //POST: api/User/Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(string email, string password)
        {
            var user = await _userService.Authenticate(email, password);

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }
    }
}
