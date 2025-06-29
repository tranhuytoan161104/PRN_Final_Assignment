using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Domain.Entities
{
    public class Category
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }



        public virtual ICollection<Product>? Products { get; set; }
    }
}
