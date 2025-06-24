using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final.Domain.Entities
{
    public class Review
    {
        [Key]
        public long Id { get; set; }

        public long ProductId { get; set; }


        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        public long UserId { get; set; }


        [ForeignKey("UserId")]
        public virtual User? User { get; set; }


        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}