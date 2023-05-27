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

        public async Task<List<Product>> SearchProductByRegex(string regex)
        {
            var products = await _context.Products.ToListAsync();
            var searchResults = products.Where(product => Regex.IsMatch(product.Name, regex)).Take(10).ToList();

            return searchResults;
        }
    }

    public interface IProductService
    {
        Task<List<Product>> SearchProductByRegex(string regex);
    }
}
