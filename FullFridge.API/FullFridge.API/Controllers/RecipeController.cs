using Microsoft.AspNetCore.Mvc;
using FullFridge.API.Models;
using FullFridge.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace FullFridge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        //GET: api/Recipe/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipeById(Guid id)
        {
            var recipe = _recipeService.GetRecipeByID(id);

            if(recipe == null)
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
            var newId = _recipeService.SaveRecipe(recipe);

            if(newId == null)
            {
                return BadRequest();
            }

            return Ok(newId);
        }

        //DELETE: api/Recipe/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteRecipe(Guid id)
        {
            await _recipeService.DeleteRecipe(id);

            return Ok();
        }

        //GET: api/Recipe/Products
        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipesByProducts([FromQuery] List<int?> productIds, bool allProducts, bool otherProducts)
        {
            var result = await _recipeService.GetRecipesByProductList(productIds, allProducts, otherProducts);

            return Ok(result);
        }

        //GET: api/Recipe/Top
        [HttpGet("Top")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetTopRecipes()
        {
            return Ok(await _recipeService.GetTopRecipes());
        }

        //GET: api/Recipe/Search
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<Recipe>>> SeacrhRecipes(string searchString)
        {
            return Ok(await _recipeService.SearchRecipeByRegex(searchString));
        }

        //POST: api/Recipe/Comment
        [HttpPost("Comment")]
        [Authorize]
        public async Task<ActionResult> PostComment(Comment comment)
        {
            var result = await _recipeService.CommentRecipe(comment);

            return StatusCode(result.Status, result.Message);
        }

        //POST: api/Recipe/File/{recipeId}
        [HttpPost("File/{recipeId}")]
        //[Authorize]
        public async Task<IActionResult> Upload(Guid recipeId, IFormFile file)
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

                var result = await _recipeService.AssignImage(recipeId, fileName);
                return Ok(new { fileName });
            }

            return BadRequest("No file was uploaded.");
        }

        [HttpGet("File/{fileName}")]
        public async Task<IActionResult> GetFile(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            string fileExtension = Path.GetExtension(filePath);

            // Set the content-disposition header with the file name and extension
            string contentDisposition = $"attachment; filename=\"{fileName}\"";
            Response.Headers.Add("Content-Disposition", contentDisposition);

            // Read the file content
            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

            // Return the file content as a file response with the appropriate content type
            return File(fileBytes, GetContentType(fileExtension));
        }

        private string GetContentType(string fileExtension)
        {
            // Map the file extension to the appropriate content type
            // You can use the following example or create your own mapping logic
            switch (fileExtension)
            {
                case ".pdf":
                    return "application/pdf";
                case ".png":
                    return "image/png";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                // Add more file extensions and content types as needed
                default:
                    return "application/octet-stream";
            }
        }

    }
}
