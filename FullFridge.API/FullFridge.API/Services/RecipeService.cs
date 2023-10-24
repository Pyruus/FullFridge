using FullFridge.API.Models;
using FullFridge.API.Models.MealDb;
using FullFridge.API.Services.MealDb;
using FullFridge.Model;
using FullFridge.Model.Helpers;

namespace FullFridge.API.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IDapperRepository _repository;
        private readonly IMealDbHttpClient _mealDbHttpClient;
        private readonly IProductService _productService;
        public RecipeService(IDapperRepository repository, IMealDbHttpClient mealDbHttpClient, IProductService productService)
        {
            _repository = repository;
            _mealDbHttpClient = mealDbHttpClient;
            _productService = productService;
        }

        public async Task<IEnumerable<RecipeListDTO>> GetRecipesByProductList(List<int?> productIds, bool allProducts, bool otherProducts)
        {
            var recipes = await _repository.Query<Recipe>(
                SqlQueryHelper.GetRecipes);


            List<RecipeListDTO> filteredRecipes;

            if (!allProducts && !otherProducts)
            {
                filteredRecipes = recipes.Where(recipe =>
                productIds.All(productId =>
                    recipe.Products.Any(pr => pr == productId)))
                    .Select(recipes => new RecipeListDTO
                    {
                        Id = recipes.Id,
                        Title = recipes.Title,
                        Rating = recipes.Rating,
                        Image = recipes.Image
                    })
                .ToList();
            }
            else if (otherProducts && !allProducts)
            {
                filteredRecipes = recipes.Where(recipe =>
                productIds.Any(productId =>
                    recipe.Products.Any(pr => pr == productId)))
                    .Select(recipes => new RecipeListDTO
                    {
                        Id = recipes.Id,
                        Title = recipes.Title,
                        Rating = recipes.Rating,
                        Image = recipes.Image
                    })
                .ToList();
            }
            else if (!otherProducts && allProducts)
            {
                var providedProductSet = new HashSet<int?>(productIds);
                filteredRecipes = recipes.Where(recipe =>
                    recipe.Products
                        .Select(pr => pr)
                        .ToHashSet()
                        .SetEquals(providedProductSet))
                        .Select(recipes => new RecipeListDTO
                        {
                            Id = recipes.Id,
                            Title = recipes.Title,
                            Rating = recipes.Rating,
                            Image = recipes.Image
                        })
                    .ToList();
            }
            else
            {
                var providedProductSet = new HashSet<int?>(productIds);
                filteredRecipes = recipes.Where(recipe =>
                    recipe.Products
                        .Select(pr => pr)
                        .ToHashSet()
                        .IsSubsetOf(providedProductSet))
                        .Select(recipes => new RecipeListDTO
                        {
                            Id = recipes.Id,
                            Title = recipes.Title,
                            Rating = recipes.Rating,
                            Image = recipes.Image
                        })
                    .ToList();
            }

            return filteredRecipes;
        }

        public async Task<IEnumerable<Recipe>> SearchRecipeByRegex(string regex)
        {
            var recipes = await _repository.Query<Recipe>(
                SqlQueryHelper.SearchRecipesByRegex, new { regex = string.Format("%{0}%", regex) });

            return recipes;
        }

        public async Task<IEnumerable<Recipe>> GetTopRecipes()
        {
            var recipes = await _repository.Query<Recipe>(
                SqlQueryHelper.GetTopRecipes);

            return recipes;
        }

        public async Task<Recipe> GetRecipeByID(Guid id)
        {
            var recipe = await _repository.QueryFirstOrDefault<Recipe>(
                SqlQueryHelper.GetRecipeById, new { id });

            if(recipe == null)
            {
                return recipe;
            }

            recipe.Comments = await _repository.Query<CommentDTO>(
                SqlQueryHelper.GetRecipeComments, new { recipeId = recipe.Id });

            return recipe;
        }

        public async Task<Guid> SaveRecipe(Recipe recipe)
        {
            var id = await _repository.QueryFirstOrDefault<Guid>(
                SqlQueryHelper.InsertRecipe, recipe);

            foreach (var product in recipe.Products)
            {
                await _repository.Execute(
                    SqlQueryHelper.InsertRecipeProduct, new { RecipeId = id, ProductId = product });
            }

            return id;
        }

        public async Task DeleteRecipe(Guid id)
        {
            await _repository.Execute(
                SqlQueryHelper.DeleteRecipe, new { id });
        }

        public async Task<Result> CommentRecipe(Comment comment)
        {
            var existingComment = await _repository.QueryFirstOrDefault<Guid?>(
                SqlQueryHelper.AlreadyCommented, new { comment.RecipeId, comment.CreatedById });

            if (!string.IsNullOrEmpty(existingComment.ToString()))
            {
                return new Result(StatusCodes.Status400BadRequest, "You have already commented on this recipe");
            }

            var recipeExists = await RecipeExists(comment.RecipeId);

            if (!recipeExists)
            {
                return new Result(StatusCodes.Status404NotFound, "Recipe not found");
            }

            await _repository.Execute(
                SqlQueryHelper.InsertComment, 
                new {
                    comment.Content,
                    comment.CreatedById,
                    comment.RecipeId,
                    comment.Rating
                });

            var currentRating = await _repository.QueryFirstOrDefault<double>(
                SqlQueryHelper.GetRating, new { comment.RecipeId });

            var commentCount = await _repository.QueryFirstOrDefault<int>(
                SqlQueryHelper.GetCommentCount, new { comment.RecipeId });

            var newRating = (currentRating * commentCount + comment.Rating) / (commentCount);

            await _repository.Execute(
                SqlQueryHelper.ChangeRating, new { rating = newRating, comment.RecipeId});

            return new Result(StatusCodes.Status200OK);
        }

        public async Task<Result> AssignImage(Guid id, string fileName)
        {
            var recipeId = await RecipeExists(id);
            if (!recipeId)
            {
                return new Result(StatusCodes.Status404NotFound, "No recipe with given Id");
            }

            await _repository.Execute(
                SqlQueryHelper.AssignImage, new { fileName, id });

            return new Result(StatusCodes.Status200OK);
        }

        public async Task FetchMealDbRecipes()
        {
            var categories = await _mealDbHttpClient.GetCategories();
            var recipes = await _mealDbHttpClient.GetRecipesFromCategories(categories);
            var products = await _productService.GetProducts();

            foreach(var recipe in recipes)
            {
                var details = await _mealDbHttpClient.GetRecipeDetails(recipe.Id);
                var newRecipe = new Recipe(details, products);

                var id = await _repository.QueryFirstOrDefault<Guid>(
                    SqlQueryHelper.InsertMealDbRecipe, new
                    {
                        Title = newRecipe.Title,
                        Description = newRecipe.Description,
                        Image = newRecipe.Image,
                        MealDbId = newRecipe.MealDbId
                    });

                await InsertProducts(newRecipe.Products, id);
            }
        }

        #region private methods
        private async Task<bool> RecipeExists(Guid id)
        {
            var recipeId = await _repository.QueryFirstOrDefault<Guid?>(
                SqlQueryHelper.GetRecipeId, new
                {
                    id
                });
            return recipeId != null;
        }

        private async Task InsertProducts(List<int?> products, Guid id)
        {
            foreach(var product in products)
            {
                if (product != null)
                {
                    await _repository.Execute(
                        SqlQueryHelper.InsertRecipeProduct, new
                        {
                            RecipeId = id,
                            ProductId = product
                        });
                }
            }
        }
        #endregion
    }

    public interface IRecipeService
    {
        Task<IEnumerable<RecipeListDTO>> GetRecipesByProductList(List<int?> productIds, bool allProducts, bool otherProducts);
        Task<IEnumerable<Recipe>> SearchRecipeByRegex(string regex);
        Task<IEnumerable<Recipe>> GetTopRecipes();
        Task<Recipe> GetRecipeByID(Guid id);
        Task<Guid> SaveRecipe(Recipe recipe);
        Task DeleteRecipe(Guid id);
        Task<Result> CommentRecipe(Comment comment);
        Task<Result> AssignImage(Guid id, string fileName);
        Task FetchMealDbRecipes();
    }
}
