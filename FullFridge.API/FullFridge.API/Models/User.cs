﻿namespace FullFridge.API.Models
{
    public class User
    {
        public Guid? Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string Role { get; set; } = "user";
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }

    public class UserDTO
    {
        public Guid? Id { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Token { get; set; }
        public string Role { get; set; }
    }
}
