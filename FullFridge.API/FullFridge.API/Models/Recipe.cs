namespace FullFridge.API.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Likes { get; set; } = 0;
        public int? Dislikes { get; set; } = 0;
        public User? CreatedBy { get; set; }
        public int? CreatedById { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Image { get; set; }
        public ICollection<ProductsRecipes>? ProductsRecipes { get; set; }
        public List<Comment>? Comments { get; set; }
    }

    public class RecipeListDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Ratio { get; set; }
        public string? Image { get; set; }
    }

    public class RecipeDetailsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Ratio { get; set; }
        public string? Image { get; set; }
        public List<Comment>? Comments { get; set; }

    }
}
