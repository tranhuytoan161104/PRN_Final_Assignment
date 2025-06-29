using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Domain.Queries
{
    public class ProductQuery
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public long? CategoryId { get; set; }

        public long? BrandId { get; set; }

        public string? Name { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public string? SortBy { get; set; }

        public string? SortDirection { get; set; }
    }
}
