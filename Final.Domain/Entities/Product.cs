using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Final.Domain.Enums;

namespace Final.Domain.Entities
{
    public class Product
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime AddAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public EProductStatus Status { get; set; } = EProductStatus.Available;

        public long BrandId { get; set; }
        public long CategoryId { get; set; }
        public virtual Brand? Brand { get; set; }
        public virtual Category? Category { get; set; }

        public virtual ICollection<ProductImage>? Images { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
    }
}
