using FullFridge.API.Models;
using FullFridge.API.Models.MealDb;
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
            var response = await client.ExecuteAsync<MealDbList<Product>>( request );
            return response.Data.Values;
        }

        public async Task<List<Categories>> GetCategories()
        {
            var client = new RestClient($"{mealDbUrl}list.php?c=list");
            var request = new RestRequest();
            request.AddHeader("Accept", "application/json");
            var response = await client.ExecuteAsync<MealDbList<Categories>>(request);
            return response.Data.Values;
        }

        public async Task<List<MealDbRecipeCategory>> GetRecipesFromCategories(List<Categories> categories)
        {
            var recipes = new List<MealDbRecipeCategory>();

            var request = new RestRequest();
            request.AddHeader("Accept", "application/json");
            foreach(var category in categories)
            {
                var client = new RestClient($"{mealDbUrl}filter.php?c={category.Category}");
                var response = await client.ExecuteAsync<MealDbList<MealDbRecipeCategory>>(request);
                recipes.AddRange(response.Data.Values);
            }

            return recipes;
        }
    }

    public interface IMealDbHttpClient
    {
        Task<List<Product>> GetProducts();
        Task<List<Categories>> GetCategories();
        Task<List<MealDbRecipeCategory>> GetRecipesFromCategories(List<Categories> categories);
    }
}
