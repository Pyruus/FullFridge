namespace FullFridge.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Calories { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
