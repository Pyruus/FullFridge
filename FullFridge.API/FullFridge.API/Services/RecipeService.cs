using FullFridge.API.Context;
using FullFridge.API.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
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

        public async Task<List<RecipeListDTO>> GetRecipesByProductList(List<int?> productIds, bool allProducts, bool otherProducts)
        {
            var recipes = await _context.Recipes
            .OrderByDescending(recipe => recipe.Likes - recipe.Dislikes)
            .Include(r => r.ProductsRecipes)
            .ToListAsync();

            _context.ChangeTracker.LazyLoadingEnabled = false;

            List<RecipeListDTO> filteredRecipes;

            if(!allProducts && !otherProducts)
            {
                filteredRecipes = recipes.Where(recipe =>
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
            }
            else if(otherProducts && !allProducts)
            {
                filteredRecipes = recipes.Where(recipe =>
                productIds.Any(productId =>
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
            }
            else if(!otherProducts && allProducts)
            {
                var providedProductSet = new HashSet<int?>(productIds);
                filteredRecipes = recipes.Where(recipe =>
                    recipe.ProductsRecipes
                        .Select(pr => pr.ProductId)
                        .ToHashSet()
                        .SetEquals(providedProductSet))
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
            }
            else
            {
                var providedProductSet = new HashSet<int?>(productIds);
                filteredRecipes = recipes.Where(recipe =>
                    recipe.ProductsRecipes
                        .Select(pr => pr.ProductId)
                        .ToHashSet()
                        .IsSubsetOf(providedProductSet))
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
            }            

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
        Task<List<RecipeListDTO>> GetRecipesByProductList(List<int?> productIds, bool allProducts, bool otherProducts);
        Task<List<RecipeListDTO>> SearchRecipeByRegex(string regex);
        Task<List<RecipeListDTO>> GetTopRecipes();
    }
}
