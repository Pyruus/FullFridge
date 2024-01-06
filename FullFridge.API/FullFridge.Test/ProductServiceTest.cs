using AutoFixture;
using FullFridge.API.Models;
using FullFridge.API.Services;
using FullFridge.API.Services.MealDb;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullFridge.Test
{
    public class ProductServiceTest
    {
        private readonly ProductService _sut;
        private readonly IMealDbHttpClient _httpClient = Substitute.For<IMealDbHttpClient>();
        private readonly IFixture _fixture = new Fixture();

        private readonly List<int> productIds = new()
        {
            0,
            1,
            2,
            3,
            4
        };

        private readonly List<string> productNames = new()
        {
            "Chicken Breast",
            "Mango",
            "Bell Pepper",
            "Soy Sauce",
            "Chicken Broth"
        };

        public ProductServiceTest()
        {
            _sut = new ProductService(_httpClient);
        }

        [Theory]
        [InlineData("bell", 2)]
        [InlineData("none")]
        [InlineData("chicken", 0, 4)]

        public async void SearchProductByRegex_ShouldReturnCorrectProducts_WhenRegexProvided(string regex, params int[] expectedIds)
        {
            var products = GetProducts();
            _httpClient.GetProducts().Returns(products);

            var expectedProducts = products.Where(p => expectedIds.ToList().Contains(p.Id.Value)).ToList();


            var result = await _sut.SearchProductByRegex(regex);


            Assert.Equal(expectedProducts, result);
        }

        [Fact]
        public async void GetProductById_ShouldReturnCorrectProduct_WhenCorrectIdProvided()
        {
            var products = GetProducts();
            _httpClient.GetProducts().Returns(products);


            var result1 = await _sut.GetProductById(productIds[0]);
            var result2 = await _sut.GetProductById(productIds[3]);


            Assert.Equal(products[0], result1);
            Assert.Equal(products[3], result2);
        }

        [Fact]
        public async void GetProductById_ShouldReturnNull_WhenIncorrectIdProvided()
        {
            var products = GetProducts();
            _httpClient.GetProducts().Returns(products);


            var result1 = await _sut.GetProductById(6);
            var result2 = await _sut.GetProductById(-1);


            Assert.Null(result1);
            Assert.Null(result2);
        }

        #region Private Methods
        private List<Product> GetProducts()
        {
            return new List<Product>()
            {
                _fixture.Build<Product>()
                    .With(r => r.Id, productIds[0])
                    .With(r => r.Name, productNames[0])
                    .Create(),
                _fixture.Build<Product>()
                    .With(r => r.Id, productIds[1])
                    .With(r => r.Name, productNames[1])
                    .Create(),
                _fixture.Build<Product>()
                    .With(r => r.Id, productIds[2])
                    .With(r => r.Name, productNames[2])
                    .Create(),
                _fixture.Build<Product>()
                    .With(r => r.Id, productIds[3])
                    .With(r => r.Name, productNames[3])
                    .Create(),
                _fixture.Build<Product>()
                    .With(r => r.Id, productIds[4])
                    .With(r => r.Name, productNames[4])
                    .Create()
            };
        }
        #endregion
    }
}
