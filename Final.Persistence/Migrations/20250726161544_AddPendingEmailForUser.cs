using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPendingEmailForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PendingRecoveryEmail",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1L,
                column: "OrderDate",
                value: new DateTime(2025, 6, 11, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7691));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2L,
                column: "OrderDate",
                value: new DateTime(2025, 7, 1, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7696));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 3L,
                column: "OrderDate",
                value: new DateTime(2025, 7, 16, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7698));

            migrationBuilder.UpdateData(
                table: "PaymentTransactions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "TransactionDate",
                value: new DateTime(2025, 6, 11, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7769));

            migrationBuilder.UpdateData(
                table: "PaymentTransactions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "TransactionDate",
                value: new DateTime(2025, 7, 1, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7773));

            migrationBuilder.UpdateData(
                table: "PaymentTransactions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "TransactionDate",
                value: new DateTime(2025, 7, 16, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7775));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 17, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7473));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 27, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7483));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 7, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7485));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 28, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7487));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 7, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7490));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 7, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7493));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 26, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7531));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 8, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7533));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 27, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7535));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 6, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7538));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 27, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7540));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 6, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7542));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 16, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7544));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 26, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7546));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 7, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7616));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 17, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7619));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 27, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7621));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 16, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7622));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 6, 16, 15, 44, 78, DateTimeKind.Utc).AddTicks(7624));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PendingRecoveryEmail",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                column: "PendingRecoveryEmail",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                column: "PendingRecoveryEmail",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4L,
                column: "PendingRecoveryEmail",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5L,
                column: "PendingRecoveryEmail",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6L,
                column: "PendingRecoveryEmail",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7L,
                column: "PendingRecoveryEmail",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PendingRecoveryEmail",
                table: "Users");

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
        }
    }
}
