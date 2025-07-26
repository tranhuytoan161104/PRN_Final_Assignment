using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRecorvery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRecoveryEmailVerified",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RecoveryEmail",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecurityAnswerHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecurityQuestion",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "PaymentTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
                columns: new[] { "OrderDate", "Status" },
                values: new object[] { new DateTime(2025, 7, 1, 10, 38, 17, 99, DateTimeKind.Utc).AddTicks(600), "Processing" });

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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "IsRecoveryEmailVerified", "RecoveryEmail", "SecurityAnswerHash", "SecurityQuestion" },
                values: new object[] { false, null, null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "IsRecoveryEmailVerified", "RecoveryEmail", "SecurityAnswerHash", "SecurityQuestion" },
                values: new object[] { false, null, null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "IsRecoveryEmailVerified", "RecoveryEmail", "SecurityAnswerHash", "SecurityQuestion" },
                values: new object[] { false, null, null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "IsRecoveryEmailVerified", "RecoveryEmail", "SecurityAnswerHash", "SecurityQuestion" },
                values: new object[] { false, null, null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "IsRecoveryEmailVerified", "RecoveryEmail", "SecurityAnswerHash", "SecurityQuestion" },
                values: new object[] { false, null, null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "IsRecoveryEmailVerified", "RecoveryEmail", "SecurityAnswerHash", "SecurityQuestion" },
                values: new object[] { false, null, null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "IsRecoveryEmailVerified", "RecoveryEmail", "SecurityAnswerHash", "SecurityQuestion" },
                values: new object[] { false, null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRecoveryEmailVerified",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RecoveryEmail",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecurityAnswerHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecurityQuestion",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "PaymentTransactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1L,
                column: "OrderDate",
                value: new DateTime(2025, 6, 9, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3804));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "OrderDate", "Status" },
                values: new object[] { new DateTime(2025, 6, 29, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3807), "Shipped" });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 3L,
                column: "OrderDate",
                value: new DateTime(2025, 7, 14, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3809));

            migrationBuilder.UpdateData(
                table: "PaymentTransactions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "TransactionDate",
                value: new DateTime(2025, 6, 9, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3882));

            migrationBuilder.UpdateData(
                table: "PaymentTransactions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "TransactionDate",
                value: new DateTime(2025, 6, 29, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3886));

            migrationBuilder.UpdateData(
                table: "PaymentTransactions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "TransactionDate",
                value: new DateTime(2025, 7, 14, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3888));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 15, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3445));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 25, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3454));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 5, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3456));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 26, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3458));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 5, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3460));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 5, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3461));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 24, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3463));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2025, 3, 6, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3465));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 25, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3467));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 4, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3468));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 25, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3470));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 4, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3472));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 14, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3473));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 24, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3475));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 5, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3699));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 15, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3701));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2025, 5, 25, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3702));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 14, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3704));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 18, 59, 7, 820, DateTimeKind.Utc).AddTicks(3705));
        }
    }
}
