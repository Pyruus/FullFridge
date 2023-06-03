namespace FullFridge.API.Models
{
    public class User
    {
        public int? Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public UserDetails? Details { get; set; }
        public string Role { get; set; }
    }

    public class UserDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class UserDTO
    {
        public int? Id { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Token { get; set; }
        public string Role { get; set; }
    }
}
