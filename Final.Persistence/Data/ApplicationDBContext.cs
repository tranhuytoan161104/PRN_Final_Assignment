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
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            // === Fluent API Configurations ===

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.StockQuantity).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.AddAt).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasConversion<string>().HasMaxLength(50);
                entity.HasOne(e => e.Brand)
                    .WithMany(b => b.Products)
                    .HasForeignKey(e => e.BrandId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImageUrl).IsRequired();
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.Images)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Rating).IsRequired();
                entity.Property(e => e.Rating).HasDefaultValue(1);
                entity.Property(e => e.Comment);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Reviews)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.OrderDate).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.ShippingAddress).IsRequired();
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Quantity).IsRequired();
                entity.HasOne(e => e.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.User)
                      .WithOne(u => u.ShoppingCart)
                      .HasForeignKey<ShoppingCart>(e => e.UserId)
                      .IsRequired();
            });

            modelBuilder.Entity<ShoppingCartItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.ShoppingCart)
                      .WithMany(sc => sc.Items)
                      .HasForeignKey(e => e.ShoppingCartId)
                      .IsRequired();

                entity.HasOne(e => e.Product)
                      .WithMany() 
                      .HasForeignKey(e => e.ProductId)
                      .IsRequired();
            });


            // === Seed Data ===

            #region === Category Seed Data ===
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "CPU" },
                new Category { Id = 2, Name = "GPU" },
                new Category { Id = 3, Name = "RAM" },
                new Category { Id = 4, Name = "SSD" },
                new Category { Id = 5, Name = "PSU" }
            );
            #endregion

            #region === Brand Seed Data ===
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
            #endregion

            #region === User Seed Data ===
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FirstName = "Admin", LastName = "User", Email = "admin@example.com", PasswordHash = "$2a$11$9i.2nCqjA1DkC8B4lQ9C8uJ.Uj5GqXy.z/A7X2Q.Z9iB8qF.K/9W.", Role = "Admin", CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1), DateTimeKind.Utc) },
                new User { Id = 2, FirstName = "An", LastName = "Nguyễn", Email = "customer1@example.com", PasswordHash = "$2a$11$gT/jKqC0Z.I7H.v2Uu4j6u8kK/gL7Xy.Z/Q5F/E9Z/A9qG.H/3e.", Role = "Customer", CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 2, 15), DateTimeKind.Utc) },
                new User { Id = 3, FirstName = "Bình", LastName = "Trần", Email = "customer2@example.com", PasswordHash = "$2a$11$gT/jKqC0Z.I7H.v2Uu4j6u8kK/gL7Xy.Z/Q5F/E9Z/A9qG.H/3e.", Role = "Customer", CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 3, 20), DateTimeKind.Utc) }
            );
            #endregion

            #region === Product Seed Data ===
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, CategoryId = 1, BrandId = 1, Name = "Intel Core i9-14900K", Price = 15500000m, Description = "Vi xử lý đầu bảng cho gaming và sáng tạo nội dung, 24 nhân 32 luồng, tốc độ tối đa 6.0 GHz.", StockQuantity = 50, CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 10), DateTimeKind.Utc), AddAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc), Status = Domain.Enums.EProductStatus.Available },
                new Product { Id = 2, CategoryId = 1, BrandId = 1, Name = "Intel Core i7-14700K", Price = 11200000m, Description = "Lựa chọn tuyệt vời cho gaming hiệu năng cao, 20 nhân 28 luồng.", StockQuantity = 80, CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 10), DateTimeKind.Utc), AddAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc), Status = Domain.Enums.EProductStatus.Available },
                new Product { Id = 3, CategoryId = 1, BrandId = 2, Name = "AMD Ryzen 9 7950X3D", Price = 14800000m, Description = "Vua gaming với công nghệ 3D V-Cache, 16 nhân 32 luồng.", StockQuantity = 45, CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 2, 5), DateTimeKind.Utc), AddAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc), Status = Domain.Enums.EProductStatus.Available },
                new Product { Id = 4, CategoryId = 1, BrandId = 2, Name = "AMD Ryzen 7 7800X3D", Price = 9800000m, Description = "Hiệu năng gaming thuần túy tốt nhất phân khúc nhờ 3D V-Cache, 8 nhân 16 luồng.", StockQuantity = 120, CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 2, 5), DateTimeKind.Utc), AddAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc), Status = Domain.Enums.EProductStatus.Available },

                new Product { Id = 5, CategoryId = 2, BrandId = 3, Name = "NVIDIA GeForce RTX 4090", Price = 45000000m, Description = "Sức mạnh tối thượng cho gaming 4K và các tác vụ AI, Ray Tracing đỉnh cao.", StockQuantity = 25, CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 15), DateTimeKind.Utc), AddAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc), Status = Domain.Enums.EProductStatus.Available },
                new Product { Id = 6, CategoryId = 2, BrandId = 3, Name = "NVIDIA GeForce RTX 4080 Super", Price = 31000000m, Description = "Hiệu năng vượt trội cho gaming 1440p và 4K, phiên bản nâng cấp của RTX 4080.", StockQuantity = 40, CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 2, 20), DateTimeKind.Utc), AddAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc), Status = Domain.Enums.EProductStatus.Available },
                new Product { Id = 7, CategoryId = 2, BrandId = 2, Name = "AMD Radeon RX 7900 XTX", Price = 28500000m, Description = "Card đồ họa đầu bảng của AMD, đối thủ cạnh tranh trực tiếp với RTX 4080.", StockQuantity = 35, CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 25), DateTimeKind.Utc), AddAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc), Status = Domain.Enums.EProductStatus.Available },
                new Product { Id = 8, CategoryId = 2, BrandId = 2, Name = "AMD Radeon RX 7800 XT", Price = 15000000m, Description = "Lựa chọn p/p tốt nhất cho gaming 1440p.", StockQuantity = 90, CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 3, 1), DateTimeKind.Utc), AddAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc), Status = Domain.Enums.EProductStatus.Available },

                new Product { Id = 9, CategoryId = 3, BrandId = 4, Name = "Corsair Vengeance DDR5 32GB (2x16GB) 6000MHz", Price = 3200000m, Description = "Kit RAM DDR5 hiệu năng cao, tản nhiệt nhôm, tương thích tốt với Intel XMP và AMD EXPO.", StockQuantity = 150, CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 5), DateTimeKind.Utc), AddAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc), Status = Domain.Enums.EProductStatus.Available },
                new Product { Id = 10, CategoryId = 3, BrandId = 8, Name = "G.Skill Trident Z5 RGB DDR5 32GB (2x16GB) 6400MHz", Price = 3800000m, Description = "Thiết kế đẹp mắt với LED RGB, tốc độ bus cao dành cho người dùng chuyên nghiệp và game thủ.", StockQuantity = 110, CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 2, 12), DateTimeKind.Utc), AddAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc), Status = Domain.Enums.EProductStatus.Available },

                new Product { Id = 13, CategoryId = 4, BrandId = 6, Name = "Samsung 990 Pro NVMe M.2 SSD 2TB", Price = 4500000m, Description = "Ổ cứng NVMe Gen4 nhanh nhất thị trường, lý tưởng cho hệ điều hành, game và ứng dụng nặng.", StockQuantity = 70, CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 2, 18), DateTimeKind.Utc), AddAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc), Status = Domain.Enums.EProductStatus.Available },
                new Product { Id = 14, CategoryId = 4, BrandId = 7, Name = "WD Black SN850X NVMe M.2 SSD 2TB", Price = 4200000m, Description = "Tốc độ đọc ghi cực nhanh, lựa chọn hàng đầu của game thủ.", StockQuantity = 85, CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 3, 3), DateTimeKind.Utc), AddAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc), Status = Domain.Enums.EProductStatus.Available }
            );
            #endregion

            #region === ProductImage Seed Data ===
            modelBuilder.Entity<ProductImage>().HasData(
                new ProductImage { Id = 1, ProductId = 1, ImageUrl = "https://placehold.co/600x600/EFEFEF/333?text=i9-14900K-1" },
                new ProductImage { Id = 2, ProductId = 1, ImageUrl = "https://placehold.co/600x600/EFEFEF/333?text=i9-14900K-2" },

                new ProductImage { Id = 3, ProductId = 2, ImageUrl = "https://placehold.co/600x600/EFEFEF/333?text=i7-14700K" },

                new ProductImage { Id = 4, ProductId = 5, ImageUrl = "https://placehold.co/600x600/1B3C34/FFF?text=RTX+4090-main" },
                new ProductImage { Id = 5, ProductId = 5, ImageUrl = "https://placehold.co/600x600/1B3C34/FFF?text=RTX+4090-side" },
                new ProductImage { Id = 6, ProductId = 5, ImageUrl = "https://placehold.co/600x600/1B3C34/FFF?text=RTX+4090-box" }
            );
            #endregion

            #region === Review Seed Data ===
            modelBuilder.Entity<Review>().HasData(
                new Review { Id = 1, ProductId = 1, UserId = 2, Rating = 5, Comment = "CPU quá mạnh, chạy đa nhiệm và render video cực nhanh. Rất đáng tiền!", CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 4, 1), DateTimeKind.Utc) },
                new Review { Id = 2, ProductId = 1, UserId = 3, Rating = 5, Comment = "Hiệu năng chơi game đỉnh cao, không có gì để chê.", CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 4, 5), DateTimeKind.Utc) },
                new Review { Id = 3, ProductId = 5, UserId = 2, Rating = 5, Comment = "Đúng là trùm cuối card đồ họa. Chơi Cyberpunk 2077 max setting 4K mượt mà. Đắt nhưng xắt ra miếng.", CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 4, 10), DateTimeKind.Utc) },
                new Review { Id = 4, ProductId = 8, UserId = 3, Rating = 4, Comment = "Hiệu năng chơi game 2K rất tốt trong tầm giá. Chỉ có điều card hơi nóng một chút.", CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 4, 12), DateTimeKind.Utc) },
                new Review { Id = 5, ProductId = 9, UserId = 2, Rating = 5, Comment = "RAM chạy ổn định, cắm vào là nhận ngay, không gặp vấn đề gì.", CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 3, 29), DateTimeKind.Utc) }
            );
            #endregion
        }
    }
}