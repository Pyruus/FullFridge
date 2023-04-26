namespace FullFridge.API.Models
{
    public class Recipe
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Likes { get; set; }
        public int? DIslikes { get; set; }
        public User? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Image { get; set; }
        public ICollection<ProductsRecipes>? ProductsRecipes { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}
