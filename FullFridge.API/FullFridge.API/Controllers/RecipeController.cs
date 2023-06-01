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
        [Authorize]
        public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //DELETE: api/Recipe/{id}
        [HttpDelete("{id}")]
        [Authorize]
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
        [Authorize]
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

        //GET: api/Recipe/Top
        [HttpGet("Top")]
        public async Task<ActionResult<IEnumerable<RecipeListDTO>>> GetTopRecipes()
        {
            return await _recipeService.GetTopRecipes();
        }

        //GET: api/Recipe/Search
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<RecipeListDTO>>> SeacrhRecipes(string searchString)
        {
            return await _recipeService.SearchRecipeByRegex(searchString);
        }

        //POST: api/Recipe/Comment
        [HttpPost("Comment")]
        [Authorize]
        public async Task<ActionResult> PostComment(Comment comment)
        {
            var existingUser = await _context.Users.FindAsync(comment.CreatedById);
            if (existingUser == null)
            {
                return BadRequest("User does not exist");
            }

            var existingComment = await _context.Comments.Where(c => c.CreatedById == comment.CreatedById).Where(c => c.RecipeId == comment.RecipeId).ToListAsync();
            if (existingComment.Count != 0)
            {
                return BadRequest("You have already commented on this recipe");
            }

            var commentedRecipe = await _context.Recipes.SingleOrDefaultAsync(recipe => recipe.Id == comment.RecipeId);
            if (commentedRecipe == null)
            {
                return NotFound("Recipe doesnt exist");
            }

            if (comment.IsLike)
            {
                commentedRecipe.Likes++;
            }
            else
            {
                commentedRecipe.Dislikes++;
            }

            _context.Update(commentedRecipe);
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return Ok();
        }

        //DELETE: api/Recipe/Comment/{id}
        [HttpDelete("Comment/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //POST: api/Recipe/File/{recipeId}
        [HttpPost("File/{recipeId}")]
        //[Authorize]
        public async Task<IActionResult> Upload(int recipeId, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                var fileName = Path.GetFileName(Guid.NewGuid().ToString() + "_" + file.FileName);

                var filePath = Path.Combine(uploadDir, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                var recipe = await _context.Recipes.SingleOrDefaultAsync(r => r.Id == recipeId);
                if (recipe == null)
                {
                    return NotFound("Recipe not found");
                }
                recipe.Image = filePath;

                await _context.SaveChangesAsync();
                return Ok(new { filePath });
            }

            return BadRequest("No file was uploaded.");
        }

    }
}
