using AutoFixture;
using FullFridge.API.Models;
using FullFridge.API.Services;
using FullFridge.Model;
using FullFridge.Model.Helpers;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullFridge.Test
{
    public class UserServiceTest
    {
        private readonly UserService _sut;
        private readonly IDapperRepository _repository = Substitute.For<IDapperRepository>();
        private readonly IFixture _fixture = new Fixture();

        private readonly string userEmail = "test@test.pl";
        private readonly string userPassword = "password";

        public UserServiceTest()
        {
            _sut = new UserService(_repository);
        }

        [Fact]
        public async void Authenticate_ShouldAuthenticateUser_WhenCorrectEmailAndPasswordProvided()
        {
            var user = _fixture.Build<User>()
                .With(u => u.Email, userEmail)
                .With(u => u.Password, BCrypt.Net.BCrypt.HashPassword(userPassword))
                .Create();
            _repository.QueryFirstOrDefault<User>(SqlQueryHelper.GetUserFromEmail, Arg.Any<object>()).Returns(Task.FromResult(user));


            var result = await _sut.Authenticate(userEmail, userPassword);


            Assert.Equal(result.Email, user.Email);
        }

        [Fact]
        public async void Authenticate_ShouldNotAuthenticateUser_WhenUserNotExist()
        {
            _repository.QueryFirstOrDefault<User>(SqlQueryHelper.GetUserFromEmail, Arg.Any<object>()).Returns(Task.FromResult<User>(null));


            var result = await _sut.Authenticate(userEmail, userPassword);


            Assert.Null(result);
        }

        [Fact]
        public async void Authenticate_ShouldNotAuthenticateUser_WhenPasswordIncorrect()
        {
            var user = _fixture.Build<User>()
                .With(u => u.Email, userEmail)
                .With(u => u.Password, BCrypt.Net.BCrypt.HashPassword("differentPassword"))
                .Create();
            _repository.QueryFirstOrDefault<User>(SqlQueryHelper.GetUserFromEmail, Arg.Any<object>()).Returns(Task.FromResult(user));


            var result = await _sut.Authenticate(userEmail, userPassword);


            Assert.Null(result);
        }

        [Fact]
        public async void RegisterUser_ShouldAddUser_WhenUserNotExist()
        {
            var user = _fixture.Build<User>()
                .With(u => u.Email, userEmail)
                .With(u => u.Password, BCrypt.Net.BCrypt.HashPassword(userPassword))
                .Create();
            _repository.QueryFirstOrDefault<User>(SqlQueryHelper.GetUserFromEmail, Arg.Any<object>()).Returns(Task.FromResult<User>(null));


            var result = await _sut.RegisterUser(user);


            Assert.Equal(201, result.Status);
        }

        [Fact]
        public async void RegisterUser_ShouldNotAddUser_WhenUserExist()
        {
            var user = _fixture.Build<User>()
                .With(u => u.Email, userEmail)
                .With(u => u.Password, BCrypt.Net.BCrypt.HashPassword(userPassword))
                .Create();
            _repository.QueryFirstOrDefault<User>(SqlQueryHelper.GetUserFromEmail, Arg.Any<object>()).Returns(Task.FromResult<User>(user));


            var result = await _sut.RegisterUser(user);


            Assert.Equal(400, result.Status);
            Assert.Equal("User already exists", result.Message);
        }
    }
}
