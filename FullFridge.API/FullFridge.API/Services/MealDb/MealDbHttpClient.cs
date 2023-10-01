using FullFridge.API.Models;

namespace FullFridge.API.Services.MealDb
{
    public class MealDbHttpClient: IMealDbHttpClient
    {
        private const string mealDbUrl = "www.themealdb.com/api/json/v1/1/";

        public async Task<List<Product>> GetProducts()
        {
            return new List<Product>();
        }

    }

    public interface IMealDbHttpClient
    {
        Task<List<Product>> GetProducts();
    }
}
