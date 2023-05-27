using FullFridge.API.Context;
using FullFridge.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FullFridge.API.Services
{
    public class RecipeService: IRecipeService
    {
        private readonly FullFridgeContext _context;
        public RecipeService(FullFridgeContext context)
        {
            _context = context;
        }

        public bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }

        public async Task<List<Recipe>> GetRecipesByProductList(List<int> productIds)
        {
            var recipes = await _context.Recipes
        .Include(r => r.ProductsRecipes)
        .ToListAsync();

            _context.ChangeTracker.LazyLoadingEnabled = false;

            var filteredRecipes = recipes.Where(recipe =>
                productIds.All(productId =>
                    recipe.ProductsRecipes.Any(pr => pr.ProductId == productId)))
                .ToList();

            _context.ChangeTracker.LazyLoadingEnabled = true;

            return filteredRecipes;
        }

        public async Task<List<Recipe>> SearchRecipeByRegex(string regex)
        {
            var recipes = await _context.Recipes.ToListAsync();
            var searchResults = recipes.Where(recipe => Regex.IsMatch(recipe.Title, regex)).ToList();

            return searchResults;
        }

        public async Task<List<Recipe>> GetTopRecipes()
        {
            var recipes = await _context.Recipes.OrderByDescending( recipe => recipe.Likes - recipe.Dislikes).Take(12).ToListAsync();

            return recipes;
        }
    }

    public interface IRecipeService
    {
        bool RecipeExists(int id);
        Task<List<Recipe>> GetRecipesByProductList(List<int> productIds);
        Task<List<Recipe>> SearchRecipeByRegex(string regex);
        Task<List<Recipe>> GetTopRecipes();
    }
}
