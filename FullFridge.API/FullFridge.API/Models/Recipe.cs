using FullFridge.API.Models.MealDb;
using System.Reflection;

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
        public IEnumerable<CommentDTO>? Comments { get; set; }
        public int? MealDbId { get; set; }

        public Recipe() { }
        
        public Recipe(MealDbRecipeDetails recipeDetails, List<Product> products)
        {
            Id = Guid.NewGuid();
            Title = recipeDetails.Name;
            Description = recipeDetails.Description;
            Image = recipeDetails.Image;
            MealDbId = int.TryParse(recipeDetails.Id, out var mealDbId) ? mealDbId : null;

            var productsList = ExtractProducts(recipeDetails);
            this.Products = new List<int?>();

            foreach (var product in productsList)
            {
                var productId = products.FirstOrDefault(p => p.Name.ToLower() == product.ToLower())?.Id;

                if (productId != null)
                {
                    this.Products.Add(productId);
                }
            }
        }

        private List<string> ExtractProducts(MealDbRecipeDetails recipeDetails)
        {
            List<string> ingredientsList = new List<string>();
            Type recipeType = typeof(MealDbRecipeDetails);
            PropertyInfo[] properties = recipeType.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.Name.StartsWith("Ingredient") && property.PropertyType == typeof(string))
                {
                    string ingredient = (string)property.GetValue(recipeDetails);
                    if (!string.IsNullOrEmpty(ingredient))
                    {
                        ingredientsList.Add(ingredient);
                    }
                }
            }

            return ingredientsList;
        }
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
