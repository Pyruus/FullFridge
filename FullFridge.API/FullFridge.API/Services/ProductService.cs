﻿using FullFridge.API.Models;
using FullFridge.API.Services.MealDb;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace FullFridge.API.Services
{
    public class ProductService: IProductService
    {
        private readonly IMealDbHttpClient _mealDbHttpClient;

        public ProductService(IMealDbHttpClient mealDbHttpClient)
        {
            _mealDbHttpClient = mealDbHttpClient;
        }

        public async Task<List<Product>> SearchProductByRegex(string regex)
        {
            var products = await _mealDbHttpClient.GetProducts();
            var searchResults = products.Where(product => Regex.IsMatch(product.Name.ToLower(), regex.ToLower())).ToList();

            return searchResults;
        }
    }

    public interface IProductService
    {
        Task<List<Product>> SearchProductByRegex(string regex);
    }
}
