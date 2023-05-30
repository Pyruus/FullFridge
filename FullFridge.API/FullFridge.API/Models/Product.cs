namespace FullFridge.API.Models
{
    public class Product
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int Calories { get; set; }
        public ICollection<ProductsRecipes> ProductsRecipes { get; set; }
    }

    public class ProductDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int Calories { get; set; }
    }
}
