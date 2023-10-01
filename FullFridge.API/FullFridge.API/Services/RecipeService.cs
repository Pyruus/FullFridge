using FullFridge.API.Models;
using FullFridge.Model;
using FullFridge.Model.Helpers;

namespace FullFridge.API.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IDapperRepository _repository;
        public RecipeService(IDapperRepository repository)
        {
            _repository = repository;
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
                SqlQueryHelper.SearchRecipesByRegex, new { regex });

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

            recipe.Comments = await _repository.Query<CommentDTO>(
                SqlQueryHelper.GetRecipeComments, new { recipeId = recipe.Id });

            return recipe;
        }

        public async Task<Guid> SaveRecipe(Recipe recipe)
        {
            var id = await _repository.QueryFirstOrDefault<Guid>(
                SqlQueryHelper.InsertRecipe, recipe);

            return id;
        }

        public async Task DeleteRecipe(Guid id)
        {
            await _repository.Execute(
                SqlQueryHelper.DeleteRecipe, new { id });
        }

        public async Task<Result> CommentRecipe(Comment comment)
        {
            var existingComment = await _repository.QueryFirstOrDefault<Guid>(
                SqlQueryHelper.AlreadyCommented, new { comment.RecipeId, comment.CreatedById });

            if (string.IsNullOrEmpty(existingComment.ToString()))
            {
                return new Result(StatusCodes.Status400BadRequest, "You have already commented on this recipe");
            }

            var recipeExists = await RecipeExists(comment.RecipeId);

            if (!recipeExists)
            {
                return new Result(StatusCodes.Status404NotFound, "Recipe not found");
            }

            await _repository.Execute(
                SqlQueryHelper.InsertComment, new { comment });

            var currentRating = await _repository.QueryFirstOrDefault<double>(
                SqlQueryHelper.GetRating, new { comment.RecipeId });

            var commentCount = await _repository.QueryFirstOrDefault<int>(
                SqlQueryHelper.GetCommentCount, new { comment.RecipeId });

            var newRating = (currentRating * commentCount + comment.Rating) / (commentCount + 1);

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
    }
}
