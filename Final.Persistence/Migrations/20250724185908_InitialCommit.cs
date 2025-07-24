using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Final.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ShoppingCartId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Intel" },
                    { 2L, "AMD" },
                    { 3L, "NVIDIA" },
                    { 4L, "Corsair" },
                    { 5L, "Kingston" },
                    { 6L, "Samsung" },
                    { 7L, "Western Digital" },
                    { 8L, "G.Skill" },
                    { 9L, "Crucial" },
                    { 10L, "Seasonic" },
                    { 11L, "Cooler Master" },
                    { 12L, "MSI" },
                    { 13L, "ASUS" },
                    { 14L, "Gigabyte" },
                    { 15L, "LG" },
                    { 16L, "Dell" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Vi xử lý (CPU)" },
                    { 2L, "Card đồ họa (GPU)" },
                    { 3L, "Bộ nhớ RAM" },
                    { 4L, "Ổ cứng SSD" },
                    { 5L, "Nguồn máy tính (PSU)" },
                    { 6L, "Bo mạch chủ (Mainboard)" },
                    { 7L, "Màn hình (Monitor)" }
                });

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "Id", "Code", "IsActive" },
                values: new object[,]
                {
                    { 1L, "COD", true },
                    { 2L, "MOMO", true },
                    { 3L, "VNPAY", true },
                    { 4L, "BANK_TRANSFER", false }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "PasswordHash", "Role", "Status" },
                values: new object[,]
                {
                    { 1L, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "owner@final.com", "Văn", "Toàn", "$2a$11$N1brDk6.a9UHivirpPppuuV30cywfm.PCZIOdKoe6RPb1zfVdjlM2", "Owner", 0 },
                    { 2L, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@final.com", "Quốc", "Tuấn", "$2a$11$N1brDk6.a9UHivirpPppuuV30cywfm.PCZIOdKoe6RPb1zfVdjlM2", "Admin", 0 },
                    { 3L, new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "admin2@final.com", "Thanh", "Bình", "$2a$11$N1brDk6.a9UHivirpPppuuV30cywfm.PCZIOdKoe6RPb1zfVdjlM2", "Admin", 1 },
                    { 4L, new DateTime(2023, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "minhan@customer.com", "Minh", "An", "$2a$11$N1brDk6.a9UHivirpPppuuV30cywfm.PCZIOdKoe6RPb1zfVdjlM2", "Customer", 0 },
                    { 5L, new DateTime(2023, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), "baochau@customer.com", "Bảo", "Châu", "$2a$11$N1brDk6.a9UHivirpPppuuV30cywfm.PCZIOdKoe6RPb1zfVdjlM2", "Customer", 0 },
                    { 6L, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "giahan@customer.com", "Gia", "Hân", "$2a$11$N1brDk6.a9UHivirpPppuuV30cywfm.PCZIOdKoe6RPb1zfVdjlM2", "Customer", 0 },
                    { 7L, new DateTime(2023, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), "dangkhoa@customer.com", "Đăng", "Khoa", "$2a$11$N1brDk6.a9UHivirpPppuuV30cywfm.PCZIOdKoe6RPb1zfVdjlM2", "Customer", 0 }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "OrderDate", "PhoneNumber", "ShippingAddress", "Status", "TotalAmount", "UserId" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 6, 9, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3804), "0901234567", "123 Đường ABC, Quận 1, TP.HCM", "Delivered", 15100000m, 4L },
                    { 2L, new DateTime(2025, 6, 29, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3807), "0987654321", "456 Đường XYZ, Quận Hoàn Kiếm, Hà Nội", "Shipped", 53000000m, 7L },
                    { 3L, new DateTime(2025, 7, 14, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3809), "0901234567", "123 Đường ABC, Quận 1, TP.HCM", "Cancelled", 3200000m, 4L }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AddAt", "BrandId", "CategoryId", "CreatedAt", "Description", "Name", "Price", "Status", "StockQuantity", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, 1L, new DateTime(2025, 4, 15, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3445), "Vi xử lý đầu bảng cho gaming và sáng tạo nội dung.", "Intel Core i9-14900K", 15500000m, "Available", 50, null },
                    { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, 1L, new DateTime(2025, 4, 25, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3454), "Hiệu năng gaming thuần túy tốt nhất phân khúc nhờ 3D V-Cache.", "AMD Ryzen 7 7800X3D", 9800000m, "Available", 120, null },
                    { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, 1L, new DateTime(2025, 5, 5, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3456), "Vi xử lý tầm trung p/p tốt nhất cho gaming.", "Intel Core i5-14600K", 8500000m, "OutOfStock", 0, null },
                    { 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3L, 2L, new DateTime(2025, 3, 26, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3458), "Sức mạnh tối thượng cho gaming 4K và các tác vụ AI.", "NVIDIA GeForce RTX 4090", 45000000m, "Available", 25, null },
                    { 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, 2L, new DateTime(2025, 4, 5, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3460), "Card đồ họa đầu bảng của AMD, đối thủ cạnh tranh trực tiếp với RTX 4080.", "AMD Radeon RX 7900 XTX", 28500000m, "Available", 35, null },
                    { 6L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 14L, 2L, new DateTime(2025, 1, 5, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3461), "Card đồ họa quốc dân cho gaming Full HD.", "Gigabyte RTX 3060 Gaming OC", 8200000m, "Archived", 200, null },
                    { 7L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4L, 3L, new DateTime(2025, 2, 24, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3463), "Kit RAM DDR5 hiệu năng cao, tản nhiệt nhôm.", "Corsair Vengeance DDR5 32GB 6000MHz", 3200000m, "Available", 150, null },
                    { 8L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8L, 3L, new DateTime(2025, 3, 6, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3465), "Thiết kế đẹp mắt với LED RGB, tốc độ bus cao.", "G.Skill Trident Z5 RGB DDR5 32GB 6400MHz", 3800000m, "Available", 110, null },
                    { 9L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6L, 4L, new DateTime(2025, 1, 25, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3467), "Ổ cứng NVMe Gen4 nhanh nhất thị trường.", "Samsung 990 Pro NVMe M.2 SSD 2TB", 4500000m, "Available", 70, null },
                    { 10L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7L, 4L, new DateTime(2025, 2, 4, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3468), "Tốc độ đọc ghi cực nhanh, lựa chọn hàng đầu của game thủ.", "WD Black SN850X NVMe M.2 SSD 1TB", 2600000m, "Available", 95, null },
                    { 11L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13L, 6L, new DateTime(2025, 5, 25, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3470), "Bo mạch chủ cao cấp cho CPU Intel thế hệ 14.", "ASUS ROG STRIX Z790-E GAMING WIFI II", 16000000m, "Available", 40, null },
                    { 12L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12L, 6L, new DateTime(2025, 6, 4, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3472), "Bo mạch chủ tầm trung tốt nhất cho nhu cầu gaming.", "MSI MAG B760M MORTAR WIFI DDR5", 5300000m, "Available", 80, null },
                    { 13L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 15L, 7L, new DateTime(2025, 6, 14, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3473), "Màn hình OLED 2K 240Hz cho trải nghiệm gaming đỉnh cao.", "LG UltraGear 27GR95QE-B 240Hz OLED", 24500000m, "Available", 30, null },
                    { 14L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 16L, 7L, new DateTime(2025, 6, 24, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3475), "Màn hình 4K chuyên đồ họa với tấm nền IPS Black.", "Dell UltraSharp U2723QE 4K IPS", 13800000m, "Available", 60, null }
                });

            migrationBuilder.InsertData(
                table: "ShoppingCarts",
                columns: new[] { "Id", "UserId" },
                values: new object[,]
                {
                    { 1L, 5L },
                    { 2L, 6L }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "OrderId", "Price", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { 1L, 1L, 9800000m, 2L, 1 },
                    { 2L, 1L, 5300000m, 12L, 1 },
                    { 3L, 2L, 28500000m, 5L, 1 },
                    { 4L, 2L, 24500000m, 13L, 1 },
                    { 5L, 3L, 3200000m, 7L, 1 }
                });

            migrationBuilder.InsertData(
                table: "PaymentTransactions",
                columns: new[] { "Id", "Amount", "OrderId", "PaymentMethod", "Status", "TransactionDate", "TransactionId" },
                values: new object[,]
                {
                    { 1L, 15100000m, 1L, "MOMO", "Success", new DateTime(2025, 6, 9, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3882), "MOMO123456789" },
                    { 2L, 53000000m, 2L, "VNPAY", "Success", new DateTime(2025, 6, 29, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3886), "VNPAY987654321" },
                    { 3L, 3200000m, 3L, "MOMO", "Failed", new DateTime(2025, 7, 14, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3888), "MOMOFAILED001" }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "ImageUrl", "ProductId" },
                values: new object[,]
                {
                    { 1L, "https://placehold.co/600x600/EFEFEF/333?text=i9-14900K-1", 1L },
                    { 2L, "https://placehold.co/600x600/EFEFEF/333?text=i9-14900K-2", 1L },
                    { 3L, "https://placehold.co/600x600/EFEFEF/333?text=7800X3D", 2L },
                    { 4L, "https://placehold.co/600x600/1B3C34/FFF?text=RTX+4090", 4L },
                    { 5L, "https://placehold.co/600x600/E83131/FFF?text=RX+7900XTX", 5L },
                    { 6L, "https://placehold.co/600x600/111/EEE?text=Corsair+RAM", 7L },
                    { 7L, "https://placehold.co/600x600/0078D4/FFF?text=Samsung+SSD", 9L },
                    { 8L, "https://placehold.co/600x600/D82727/FFF?text=ASUS+ROG", 11L },
                    { 9L, "https://placehold.co/600x600/333/FFF?text=LG+OLED", 13L },
                    { 10L, "https://placehold.co/600x600/333/FFF?text=LG+OLED-2", 13L }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "Comment", "CreatedAt", "ProductId", "Rating", "UserId" },
                values: new object[,]
                {
                    { 1L, "CPU gaming tốt nhất hiện tại, không có gì để chê!", new DateTime(2025, 5, 5, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3699), 2L, 5, 4L },
                    { 2L, "Đắt nhưng xắt ra miếng. Cân mọi game 4K max setting.", new DateTime(2025, 5, 15, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3701), 4L, 5, 5L },
                    { 3L, "Tốc độ rất nhanh, nhưng giá hơi cao so với các hãng khác.", new DateTime(2025, 5, 25, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3702), 10L, 4, 6L },
                    { 4L, "Mainboard p/p quá tốt, đầy đủ cổng kết nối.", new DateTime(2025, 6, 14, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3704), 12L, 5, 4L },
                    { 5L, "Màu sắc và tần số quét của màn hình này thật sự tuyệt vời.", new DateTime(2025, 7, 4, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3705), 13L, 5, 7L }
                });

            migrationBuilder.InsertData(
                table: "ShoppingCartItems",
                columns: new[] { "Id", "ProductId", "Quantity", "ShoppingCartId" },
                values: new object[,]
                {
                    { 1L, 8L, 1, 1L },
                    { 2L, 10L, 1, 1L },
                    { 3L, 14L, 2, 2L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_Code",
                table: "PaymentMethods",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_OrderId",
                table: "PaymentTransactions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_ProductId",
                table: "ShoppingCartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_ShoppingCartId",
                table: "ShoppingCartItems",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_UserId",
                table: "ShoppingCarts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "PaymentTransactions");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "ShoppingCartItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
