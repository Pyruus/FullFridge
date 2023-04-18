namespace FullFridge.API.Models
{
    public class ProductsRecipes
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
