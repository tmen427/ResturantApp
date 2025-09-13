using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resturant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Thirdss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                table: "ShoppingCartItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                table: "ShoppingCartItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxRate",
                table: "ShoppingCartItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "ShoppingCartItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "SubTotal", "TaxAmount", "TaxRate" },
                values: new object[] { new DateTime(2025, 9, 13, 21, 43, 18, 408, DateTimeKind.Utc).AddTicks(9850), 0m, 0m, 0.06875m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "TaxRate",
                table: "ShoppingCartItems");

            migrationBuilder.UpdateData(
                table: "ShoppingCartItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 9, 11, 0, 39, 12, 905, DateTimeKind.Utc).AddTicks(2650));
        }
    }
}
