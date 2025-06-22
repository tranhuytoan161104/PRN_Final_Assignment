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
                new Category { Id = 1, Name = "CPU (Vi xử lý)", Slug = "cpu" },
                new Category { Id = 2, Name = "GPU (Card đồ họa)", Slug = "gpu" },
                new Category { Id = 3, Name = "RAM (Bộ nhớ trong)", Slug = "ram" },
                new Category { Id = 4, Name = "SSD (Ổ cứng thể rắn)", Slug = "ssd" },
                new Category { Id = 5, Name = "PSU (Nguồn máy tính)", Slug = "psu" }
            );

            modelBuilder.Entity<Brand>().HasData(
                new Brand { Id = 1, Name = "Intel" },
                new Brand { Id = 2, Name = "AMD" },
                new Brand { Id = 3, Name = "NVIDIA" },
                new Brand { Id = 4, Name = "Corsair" },
                new Brand { Id = 5, Name = "Kingston" },
                new Brand { Id = 6, Name = "Samsung" },
                new Brand { Id = 7, Name = "Western Digital" },
                new Brand { Id = 8, Name = "G.Skill" },
                new Brand { Id = 9, Name = "Crucial" },
                new Brand { Id = 10, Name = "Seasonic" },
                new Brand { Id = 11, Name = "Cooler Master" },
                new Brand { Id = 12, Name = "MSI" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, CategoryId = 1, BrandId = 1, Name = "Intel Core i9-14900K", Sku = "CPU-INT-14900K", Price = 15500000m },
                new Product { Id = 2, CategoryId = 1, BrandId = 1, Name = "Intel Core i7-14700K", Sku = "CPU-INT-14700K", Price = 11200000m },
                new Product { Id = 3, CategoryId = 1, BrandId = 2, Name = "AMD Ryzen 9 7950X3D", Sku = "CPU-AMD-7950X3D", Price = 14800000m },
                new Product { Id = 4, CategoryId = 1, BrandId = 2, Name = "AMD Ryzen 7 7800X3D", Sku = "CPU-AMD-7800X3D", Price = 9800000m },

                new Product { Id = 5, CategoryId = 2, BrandId = 3, Name = "NVIDIA GeForce RTX 4090", Sku = "GPU-NV-4090", Price = 45000000m },
                new Product { Id = 6, CategoryId = 2, BrandId = 3, Name = "NVIDIA GeForce RTX 4080 Super", Sku = "GPU-NV-4080S", Price = 31000000m },
                new Product { Id = 7, CategoryId = 2, BrandId = 2, Name = "AMD Radeon RX 7900 XTX", Sku = "GPU-AMD-7900XTX", Price = 28500000m },
                new Product { Id = 8, CategoryId = 2, BrandId = 2, Name = "AMD Radeon RX 7800 XT", Sku = "GPU-AMD-7800XT", Price = 15000000m },

                new Product { Id = 9, CategoryId = 3, BrandId = 4, Name = "Corsair Vengeance DDR5 32GB (2x16GB) 6000MHz", Sku = "RAM-COR-V32GB", Price = 3200000m },
                new Product { Id = 10, CategoryId = 3, BrandId = 8, Name = "G.Skill Trident Z5 RGB DDR5 32GB (2x16GB) 6400MHz", Sku = "RAM-GSK-Z532GB", Price = 3800000m },
                new Product { Id = 11, CategoryId = 3, BrandId = 5, Name = "Kingston Fury Beast DDR5 16GB 5200MHz", Sku = "RAM-KIN-F16GB", Price = 1500000m },
                new Product { Id = 12, CategoryId = 3, BrandId = 9, Name = "Crucial Pro DDR5 16GB 5600MHz", Sku = "RAM-CRU-P16GB", Price = 1450000m },

                new Product { Id = 13, CategoryId = 4, BrandId = 6, Name = "Samsung 990 Pro NVMe M.2 SSD 2TB", Sku = "SSD-SS-990P2TB", Price = 4500000m },
                new Product { Id = 14, CategoryId = 4, BrandId = 7, Name = "WD Black SN850X NVMe M.2 SSD 2TB", Sku = "SSD-WD-850X2TB", Price = 4200000m },
                new Product { Id = 15, CategoryId = 4, BrandId = 9, Name = "Crucial P5 Plus NVMe M.2 SSD 1TB", Sku = "SSD-CRU-P5P1TB", Price = 2100000m },
                new Product { Id = 16, CategoryId = 4, BrandId = 5, Name = "Kingston KC3000 NVMe M.2 SSD 1TB", Sku = "SSD-KIN-KC3K1TB", Price = 2300000m },

                new Product { Id = 17, CategoryId = 5, BrandId = 4, Name = "Corsair RM1000e 1000W 80+ Gold", Sku = "PSU-COR-RM1000E", Price = 4100000m },
                new Product { Id = 18, CategoryId = 5, BrandId = 10, Name = "Seasonic FOCUS Plus Gold 850W", Sku = "PSU-SEA-FP850W", Price = 3400000m },
                new Product { Id = 19, CategoryId = 5, BrandId = 11, Name = "Cooler Master MWE Gold 750 V2", Sku = "PSU-CM-MWE750", Price = 2500000m },
                new Product { Id = 20, CategoryId = 5, BrandId = 12, Name = "MSI MPG A850G PCIE5 850W", Sku = "PSU-MSI-A850G", Price = 3600000m }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FirstName = "Admin", LastName = "User", Email = "admin@example.com", PasswordHash = "123", Role = "Admin" },
                new User { Id = 2, FirstName = "Test", LastName = "Customer", Email = "customer@example.com", PasswordHash = "123", Role = "Customer" }
            );
        }
    }
}
