using FullFridge.API.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FullFridge.API.Models;
using Microsoft.EntityFrameworkCore;
using FullFridge.API.Services;
using System.Threading.Tasks;

namespace FullFridge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly FullFridgeContext _context;
        private readonly IRecipeService _recipeService;
        public RecipeController(FullFridgeContext context, IRecipeService recipeService)
        {
            _context= context;
            _recipeService = recipeService;
        }

        //GET: api/Recipe
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            return await _context.Recipes.ToListAsync();
        }

        //GET: api/Recipe/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipeById(int id)
        {
            var recipe = await _context.Recipes.SingleOrDefaultAsync(r => r.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }
            return Ok(recipe);
        }

        //POST: api/Recipe
        [HttpPost]
        public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
        {
            //recipe.CreatedAt = DateTime.Now;

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //DELETE: api/Recipe/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if(recipe == null)
            {
                return NotFound();
            }
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //PUT: api/Recipe/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutRecipe(int id, Recipe recipe)
        {
            recipe.Id = id;

            _context.Entry(recipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_recipeService.RecipeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //GET: api/Recipe/Products
        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipesByProducts([FromQuery] List<int> productIds, bool allProducts, bool otherProducts)
        {
            var result = await _recipeService.GetRecipesByProductList(productIds);

            return Ok(result);
        }
    }
}
