using FullFridge.API.Models;
using FullFridge.API.Models.Enums;
using FullFridge.Model;
using FullFridge.Model.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullFridge.API.Services
{
    public class UserService: IUserService
    {
        private readonly IDapperRepository _repository;

        public UserService(IDapperRepository repository)
        {
            _repository= repository;
        }
        public async Task<UserDTO> Authenticate(string email, string password)
        {
            var user = await _repository.QueryFirstOrDefault<User>(
                SqlQueryHelper.GetUserFromEmail, new { email });

            if (user == null || !VerifyPassword(password, user.Password))
            {
                return null;
            }

            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                Role = user.Role
            };
        }

        public async Task<Result> RegisterUser(User user)
        {
            if (await UserExists(user.Email))
            {
                return new Result(StatusCodes.Status400BadRequest, "User already exists");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            user.Role = user.Role == Roles.RoleAdmin ? Roles.RoleAdmin : Roles.RoleUser;

            await _repository.Execute(
                SqlQueryHelper.InsertUser, user);

            return new Result(StatusCodes.Status201Created);
        }

        private async Task<bool> UserExists(string email)
        {
            var user = await _repository.QueryFirstOrDefault<User>(
                SqlQueryHelper.GetUserFromEmail, new { email });

            return user != null;
        }

        private static bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }

    public interface IUserService
    {
        Task<UserDTO> Authenticate(string email, string password);
        Task<Result> RegisterUser(User user);

    }
}
