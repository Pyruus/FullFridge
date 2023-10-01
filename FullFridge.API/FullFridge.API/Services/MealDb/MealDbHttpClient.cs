using FullFridge.API.Models;
using RestSharp;

namespace FullFridge.API.Services.MealDb
{
    public class MealDbHttpClient: IMealDbHttpClient
    {
        private const string mealDbUrl = "https://www.themealdb.com/api/json/v1/1/";

        public async Task<List<Product>> GetProducts()
        {
            var client = new RestClient($"{mealDbUrl}list.php?i=list");
            var request = new RestRequest();
            request.AddHeader("Accept", "application/json");
            var response = await client.ExecuteAsync<ProductListResponse>( request );
            return response.Data.Products;
        }

    }

    public interface IMealDbHttpClient
    {
        Task<List<Product>> GetProducts();
    }
}
