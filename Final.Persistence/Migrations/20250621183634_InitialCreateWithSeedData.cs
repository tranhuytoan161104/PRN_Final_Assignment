using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Final.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "CPU (Vi xử lý)" },
                    { 2L, "GPU (Card đồ họa)" },
                    { 3L, "RAM (Bộ nhớ trong)" },
                    { 4L, "SSD (Ổ cứng thể rắn)" },
                    { 5L, "PSU (Nguồn máy tính)" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Name", "Price" },
                values: new object[,]
                {
                    { 1L, 1L, "Intel Core i9-14900K", 15500000m },
                    { 2L, 1L, "Intel Core i7-14700K", 11200000m },
                    { 3L, 1L, "AMD Ryzen 9 7950X3D", 14800000m },
                    { 4L, 1L, "AMD Ryzen 7 7800X3D", 9800000m },
                    { 5L, 2L, "NVIDIA GeForce RTX 4090", 45000000m },
                    { 6L, 2L, "NVIDIA GeForce RTX 4080 Super", 31000000m },
                    { 7L, 2L, "AMD Radeon RX 7900 XTX", 28500000m },
                    { 8L, 2L, "AMD Radeon RX 7800 XT", 15000000m },
                    { 9L, 3L, "Corsair Vengeance DDR5 32GB (2x16GB) 6000MHz", 3200000m },
                    { 10L, 3L, "G.Skill Trident Z5 RGB DDR5 32GB (2x16GB) 6400MHz", 3800000m },
                    { 11L, 3L, "Kingston Fury Beast DDR5 16GB 5200MHz", 1500000m },
                    { 12L, 3L, "Crucial Pro DDR5 16GB 5600MHz", 1450000m },
                    { 13L, 4L, "Samsung 990 Pro NVMe M.2 SSD 2TB", 4500000m },
                    { 14L, 4L, "WD Black SN850X NVMe M.2 SSD 2TB", 4200000m },
                    { 15L, 4L, "Crucial P5 Plus NVMe M.2 SSD 1TB", 2100000m },
                    { 16L, 4L, "Kingston KC3000 NVMe M.2 SSD 1TB", 2300000m },
                    { 17L, 5L, "Corsair RM1000e 1000W 80+ Gold", 4100000m },
                    { 18L, 5L, "Seasonic FOCUS Plus Gold 850W", 3400000m },
                    { 19L, 5L, "Cooler Master MWE Gold 750 V2", 2500000m },
                    { 20L, 5L, "MSI MPG A850G PCIE5 850W", 3600000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
