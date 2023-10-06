using System.Text.Json.Serialization;

namespace FullFridge.API.Models.MealDb
{
    public class Categories
    {
        [JsonPropertyName("strCategory")]
        public string Category { get; set; }
    }
}
