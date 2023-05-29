namespace FullFridge.API.Models
{
    public class Comment
    {
        public int? Id { get; set; }
        public string Content { get; set; }
        public bool IsLike { get; set; }
        public User? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? RecipeId { get; set; }
    }
}
