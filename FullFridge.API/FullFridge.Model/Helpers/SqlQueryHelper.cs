namespace FullFridge.Model.Helpers
{
    public static class SqlQueryHelper
    {
        public static string GetRecipeId =>
            @"SELECT id FROM recipes WHERE id = @id";

        public static string SearchRecipesByRegex =>
            @"SELECT id, title, description, created_by_id, created_at, image, rating FROM recipes WHERE UPPER(title) LIKE UPPER(@regex)";

        public static string GetTopRecipes =>
            @"SELECT id, title, description, created_by_id, created_at, image, rating FROM recipes ORDER BY rating desc LIMIT 12";

        public static string GetRecipes =>
            @"SELECT r.id, r.title, r.description, r.created_by_id, r.created_at, r.image, r.rating, array_agg(rp.product_id) AS Products
              FROM recipes AS r
              LEFT JOIN recipes_products AS rp ON r.id = rp.recipe_id
              GROUP BY r.id
              ORDER BY r.rating DESC";

        public static string GetUserFromEmail =>
            @"SELECT id, email, password, role, name, surname FROM users WHERE email = @email";

        public static string InsertUser =>
            @"INSERT INTO users
              (email, password, role, name, surname)
              VALUES (@Email, @Password, @Role, @Name, @Surname)";

        public static string GetRecipeById =>
            @"SELECT r.id, r.title, r.description, r.created_by_id, r.created_at, r.image, r.rating, array_agg(rp.product_id) AS Products
              FROM recipes AS r
              LEFT JOIN recipes_products AS rp ON r.id = rp.recipe_id
              WHERE r.id = @id
              GROUP BY r.id";

        public static string GetRecipeComments =>
            @"SELECT c.id, c.content, c.created_at, c.rating, u.email, u.name, u.surname
              FROM comments AS c
              JOIN users AS u ON c.created_by_id = u.id
              WHERE c.recipe_id = @recipeId";

        public static string InsertRecipe =>
            @"INSERT INTO recipes (title, description, created_by_id, image, rating)
              VALUES (@Title, @Description, @CreatedById, @Image, 0)
              RETURNING id";

        public static string DeleteRecipe =>
            @"DELETE FROM comments WHERE recipe_id = @id;
              DELETE FROM recipes WHERE id = @id";

        public static string InsertComment =>
            @"INSERT INTO comments (content, created_by_id, recipe_id, rating)
              VALUES (@Content, @CreatedById, @RecipeId, @Rating)";

        public static string ChangeRating =>
            @"UPDATE recipes SET rating = @rating WHERE id = @RecipeId";

        public static string GetRating =>
            @"SELECT rating FROM recipes WHERE id = @RecipeId";

        public static string GetCommentCount =>
            @"SELECT COUNT(id) FROM comments WHERE recipe_id = @RecipeId";

        public static string AlreadyCommented =>
            @"SELECT id FROM comments WHERE recipe_id = @RecipeId AND created_by_id = @CreatedById";

        public static string AssignImage =>
            "UPDATE recipes SET image = @fileName WHERE id = @id";

        public static string InsertMealDbRecipe =>
            @"INSERT INTO recipes (title, description, image, rating, mealdb_id)
              VALUES (@Title, @Description, @Image, 0, @MealDbId)
              RETURNING id";

        public static string InsertRecipeProduct =>
            @"INSERT INTO recipes_products (recipe_id, product_id)
              VALUES (@RecipeId, @ProductId)";

        public static string InsertPost =>
            @"INSERT INTO posts (title, content, created_by, recipe_id)
              VALUES (@Title, @Content, @CreatedBy, @RecipeId)";

        public static string InsertPostComment =>
            @"INSERT INTO post_comments (post_id, content, created_by)
	          VALUES (@PostId, @Content, @CreatedBy)";

        public static string GetPosts =>
            @"SELECT id, title, content, created_by AS CreatedBy, created_at AS CreatedAt, recipe_id As RecipeId FROM posts ORDER BY created_at desc";

        public static string GetPostComments =>
            @"SELECT c.id, c.post_id AS PostId, c.content, c.created_at AS CreatedAt, u.name, u.surname
              FROM post_comments AS c 
              JOIN users AS u ON c.created_by = u.id
              WHERE post_id = @postId";

        public static string GetUserDetails =>
            @"SELECT id, email, role, name, surname FROM users WHERE id = @id";

        public static string GetPostById =>
            @"SELECT id, title, content, created_by AS CreatedBy, created_at AS CreatedAt, recipe_id AS RecipeId FROM posts WHERE id = @id";
    }
}
