namespace Final.WebApp.DTOs.Products
{
    public class ProductQuery
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 8;

        public long? CategoryId { get; set; }

        public long? BrandId { get; set; }

        public string? Name { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public string? SortBy { get; set; }

        public string? SortDirection { get; set; }
    }
}
