namespace FullFridge.Model.Helpers
{
    public static class SqlQueryHelper
    {
        public static string GetRecipeId =>
            @"SELECT Id FROM Recipes WHERE Id = @id";

        public static string SearchRecipesByRegex =>
            @"SELECT Id, Title, Description, CreatedById, CreatedAt, Image, Rating FROM Recipes WHERE Title LIKE '%@regex%'";

        public static string GetTopRecipes =>
            @"SELECT Id, Title, Description, CreatedById, CreatedAt, Image, Rating FROM Recipes ORDER BY Rating desc LIMIT 12";

        public static string GetRecipes =>
            @"SELECT r.Id, r.Title, r.Description, r.CreatedById, r.CreatedAt, r.Image, r. Rating, array_agg(rp.ProductId) AS Products
              FROM Recipes AS r
              LEFT JOIN RecipesProducts AS rp ON r.Id = rp.RecipeId
              GROUP BY r.Id
              ORDER BY r.Rating DESC";

        public static string GetUserFromEmail =>
            @"SELECT Id, Email, Password, Role, Name, Surname FROM Users WHERE Email = @email";

        public static string InsertUser =>
            @"INSERT INTO Users
              (Email, Password, Role, Name, Surname)
              VALUES (@Email, @Password, @Role, @Name, @Surname)";

        public static string GetRecipeById =>
            @"SELECT r.Id, r.Title, r.Description, r.CreatedById, r.CreatedAt, r.Image, r. Rating, array_agg(rp.ProductId) AS Products
              FROM Recipes AS r
              LEFT JOIN RecipesProducts AS rp ON r.Id = rp.RecipeId
              GROUP BY r.Id
              WHERE r.Id = @id";

        public static string GetRecipeComments =>
            @"SELECT c.Id, c.Content, c.CreatedAt, c.Rating, u.Email, u.Name, u.Surname
              FROM Comments AS c
              JOIN Users AS u ON c.CreatedById = u.Id
              WHERE c.RecipeId = @recipeId";

        public static string InsertRecipe =>
            @"INSERT INTO Recipes (Title, Description, CreatedById, Image, Rating)
              VALUES (@Title, @Description, @CreatedById, @Image, 0)
              RETURNING Id";

        public static string DeleteRecipe =>
            @"DELETE FROM Comments WHERE RecipeId = @id;
              DELETE FROM Recipes WHERE Id = @id";

        public static string InsertComment =>
            @"INSERT INTO Comments (Content, CreatedById, RecipeId, Rating)
              VALUES (@Content, @CreatedById, @RecipeId, @Rating)";

        public static string ChangeRating =>
            @"UPDATE Recipes SET Rating = @rating WHERE Id = @RecipeId";

        public static string GetRating =>
            @"SELECT Rating FROM Recipes WHERE Id = @RecipeId";

        public static string GetCommentCount =>
            @"SELECT COUNT(Id) FROM Comments WHERE RecipeId = @RecipeId";

        public static string AlreadyCommented =>
            "@SELECT Id FROM Comments WHERE RecipeId = @RecipeId AND CreatedById = @CreatedById";

        public static string AssignImage =>
            "UPDATE Recipes SET Image = @fileName WHERE Id = id";
    }
}
