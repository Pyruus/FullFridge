namespace FullFridge.API.Models
{
    public class User
    {
        public int? Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserDetails Details { get; set; }
    }

    public class UserDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
