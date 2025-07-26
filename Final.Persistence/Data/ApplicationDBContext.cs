using Final.Domain.Entities;
using Final.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

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
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
                entity.HasIndex(e => e.Email).IsUnique();
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
                entity.Property(e => e.AddAt);
                entity.Property(e => e.Status).IsRequired().HasConversion<string>().HasMaxLength(50);
                entity.HasOne(e => e.Brand).WithMany(b => b.Products).HasForeignKey(e => e.BrandId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Category).WithMany(c => c.Products).HasForeignKey(e => e.CategoryId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImageUrl).IsRequired();
                entity.HasOne(e => e.Product).WithMany(p => p.Images).HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Rating).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.HasOne(e => e.Product).WithMany(p => p.Reviews).HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.User).WithMany(u => u.Reviews).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.OrderDate).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasConversion<string>().HasMaxLength(50);
                entity.Property(e => e.ShippingAddress).IsRequired();
                entity.HasOne(e => e.User).WithMany(u => u.Orders).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Quantity).IsRequired();
                entity.HasOne(e => e.Order).WithMany(o => o.OrderItems).HasForeignKey(e => e.OrderId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Product).WithMany(p => p.OrderItems).HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User).WithOne(u => u.ShoppingCart).HasForeignKey<ShoppingCart>(e => e.UserId).IsRequired();
            });

            modelBuilder.Entity<ShoppingCartItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.ShoppingCart).WithMany(sc => sc.Items).HasForeignKey(e => e.ShoppingCartId).IsRequired();
                entity.HasOne(e => e.Product).WithMany().HasForeignKey(e => e.ProductId).IsRequired();
            });

            modelBuilder.Entity<PaymentTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Status).IsRequired().HasConversion<string>().HasMaxLength(50);
                entity.HasOne(e => e.Order).WithMany(o => o.PaymentTransactions).HasForeignKey(e => e.OrderId).IsRequired();
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Code).IsUnique();
            });


            const string universalPasswordHash = "$2a$11$N1brDk6.a9UHivirpPppuuV30cywfm.PCZIOdKoe6RPb1zfVdjlM2";

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FirstName = "Văn", LastName = "Toàn", Email = "owner@final.com", PasswordHash = universalPasswordHash, Role = "Owner", Status = EUserStatus.Active, CreatedAt = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc) },
                new User { Id = 2, FirstName = "Quốc", LastName = "Tuấn", Email = "admin@final.com", PasswordHash = universalPasswordHash, Role = "Admin", Status = EUserStatus.Active, CreatedAt = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc) },
                new User { Id = 3, FirstName = "Thanh", LastName = "Bình", Email = "admin2@final.com", PasswordHash = universalPasswordHash, Role = "Admin", Status = EUserStatus.Inactive, CreatedAt = DateTime.SpecifyKind(new DateTime(2023, 1, 15), DateTimeKind.Utc) },
                new User { Id = 4, FirstName = "Minh", LastName = "An", Email = "minhan@customer.com", PasswordHash = universalPasswordHash, Role = "Customer", Status = EUserStatus.Active, CreatedAt = DateTime.SpecifyKind(new DateTime(2023, 2, 10), DateTimeKind.Utc) },
                new User { Id = 5, FirstName = "Bảo", LastName = "Châu", Email = "baochau@customer.com", PasswordHash = universalPasswordHash, Role = "Customer", Status = EUserStatus.Active, CreatedAt = DateTime.SpecifyKind(new DateTime(2023, 3, 20), DateTimeKind.Utc) },
                new User { Id = 6, FirstName = "Gia", LastName = "Hân", Email = "giahan@customer.com", PasswordHash = universalPasswordHash, Role = "Customer", Status = EUserStatus.Active, CreatedAt = DateTime.SpecifyKind(new DateTime(2023, 5, 1), DateTimeKind.Utc) },
                new User { Id = 7, FirstName = "Đăng", LastName = "Khoa", Email = "dangkhoa@customer.com", PasswordHash = universalPasswordHash, Role = "Customer", Status = EUserStatus.Active, CreatedAt = DateTime.SpecifyKind(new DateTime(2023, 6, 12), DateTimeKind.Utc) }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Vi xử lý (CPU)" },
                new Category { Id = 2, Name = "Card đồ họa (GPU)" },
                new Category { Id = 3, Name = "Bộ nhớ RAM" },
                new Category { Id = 4, Name = "Ổ cứng SSD" },
                new Category { Id = 5, Name = "Nguồn máy tính (PSU)" },
                new Category { Id = 6, Name = "Bo mạch chủ (Mainboard)" },
                new Category { Id = 7, Name = "Màn hình (Monitor)" }
            );

            modelBuilder.Entity<Brand>().HasData(
                new Brand { Id = 1, Name = "Intel" }, new Brand { Id = 2, Name = "AMD" }, new Brand { Id = 3, Name = "NVIDIA" },
                new Brand { Id = 4, Name = "Corsair" }, new Brand { Id = 5, Name = "Kingston" }, new Brand { Id = 6, Name = "Samsung" },
                new Brand { Id = 7, Name = "Western Digital" }, new Brand { Id = 8, Name = "G.Skill" }, new Brand { Id = 9, Name = "Crucial" },
                new Brand { Id = 10, Name = "Seasonic" }, new Brand { Id = 11, Name = "Cooler Master" }, new Brand { Id = 12, Name = "MSI" },
                new Brand { Id = 13, Name = "ASUS" }, new Brand { Id = 14, Name = "Gigabyte" }, new Brand { Id = 15, Name = "LG" },
                new Brand { Id = 16, Name = "Dell" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, CategoryId = 1, BrandId = 1, Name = "Intel Core i9-14900K", Price = 15500000m, Description = "Vi xử lý đầu bảng cho gaming và sáng tạo nội dung.", StockQuantity = 50, CreatedAt = DateTime.UtcNow.AddDays(-100), Status = EProductStatus.Available },
                new Product { Id = 2, CategoryId = 1, BrandId = 2, Name = "AMD Ryzen 7 7800X3D", Price = 9800000m, Description = "Hiệu năng gaming thuần túy tốt nhất phân khúc nhờ 3D V-Cache.", StockQuantity = 120, CreatedAt = DateTime.UtcNow.AddDays(-90), Status = EProductStatus.Available },
                new Product { Id = 3, CategoryId = 1, BrandId = 1, Name = "Intel Core i5-14600K", Price = 8500000m, Description = "Vi xử lý tầm trung p/p tốt nhất cho gaming.", StockQuantity = 0, CreatedAt = DateTime.UtcNow.AddDays(-80), Status = EProductStatus.OutOfStock },
                new Product { Id = 4, CategoryId = 2, BrandId = 3, Name = "NVIDIA GeForce RTX 4090", Price = 45000000m, Description = "Sức mạnh tối thượng cho gaming 4K và các tác vụ AI.", StockQuantity = 25, CreatedAt = DateTime.UtcNow.AddDays(-120), Status = EProductStatus.Available },
                new Product { Id = 5, CategoryId = 2, BrandId = 2, Name = "AMD Radeon RX 7900 XTX", Price = 28500000m, Description = "Card đồ họa đầu bảng của AMD, đối thủ cạnh tranh trực tiếp với RTX 4080.", StockQuantity = 35, CreatedAt = DateTime.UtcNow.AddDays(-110), Status = EProductStatus.Available },
                new Product { Id = 6, CategoryId = 2, BrandId = 14, Name = "Gigabyte RTX 3060 Gaming OC", Price = 8200000m, Description = "Card đồ họa quốc dân cho gaming Full HD.", StockQuantity = 200, CreatedAt = DateTime.UtcNow.AddDays(-200), Status = EProductStatus.Archived },
                new Product { Id = 7, CategoryId = 3, BrandId = 4, Name = "Corsair Vengeance DDR5 32GB 6000MHz", Price = 3200000m, Description = "Kit RAM DDR5 hiệu năng cao, tản nhiệt nhôm.", StockQuantity = 150, CreatedAt = DateTime.UtcNow.AddDays(-150), Status = EProductStatus.Available },
                new Product { Id = 8, CategoryId = 3, BrandId = 8, Name = "G.Skill Trident Z5 RGB DDR5 32GB 6400MHz", Price = 3800000m, Description = "Thiết kế đẹp mắt với LED RGB, tốc độ bus cao.", StockQuantity = 110, CreatedAt = DateTime.UtcNow.AddDays(-140), Status = EProductStatus.Available },
                new Product { Id = 9, CategoryId = 4, BrandId = 6, Name = "Samsung 990 Pro NVMe M.2 SSD 2TB", Price = 4500000m, Description = "Ổ cứng NVMe Gen4 nhanh nhất thị trường.", StockQuantity = 70, CreatedAt = DateTime.UtcNow.AddDays(-180), Status = EProductStatus.Available },
                new Product { Id = 10, CategoryId = 4, BrandId = 7, Name = "WD Black SN850X NVMe M.2 SSD 1TB", Price = 2600000m, Description = "Tốc độ đọc ghi cực nhanh, lựa chọn hàng đầu của game thủ.", StockQuantity = 95, CreatedAt = DateTime.UtcNow.AddDays(-170), Status = EProductStatus.Available },
                new Product { Id = 11, CategoryId = 6, BrandId = 13, Name = "ASUS ROG STRIX Z790-E GAMING WIFI II", Price = 16000000m, Description = "Bo mạch chủ cao cấp cho CPU Intel thế hệ 14.", StockQuantity = 40, CreatedAt = DateTime.UtcNow.AddDays(-60), Status = EProductStatus.Available },
                new Product { Id = 12, CategoryId = 6, BrandId = 12, Name = "MSI MAG B760M MORTAR WIFI DDR5", Price = 5300000m, Description = "Bo mạch chủ tầm trung tốt nhất cho nhu cầu gaming.", StockQuantity = 80, CreatedAt = DateTime.UtcNow.AddDays(-50), Status = EProductStatus.Available },
                new Product { Id = 13, CategoryId = 7, BrandId = 15, Name = "LG UltraGear 27GR95QE-B 240Hz OLED", Price = 24500000m, Description = "Màn hình OLED 2K 240Hz cho trải nghiệm gaming đỉnh cao.", StockQuantity = 30, CreatedAt = DateTime.UtcNow.AddDays(-40), Status = EProductStatus.Available },
                new Product { Id = 14, CategoryId = 7, BrandId = 16, Name = "Dell UltraSharp U2723QE 4K IPS", Price = 13800000m, Description = "Màn hình 4K chuyên đồ họa với tấm nền IPS Black.", StockQuantity = 60, CreatedAt = DateTime.UtcNow.AddDays(-30), Status = EProductStatus.Available }
            );

            modelBuilder.Entity<ProductImage>().HasData(
                new ProductImage { Id = 1, ProductId = 1, ImageUrl = "https://placehold.co/600x600/EFEFEF/333?text=i9-14900K-1" },
                new ProductImage { Id = 2, ProductId = 1, ImageUrl = "https://placehold.co/600x600/EFEFEF/333?text=i9-14900K-2" },
                new ProductImage { Id = 3, ProductId = 2, ImageUrl = "https://placehold.co/600x600/EFEFEF/333?text=7800X3D" },
                new ProductImage { Id = 4, ProductId = 4, ImageUrl = "https://placehold.co/600x600/1B3C34/FFF?text=RTX+4090" },
                new ProductImage { Id = 5, ProductId = 5, ImageUrl = "https://placehold.co/600x600/E83131/FFF?text=RX+7900XTX" },
                new ProductImage { Id = 6, ProductId = 7, ImageUrl = "https://placehold.co/600x600/111/EEE?text=Corsair+RAM" },
                new ProductImage { Id = 7, ProductId = 9, ImageUrl = "https://placehold.co/600x600/0078D4/FFF?text=Samsung+SSD" },
                new ProductImage { Id = 8, ProductId = 11, ImageUrl = "https://placehold.co/600x600/D82727/FFF?text=ASUS+ROG" },
                new ProductImage { Id = 9, ProductId = 13, ImageUrl = "https://placehold.co/600x600/333/FFF?text=LG+OLED" },
                new ProductImage { Id = 10, ProductId = 13, ImageUrl = "https://placehold.co/600x600/333/FFF?text=LG+OLED-2" }
            );

            modelBuilder.Entity<Review>().HasData(
                new Review { Id = 1, ProductId = 2, UserId = 4, Rating = 5, Comment = "CPU gaming tốt nhất hiện tại, không có gì để chê!", CreatedAt = DateTime.UtcNow.AddDays(-80) },
                new Review { Id = 2, ProductId = 4, UserId = 5, Rating = 5, Comment = "Đắt nhưng xắt ra miếng. Cân mọi game 4K max setting.", CreatedAt = DateTime.UtcNow.AddDays(-70) },
                new Review { Id = 3, ProductId = 10, UserId = 6, Rating = 4, Comment = "Tốc độ rất nhanh, nhưng giá hơi cao so với các hãng khác.", CreatedAt = DateTime.UtcNow.AddDays(-60) },
                new Review { Id = 4, ProductId = 12, UserId = 4, Rating = 5, Comment = "Mainboard p/p quá tốt, đầy đủ cổng kết nối.", CreatedAt = DateTime.UtcNow.AddDays(-40) },
                new Review { Id = 5, ProductId = 13, UserId = 7, Rating = 5, Comment = "Màu sắc và tần số quét của màn hình này thật sự tuyệt vời.", CreatedAt = DateTime.UtcNow.AddDays(-20) }
            );

            modelBuilder.Entity<ShoppingCart>().HasData(
                new ShoppingCart { Id = 1, UserId = 5 },
                new ShoppingCart { Id = 2, UserId = 6 }
            );

            modelBuilder.Entity<ShoppingCartItem>().HasData(
                new ShoppingCartItem { Id = 1, ShoppingCartId = 1, ProductId = 8, Quantity = 1 },
                new ShoppingCartItem { Id = 2, ShoppingCartId = 1, ProductId = 10, Quantity = 1 },
                new ShoppingCartItem { Id = 3, ShoppingCartId = 2, ProductId = 14, Quantity = 2 }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, UserId = 4, TotalAmount = 15100000m, OrderDate = DateTime.UtcNow.AddDays(-45), Status = EOrderStatus.Delivered, ShippingAddress = "123 Đường ABC, Quận 1, TP.HCM", PhoneNumber = "0901234567" },
                new Order { Id = 2, UserId = 7, TotalAmount = 53000000m, OrderDate = DateTime.UtcNow.AddDays(-25), Status = EOrderStatus.Processing, ShippingAddress = "456 Đường XYZ, Quận Hoàn Kiếm, Hà Nội", PhoneNumber = "0987654321" },
                new Order { Id = 3, UserId = 4, TotalAmount = 3200000m, OrderDate = DateTime.UtcNow.AddDays(-10), Status = EOrderStatus.Cancelled, ShippingAddress = "123 Đường ABC, Quận 1, TP.HCM", PhoneNumber = "0901234567" }
            );
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { Id = 1, OrderId = 1, ProductId = 2, Quantity = 1, Price = 9800000m },
                new OrderItem { Id = 2, OrderId = 1, ProductId = 12, Quantity = 1, Price = 5300000m },
                new OrderItem { Id = 3, OrderId = 2, ProductId = 5, Quantity = 1, Price = 28500000m },
                new OrderItem { Id = 4, OrderId = 2, ProductId = 13, Quantity = 1, Price = 24500000m },
                new OrderItem { Id = 5, OrderId = 3, ProductId = 7, Quantity = 1, Price = 3200000m }
            );

            modelBuilder.Entity<PaymentMethod>().HasData(
                new PaymentMethod { Id = 1, Code = "COD", IsActive = true },
                new PaymentMethod { Id = 2, Code = "MOMO", IsActive = true },
                new PaymentMethod { Id = 3, Code = "VNPAY", IsActive = true },
                new PaymentMethod { Id = 4, Code = "BANK_TRANSFER", IsActive = false }
            );

            modelBuilder.Entity<PaymentTransaction>().HasData(
                new PaymentTransaction { Id = 1, OrderId = 1, Amount = 15100000m, TransactionDate = DateTime.UtcNow.AddDays(-45), Status = EPaymentStatus.Success, PaymentMethod = "MOMO", TransactionId = "MOMO123456789" },
                new PaymentTransaction { Id = 2, OrderId = 2, Amount = 53000000m, TransactionDate = DateTime.UtcNow.AddDays(-25), Status = EPaymentStatus.Success, PaymentMethod = "VNPAY", TransactionId = "VNPAY987654321" },
                new PaymentTransaction { Id = 3, OrderId = 3, Amount = 3200000m, TransactionDate = DateTime.UtcNow.AddDays(-10), Status = EPaymentStatus.Failed, PaymentMethod = "MOMO", TransactionId = "MOMOFAILED001" }
            );
        }
    }
}