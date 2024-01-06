using AutoFixture;
using FluentAssertions;
using FullFridge.API.Models;
using FullFridge.API.Services;
using FullFridge.API.Services.MealDb;
using FullFridge.Model;
using FullFridge.Model.Helpers;
using Microsoft.OpenApi.Any;
using NSubstitute;
using NSubstitute.Core.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullFridge.Test
{
    public class RecipeServiceTests
    {
        private readonly RecipeService _sut;
        private readonly IDapperRepository _repository = Substitute.For<IDapperRepository>();
        private readonly IMealDbHttpClient _mealDbHttpClient = Substitute.For<IMealDbHttpClient>();
        private readonly IProductService _productService = Substitute.For<IProductService>();
        private readonly IFixture _fixture = new Fixture();

        private readonly List<Guid> recipeIds = new()
        {
            new Guid("00000000-0000-0000-0000-000000000000"),
            new Guid("10000000-0000-0000-0000-000000000000"),
            new Guid("20000000-0000-0000-0000-000000000000"),
            new Guid("30000000-0000-0000-0000-000000000000"),
            new Guid("40000000-0000-0000-0000-000000000000"),
            new Guid("50000000-0000-0000-0000-000000000000"),
            new Guid("60000000-0000-0000-0000-000000000000")
        };

        private readonly List<string> recipeNames = new()
        {
            "Spicy Mango Chicken Stir-Fry",
            "Quinoa and Black Bean Fiesta Salad",
            "Lemon Garlic Shrimp Skewers with Herb Quinoa",
            "Creamy Mushroom and Spinach Stuffed Chicken",
            "Thai Coconut Curry Noodle Soup",
            "Mediterranean Chickpea and Vegetable Skillet",
            "Blueberry Almond Oatmeal Breakfast Bake"
        };

        public RecipeServiceTests() 
        {
            _sut = new RecipeService(_repository, _mealDbHttpClient, _productService);
        }

        [Fact]
        public async void SaveRecipe_ShouldSaveRecipe_WhenCorrectProvided()
        {
            var products = new List<int?> { 1, 2, 3 };

            var recipe = _fixture.Build<Recipe>()
                .With(r => r.Products, products)
                .Create();

            _repository.QueryFirstOrDefault<Guid>(Arg.Any<string>()).Returns(Task.FromResult(recipeIds[0]));


            var result = await _sut.SaveRecipe(recipe);


            Assert.Equal(recipeIds[0], result);
        }

        [Theory]
        [InlineData(false, false, 1, 3, 7)]
        [InlineData(true, false, 1, 1, 1)]
        [InlineData(false, true, 4, 6, 7)]
        [InlineData(true, true, 4, 2, 1)]
        public async void GetRecipesByProductId_ShouldReturnCorrectRecipes_WhenDifferentParametersProvided(bool allProducts, bool otherProducst, int oneResultCount, int twoResultCount, int threeResultCount)
        {
            var recipes = GetRecipes();
            _repository.Query<Recipe>(Arg.Any<string>()).Returns(Task.FromResult(recipes));

            var products = new List<List<int?>>
            {
                new List<int?>{1},
                new List<int?>{1, 2},
                new List<int?>{1,2, 3}
            };


            var result1 = await _sut.GetRecipesByProductList(products[0], allProducts, otherProducst);
            var result2 = await _sut.GetRecipesByProductList(products[1], allProducts, otherProducst);
            var result3 = await _sut.GetRecipesByProductList(products[2], allProducts, otherProducst);


            Assert.Equal(oneResultCount, result1.Count());
            Assert.Equal(twoResultCount, result2.Count());
            Assert.Equal(threeResultCount, result3.Count());
        }

        [Fact]
        public async void GetRecipeByID_ShouldReturnCorrectRecipe_WhenCorrectRecipeIdProvided()
        {
            var recipe = GetRecipes().ToList()[0];
            _repository.QueryFirstOrDefault<Recipe>(Arg.Any<string>(), Arg.Any<object>()).Returns(Task.FromResult(recipe));

            IEnumerable<CommentDTO> comments;
            comments = new List<CommentDTO>()
            {
                _fixture.Build<CommentDTO>().Create()
            };
            _repository.Query<CommentDTO>(Arg.Any<string>(), Arg.Any<object>()).Returns(Task.FromResult(comments));


            var result = await _sut.GetRecipeByID(recipeIds[0]);


            Assert.Equal(recipeIds[0], result.Id);
            Assert.Equal(recipeNames[0], result.Title);
            Assert.Equal(comments, result.Comments);
        }

        [Fact]
        public async void GetRecipeByID_ShouldReturnNull_WhenIncorectRecipeIdProvided()
        {
            _repository.QueryFirstOrDefault<Recipe>(Arg.Any<string>(), Arg.Any<object>()).Returns(Task.FromResult<Recipe>(null));
            

            var result = await _sut.GetRecipeByID(recipeIds[0]);


            Assert.Null(result);
        }

        [Fact] 
        public async void CommentRecipe_ShouldSaveComment_WhenCorrectProvided()
        {
            _repository.QueryFirstOrDefault<Guid?>(SqlQueryHelper.AlreadyCommented, Arg.Any<object>()).Returns(Task.FromResult<Guid?>(null));
            _repository.QueryFirstOrDefault<Guid?>(SqlQueryHelper.GetRecipeId, Arg.Any<object>()).Returns(Task.FromResult<Guid?>(Guid.NewGuid()));
            _repository.QueryFirstOrDefault<double>(SqlQueryHelper.GetRating, Arg.Any<object>()).Returns(Task.FromResult(5.0));
            _repository.QueryFirstOrDefault<int>(SqlQueryHelper.GetCommentCount, Arg.Any<object>()).Returns(Task.FromResult(2));

            var comment = _fixture.Build<Comment>().Create();


            var result = await _sut.CommentRecipe(comment);


            Assert.Equal(200, result.Status);
        }

        [Fact]
        public async void CommentRecipe_ShouldReturnNotFound_WhenRecipeNotExisting()
        {
            _repository.QueryFirstOrDefault<Guid?>(SqlQueryHelper.AlreadyCommented, Arg.Any<object>()).Returns(Task.FromResult<Guid?>(null));
            _repository.QueryFirstOrDefault<Guid?>(SqlQueryHelper.GetRecipeId, Arg.Any<object>()).Returns(Task.FromResult<Guid?>(null));

            var comment = _fixture.Build<Comment>().Create();


            var result = await _sut.CommentRecipe(comment);


            Assert.Equal(404, result.Status);
            Assert.Equal("Recipe not found", result.Message);
        }

        [Fact]
        public async void CommentRecipe_ShouldReturnBadRequest_WhenRecipeAlreadyCommented()
        {
            _repository.QueryFirstOrDefault<Guid?>(SqlQueryHelper.AlreadyCommented, Arg.Any<object>()).Returns(Task.FromResult<Guid?>(Guid.NewGuid()));

            var comment = _fixture.Build<Comment>().Create();


            var result = await _sut.CommentRecipe(comment);


            Assert.Equal(400, result.Status);
            Assert.Equal("You have already commented on this recipe", result.Message);
        }

        #region Private Methods
        private IEnumerable<Recipe> GetRecipes()
        {
            return new List<Recipe>()
            {
                _fixture.Build<Recipe>()
                    .With(r => r.Products, new List<int?> { 1 })
                    .With(r => r.Id, recipeIds[0])
                    .With(r => r.Title, recipeNames[0])
                    .Create(),
                _fixture.Build<Recipe>()
                    .With(r => r.Products, new List<int?> { 2 })
                    .With(r => r.Id, recipeIds[1])
                    .With(r => r.Title, recipeNames[1])
                    .Create(),
                _fixture.Build<Recipe>()
                    .With(r => r.Products, new List<int?> { 3 })
                    .With(r => r.Id, recipeIds[2])
                    .With(r => r.Title, recipeNames[2])
                    .Create(),
                _fixture.Build<Recipe>()
                    .With(r => r.Products, new List<int?> { 1, 2 })
                    .With(r => r.Id, recipeIds[3])
                    .With(r => r.Title, recipeNames[3])
                    .Create(),
                _fixture.Build<Recipe>()
                    .With(r => r.Products, new List<int?> { 2, 3 })
                    .With(r => r.Id, recipeIds[4])
                    .With(r => r.Title, recipeNames[4])
                    .Create(),
                _fixture.Build<Recipe>()
                    .With(r => r.Products, new List<int?> { 1, 3 })
                    .With(r => r.Id, recipeIds[5])
                    .With(r => r.Title, recipeNames[5])
                    .Create(),
                _fixture.Build<Recipe>()
                    .With(r => r.Products, new List<int?> { 1, 2, 3 })
                    .With(r => r.Id, recipeIds[6])
                    .With(r => r.Title, recipeNames[6])
                    .Create()
            };
        }
        #endregion
    }
}
