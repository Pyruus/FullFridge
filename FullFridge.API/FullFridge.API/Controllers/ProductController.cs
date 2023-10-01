using Microsoft.AspNetCore.Mvc;
using FullFridge.API.Models;
using FullFridge.API.Services;

namespace FullFridge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        //GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productService.GetProducts();

            return products;
        }

        //GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);

            return product;
        }

        //GET: api/Product/Search
        [HttpGet("Search")]
        public async Task<ActionResult<List<Product>>> SearchProducts(string searchString)
        {
            var searchResult = await _productService.SearchProductByRegex(searchString);

            return Ok(searchResult);
        }
    }
}
