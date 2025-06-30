using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Domain.Entities
{
    public class ProductImage
    {
        public long Id { get; set; }
        public string ImageUrl { get; set; } = null!;
        public long ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
    }
}
