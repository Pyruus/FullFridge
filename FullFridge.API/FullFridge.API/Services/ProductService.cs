using FullFridge.API.Context;
using FullFridge.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace FullFridge.API.Services
{
    public class ProductService: IProductService
    {
        private readonly FullFridgeContext _context;

        public ProductService(FullFridgeContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDTO>> SearchProductByRegex(string regex)
        {
            var products = await _context.Products.ToListAsync();
            var searchResults = products.Where(product => Regex.IsMatch(product.Name.ToLower(), regex.ToLower())).Select(products => new ProductDTO { Id = products.Id, Name = products.Name, Calories = products.Calories}).ToList();

            return searchResults;
        }
    }

    public interface IProductService
    {
        Task<List<ProductDTO>> SearchProductByRegex(string regex);
    }
}
