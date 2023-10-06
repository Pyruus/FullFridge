using System.Text.Json.Serialization;

namespace FullFridge.API.Models.MealDb
{
    public class MealDbRecipeCategory
    {
        [JsonPropertyName("strMeal")]
        public string Name { get; set; }
        [JsonPropertyName("strMealThumb")]
        public string Thumb { get; set; }
        [JsonPropertyName("idMeal")]
        public string Id { get; set; }
    }
}
