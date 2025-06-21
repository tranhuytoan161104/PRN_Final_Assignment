using Microsoft.EntityFrameworkCore;
using Final.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Persistence.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "CPU (Vi xử lý)" },
                new Category { Id = 2, Name = "GPU (Card đồ họa)" },
                new Category { Id = 3, Name = "RAM (Bộ nhớ trong)" },
                new Category { Id = 4, Name = "SSD (Ổ cứng thể rắn)" },
                new Category { Id = 5, Name = "PSU (Nguồn máy tính)" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Intel Core i9-14900K", Price = 15500000m, CategoryId = 1 },
                new Product { Id = 2, Name = "Intel Core i7-14700K", Price = 11200000m, CategoryId = 1 },
                new Product { Id = 3, Name = "AMD Ryzen 9 7950X3D", Price = 14800000m, CategoryId = 1 },
                new Product { Id = 4, Name = "AMD Ryzen 7 7800X3D", Price = 9800000m, CategoryId = 1 },

                new Product { Id = 5, Name = "NVIDIA GeForce RTX 4090", Price = 45000000m, CategoryId = 2 },
                new Product { Id = 6, Name = "NVIDIA GeForce RTX 4080 Super", Price = 31000000m, CategoryId = 2 },
                new Product { Id = 7, Name = "AMD Radeon RX 7900 XTX", Price = 28500000m, CategoryId = 2 },
                new Product { Id = 8, Name = "AMD Radeon RX 7800 XT", Price = 15000000m, CategoryId = 2 },

                new Product { Id = 9, Name = "Corsair Vengeance DDR5 32GB (2x16GB) 6000MHz", Price = 3200000m, CategoryId = 3 },
                new Product { Id = 10, Name = "G.Skill Trident Z5 RGB DDR5 32GB (2x16GB) 6400MHz", Price = 3800000m, CategoryId = 3 },
                new Product { Id = 11, Name = "Kingston Fury Beast DDR5 16GB 5200MHz", Price = 1500000m, CategoryId = 3 },
                new Product { Id = 12, Name = "Crucial Pro DDR5 16GB 5600MHz", Price = 1450000m, CategoryId = 3 },

                new Product { Id = 13, Name = "Samsung 990 Pro NVMe M.2 SSD 2TB", Price = 4500000m, CategoryId = 4 },
                new Product { Id = 14, Name = "WD Black SN850X NVMe M.2 SSD 2TB", Price = 4200000m, CategoryId = 4 },
                new Product { Id = 15, Name = "Crucial P5 Plus NVMe M.2 SSD 1TB", Price = 2100000m, CategoryId = 4 },
                new Product { Id = 16, Name = "Kingston KC3000 NVMe M.2 SSD 1TB", Price = 2300000m, CategoryId = 4 },

                new Product { Id = 17, Name = "Corsair RM1000e 1000W 80+ Gold", Price = 4100000m, CategoryId = 5 },
                new Product { Id = 18, Name = "Seasonic FOCUS Plus Gold 850W", Price = 3400000m, CategoryId = 5 },
                new Product { Id = 19, Name = "Cooler Master MWE Gold 750 V2", Price = 2500000m, CategoryId = 5 },
                new Product { Id = 20, Name = "MSI MPG A850G PCIE5 850W", Price = 3600000m, CategoryId = 5 }
            );
        }
    }
}
