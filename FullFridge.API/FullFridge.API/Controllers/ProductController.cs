using FullFridge.API.Context;
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
        private readonly FullFridgeContext _context;
        private readonly IProductService _productService;
        public ProductController(FullFridgeContext context, IProductService productService)
        {
            _context= context;
            _productService = productService;
        }

        //GET: api/Product
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        //GET: api/Product/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(r => r.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        //POST: api/Product
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            //product.CreatedAt = DateTime.Now;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //GET: api/Product/Search
        [HttpGet("Search")]
        [Authorize]
        public async Task<ActionResult<List<ProductDTO>>> SearchProducts(string searchString)
        {
            var searchResult = await _productService.SearchProductByRegex(searchString);

            return Ok(searchResult);
        }
    }
}
