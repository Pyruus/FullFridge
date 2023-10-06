using System.Text.Json.Serialization;

namespace FullFridge.API.Models.MealDb
{
    public class MealDbList<T>
        where T : class
    {
        [JsonPropertyName("meals")]
        public List<T> Values { get; set; }
    }
}
