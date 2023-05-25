using FullFridge.API.Context;
using FullFridge.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FullFridge.API.Services
{
    public class UserService: IUserService
    {
        private readonly FullFridgeContext _context;

        public UserService(FullFridgeContext context)
        {
            _context = context;
        }
        public async Task<User> Authenticate(string email, string password)
        {
            var user = await _context.Users.Include(u => u.Details).SingleOrDefaultAsync(x => x.Email == email);

            if (user == null || !VerifyPassword(password, user.Password))
            {
                return null;
            }

            return user;
        }

        private static bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }

    public interface IUserService
    {
        Task<User> Authenticate(string email, string password);
    }
}
