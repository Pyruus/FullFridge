using FullFridge.API.Context;

namespace FullFridge.API.Services
{
    public class RecipeService: IRecipeService
    {
        private readonly FullFridgeContext _context;
        public RecipeService(FullFridgeContext context)
        {
            _context = context;
        }

        public bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }

    public interface IRecipeService
    {
        bool RecipeExists(int id);
    }
}
