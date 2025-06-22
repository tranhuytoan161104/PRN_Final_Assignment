using System.ComponentModel.DataAnnotations;

namespace Final.Domain.Entities
{
    public class Brand
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Product>? Products { get; set; }
    }
}