using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Final.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreTablesAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_UserId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Order_OrderId",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Products_ProductId",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brand_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Products_ProductId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_User_UserId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItem",
                table: "OrderItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brand",
                table: "Brand");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "OrderItem",
                newName: "OrderItems");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "Brand",
                newName: "Brands");

            migrationBuilder.RenameIndex(
                name: "IX_Review_UserId",
                table: "Reviews",
                newName: "IX_Reviews_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ProductId",
                table: "Reviews",
                newName: "IX_Reviews_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItem_ProductId",
                table: "OrderItems",
                newName: "IX_OrderItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItem_OrderId",
                table: "OrderItems",
                newName: "IX_OrderItems_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_UserId",
                table: "Orders",
                newName: "IX_Orders_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brands",
                table: "Brands",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Vi xử lý đầu bảng cho gaming và sáng tạo nội dung, 24 nhân 32 luồng, tốc độ tối đa 6.0 GHz.", "[\"https://placehold.co/600x600/EFEFEF/333?text=i9-14900K\"]", 50 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Lựa chọn tuyệt vời cho gaming hiệu năng cao, 20 nhân 28 luồng.", "[\"https://placehold.co/600x600/EFEFEF/333?text=i7-14700K\"]", 80 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Vua gaming với công nghệ 3D V-Cache, 16 nhân 32 luồng.", "[\"https://placehold.co/600x600/EFEFEF/333?text=R9-7950X3D\"]", 45 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Hiệu năng gaming thuần túy tốt nhất phân khúc nhờ 3D V-Cache, 8 nhân 16 luồng.", "[\"https://placehold.co/600x600/EFEFEF/333?text=R7-7800X3D\"]", 120 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Sức mạnh tối thượng cho gaming 4K và các tác vụ AI, Ray Tracing đỉnh cao.", "[\"https://placehold.co/600x600/1B3C34/FFF?text=RTX+4090\", \"https://placehold.co/600x600/CCC/333?text=RTX+4090+Side\"]", 25 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Hiệu năng vượt trội cho gaming 1440p và 4K, phiên bản nâng cấp của RTX 4080.", "[\"https://placehold.co/600x600/1B3C34/FFF?text=RTX+4080S\"]", 40 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(2024, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Card đồ họa đầu bảng của AMD, đối thủ cạnh tranh trực tiếp với RTX 4080.", "[\"https://placehold.co/600x600/BF0A30/FFF?text=RX+7900XTX\"]", 35 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lựa chọn p/p tốt nhất cho gaming 1440p.", "[\"https://placehold.co/600x600/BF0A30/FFF?text=RX+7800XT\"]", 90 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Kit RAM DDR5 hiệu năng cao, tản nhiệt nhôm, tương thích tốt với Intel XMP và AMD EXPO.", "[\"https://placehold.co/600x600/EFEFEF/333?text=Vengeance+DDR5\"]", 150 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(2024, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), "Thiết kế đẹp mắt với LED RGB, tốc độ bus cao dành cho người dùng chuyên nghiệp và game thủ.", "[\"https://placehold.co/600x600/EFEFEF/333?text=Trident+Z5\"]", 110 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(2024, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Ổ cứng NVMe Gen4 nhanh nhất thị trường, lý tưởng cho hệ điều hành, game và ứng dụng nặng.", "[\"https://placehold.co/600x600/EFEFEF/333?text=990+Pro+2TB\"]", 70 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(2024, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Tốc độ đọc ghi cực nhanh, lựa chọn hàng đầu của game thủ.", "[\"https://placehold.co/600x600/000/FFF?text=SN850X+2TB\"]", 85 });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "Comment", "CreatedAt", "ProductId", "Rating", "UserId" },
                values: new object[,]
                {
                    { 1L, "CPU quá mạnh, chạy đa nhiệm và render video cực nhanh. Rất đáng tiền!", new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1L, 5, 2L },
                    { 3L, "Đúng là trùm cuối card đồ họa. Chơi Cyberpunk 2077 max setting 4K mượt mà. Đắt nhưng xắt ra miếng.", new DateTime(2024, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), 5L, 5, 2L },
                    { 5L, "RAM chạy ổn định, cắm vào là nhận ngay, không gặp vấn đề gì.", new DateTime(2024, 3, 29, 0, 0, 0, 0, DateTimeKind.Utc), 9L, 5, 2L }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "$2a$11$9i.2nCqjA1DkC8B4lQ9C8uJ.Uj5GqXy.z/A7X2Q.Z9iB8qF.K/9W." });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "Email", "FirstName", "LastName", "PasswordHash" },
                values: new object[] { new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), "customer1@example.com", "An", "Nguyễn", "$2a$11$gT/jKqC0Z.I7H.v2Uu4j6u8kK/gL7Xy.Z/Q5F/E9Z/A9qG.H/3e." });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "PasswordHash", "Role" },
                values: new object[] { 3L, new DateTime(2024, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), "customer2@example.com", "Bình", "Trần", "$2a$11$gT/jKqC0Z.I7H.v2Uu4j6u8kK/gL7Xy.Z/Q5F/E9Z/A9qG.H/3e.", "Customer" });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "Comment", "CreatedAt", "ProductId", "Rating", "UserId" },
                values: new object[,]
                {
                    { 2L, "Hiệu năng chơi game đỉnh cao, không có gì để chê.", new DateTime(2024, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1L, 5, 3L },
                    { 4L, "Hiệu năng chơi game 2K rất tốt trong tầm giá. Chỉ có điều card hơi nóng một chút.", new DateTime(2024, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), 8L, 4, 3L }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brands",
                table: "Brands");

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                newName: "OrderItem");

            migrationBuilder.RenameTable(
                name: "Brands",
                newName: "Brand");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_UserId",
                table: "Review",
                newName: "IX_Review_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ProductId",
                table: "Review",
                newName: "IX_Review_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserId",
                table: "Order",
                newName: "IX_Order_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItem",
                newName: "IX_OrderItem_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItem",
                newName: "IX_OrderItem_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItem",
                table: "OrderItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brand",
                table: "Brand",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "CreatedAt", "Description", "ImagesJson", "StockQuantity" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 0 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BrandId", "CategoryId", "CreatedAt", "Description", "ImagesJson", "Name", "Price", "Sku", "StockQuantity" },
                values: new object[,]
                {
                    { 11L, 5L, 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Kingston Fury Beast DDR5 16GB 5200MHz", 1500000m, "RAM-KIN-F16GB", 0 },
                    { 12L, 9L, 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Crucial Pro DDR5 16GB 5600MHz", 1450000m, "RAM-CRU-P16GB", 0 },
                    { 15L, 9L, 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Crucial P5 Plus NVMe M.2 SSD 1TB", 2100000m, "SSD-CRU-P5P1TB", 0 },
                    { 16L, 5L, 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Kingston KC3000 NVMe M.2 SSD 1TB", 2300000m, "SSD-KIN-KC3K1TB", 0 },
                    { 17L, 4L, 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Corsair RM1000e 1000W 80+ Gold", 4100000m, "PSU-COR-RM1000E", 0 },
                    { 18L, 10L, 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Seasonic FOCUS Plus Gold 850W", 3400000m, "PSU-SEA-FP850W", 0 },
                    { 19L, 11L, 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Cooler Master MWE Gold 750 V2", 2500000m, "PSU-CM-MWE750", 0 },
                    { 20L, 12L, 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "MSI MPG A850G PCIE5 850W", 3600000m, "PSU-MSI-A850G", 0 }
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "123" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "Email", "FirstName", "LastName", "PasswordHash" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "customer@example.com", "Test", "Customer", "123" });

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Order_OrderId",
                table: "OrderItem",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Products_ProductId",
                table: "OrderItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brand_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Products_ProductId",
                table: "Review",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_User_UserId",
                table: "Review",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
