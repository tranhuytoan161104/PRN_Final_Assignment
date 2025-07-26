using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRecorveryFeature02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpiry",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1L,
                column: "OrderDate",
                value: new DateTime(2025, 6, 11, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5288));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2L,
                column: "OrderDate",
                value: new DateTime(2025, 7, 1, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5291));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 3L,
                column: "OrderDate",
                value: new DateTime(2025, 7, 16, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5293));

            migrationBuilder.UpdateData(
                table: "PaymentTransactions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "TransactionDate",
                value: new DateTime(2025, 6, 11, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5380));

            migrationBuilder.UpdateData(
                table: "PaymentTransactions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "TransactionDate",
                value: new DateTime(2025, 7, 1, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5383));

            migrationBuilder.UpdateData(
                table: "PaymentTransactions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "TransactionDate",
                value: new DateTime(2025, 7, 16, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5384));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 17, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5132));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 27, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5147));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 7, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5149));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 28, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5151));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 7, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5153));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 7, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5154));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 26, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5156));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 8, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5158));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 27, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5160));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 6, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5163));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 27, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5165));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 6, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5167));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 16, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5168));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 26, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5170));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 7, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5225));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 17, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5227));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 27, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5228));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 16, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5229));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 6, 10, 42, 29, 498, DateTimeKind.Utc).AddTicks(5231));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordResetToken", "ResetTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "PasswordResetToken", "ResetTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "PasswordResetToken", "ResetTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "PasswordResetToken", "ResetTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "PasswordResetToken", "ResetTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "PasswordResetToken", "ResetTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "PasswordResetToken", "ResetTokenExpiry" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpiry",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1L,
                column: "OrderDate",
                value: new DateTime(2025, 6, 11, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(597));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2L,
                column: "OrderDate",
                value: new DateTime(2025, 7, 1, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(600));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 3L,
                column: "OrderDate",
                value: new DateTime(2025, 7, 16, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(602));

            migrationBuilder.UpdateData(
                table: "PaymentTransactions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "TransactionDate",
                value: new DateTime(2025, 6, 11, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(671));

            migrationBuilder.UpdateData(
                table: "PaymentTransactions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "TransactionDate",
                value: new DateTime(2025, 7, 1, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(675));

            migrationBuilder.UpdateData(
                table: "PaymentTransactions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "TransactionDate",
                value: new DateTime(2025, 7, 16, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(701));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 17, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(424));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 27, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(441));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 7, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(443));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 28, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(444));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 7, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(446));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 7, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(448));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 26, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(450));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 8, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(452));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 27, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(454));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 6, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(456));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 27, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(457));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 6, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(459));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 16, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(461));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 26, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(463));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 7, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(527));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 17, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(529));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 27, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(530));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 16, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(532));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 6, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(533));
        }
    }
}
