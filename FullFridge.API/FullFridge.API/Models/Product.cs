using System.Text.Json.Serialization;

namespace FullFridge.API.Models
{
    public class Product
    {
        [JsonPropertyName("idIngredient")]
        public int? Id { get; set; }
        [JsonPropertyName("strIngredient")]
        public string? Name { get; set; }
        [JsonPropertyName("strDescription")]
        public string? Description { get; set; }
    }

    public class ProductListResponse
    {
        [JsonPropertyName("meals")]
        public List<Product> Products { get;set; }
    }
}
