using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FullFridge.API.Models;
using Microsoft.EntityFrameworkCore;
using FullFridge.API.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            //TODO after productService refactor
            return new List<Product>();
        }

        //GET: api/Product/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            //TODO after productService refactor
            return Ok();
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
