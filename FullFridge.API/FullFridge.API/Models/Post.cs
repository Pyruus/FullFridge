namespace FullFridge.API.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? RecipeId { get; set; }
    }

    public class PostDTO
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public UserDTO? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? RecipeId { get; set; }
        public PostDTO(Post post, UserDTO? user)
        {
            Id = post.Id;
            Title = post.Title;
            Content = post.Content;
            CreatedBy = user;
            CreatedAt = post.CreatedAt;
            RecipeId = post.RecipeId;

        }
    }

    public class PostList
    {
        public List<Post>? Posts { get; set; }
        public int Pages { get; set; }
    }

    public class PostComment
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string? Content { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class PostCommentDTO
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
