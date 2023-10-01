namespace FullFridge.API.Models
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Image { get; set; }
        public List<int?>? Products { get; set; }
        public double? Rating { get; set; }
        public IEnumerable<CommentDTO> Comments { get; set; }
    }

    public class RecipeListDTO
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public double? Rating { get; set; }
        public string? Image { get; set; }
    }

    public class RecipeDetailsDTO
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public double? Rating { get; set; }
        public string? Image { get; set; }

    }
}
