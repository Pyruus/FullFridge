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

        public async Task<List<RecipeListDTO>> GetRecipesByProductList(List<int> productIds)
        {
            var recipes = await _context.Recipes
            .OrderByDescending(recipe => recipe.Likes - recipe.Dislikes)
            .Include(r => r.ProductsRecipes)
            .ToListAsync();

            _context.ChangeTracker.LazyLoadingEnabled = false;

            var filteredRecipes = recipes.Where(recipe =>
                productIds.All(productId =>
                    recipe.ProductsRecipes.Any(pr => pr.ProductId == productId)))
                    .Select(recipes => new RecipeListDTO
                    {
                        Id = recipes.Id,
                        Title = recipes.Title,
                        Ratio = recipes.Likes - recipes.Dislikes,
                        Likes = recipes.Likes,
                        Dislikes = recipes.Dislikes,
                        Image = recipes.Image
                    })
                .ToList();

            _context.ChangeTracker.LazyLoadingEnabled = true;

            return filteredRecipes;
        }

        public async Task<List<RecipeListDTO>> SearchRecipeByRegex(string regex)
        {
            var recipes = await _context.Recipes.ToListAsync();
            var searchResults = recipes.Where(recipe => Regex.IsMatch(recipe.Title.ToLower(), regex.ToLower())).Select(recipes => new RecipeListDTO
            {
                Id = recipes.Id,
                Title = recipes.Title,
                Ratio = recipes.Likes - recipes.Dislikes,
                Likes = recipes.Likes,
                Dislikes = recipes.Dislikes,
                Image = recipes.Image
            }).ToList();

            return searchResults;
        }

        public async Task<List<RecipeListDTO>> GetTopRecipes()
        {
            var recipes = await _context.Recipes.OrderByDescending( recipe => recipe.Likes - recipe.Dislikes).Take(12).Select(recipes => new RecipeListDTO
            {
                Id = recipes.Id,
                Title = recipes.Title,
                Ratio = recipes.Likes - recipes.Dislikes,
                Likes = recipes.Likes,
                Dislikes = recipes.Dislikes,
                Image = recipes.Image
            }).ToListAsync();

            return recipes;
        }
    }

    public interface IRecipeService
    {
        bool RecipeExists(int id);
        Task<List<RecipeListDTO>> GetRecipesByProductList(List<int> productIds);
        Task<List<RecipeListDTO>> SearchRecipeByRegex(string regex);
        Task<List<RecipeListDTO>> GetTopRecipes();
    }
}
