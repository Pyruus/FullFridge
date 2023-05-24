using FullFridge.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FullFridge.API.Context
{
    public class FullFridgeContext: DbContext
    {
        public FullFridgeContext(DbContextOptions<FullFridgeContext> options):
            base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
            modelBuilder.Entity<ProductsRecipes>()
                .HasKey(pr => new { pr.ProductId, pr.RecipeId });
            modelBuilder.Entity<ProductsRecipes>()
                .HasOne(pr => pr.Product)
                .WithMany(p => p.ProductsRecipes)
                .HasForeignKey(pr => pr.ProductId);
            modelBuilder.Entity<ProductsRecipes>()
                .HasOne(pr => pr.Recipe)
                .WithMany(r => r.ProductsRecipes)
                .HasForeignKey(pr => pr.RecipeId);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductsRecipes> ProductsRecipes { get; set; }
    }
}
