namespace Final.WebApp.DTOs.Products
{
    public class ProductDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
