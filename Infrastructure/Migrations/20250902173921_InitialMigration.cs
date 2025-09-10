using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Resturant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_CustomerInformation_CustomerInformationId",
                table: "ShoppingCartItems");

            migrationBuilder.DropTable(
                name: "CustomerInformation");

            migrationBuilder.CreateTable(
                name: "CustomerPaymentInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Credit = table.Column<string>(type: "text", nullable: true),
                    NameonCard = table.Column<string>(type: "text", nullable: true),
                    CreditCardNumber = table.Column<string>(type: "text", nullable: true),
                    Expiration = table.Column<string>(type: "text", nullable: true),
                    CVV = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    Paid = table.Column<bool>(type: "boolean", nullable: false),
                    TempCartsIdentity = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPaymentInformation", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "ShoppingCartItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 9, 2, 17, 39, 21, 495, DateTimeKind.Utc).AddTicks(1770));

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_CustomerPaymentInformation_CustomerInform~",
                table: "ShoppingCartItems",
                column: "CustomerInformationId",
                principalTable: "CustomerPaymentInformation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_CustomerPaymentInformation_CustomerInform~",
                table: "ShoppingCartItems");

            migrationBuilder.DropTable(
                name: "CustomerPaymentInformation");

            migrationBuilder.CreateTable(
                name: "CustomerInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CVV = table.Column<string>(type: "text", nullable: true),
                    Credit = table.Column<string>(type: "text", nullable: true),
                    CreditCardNumber = table.Column<string>(type: "text", nullable: true),
                    Expiration = table.Column<string>(type: "text", nullable: true),
                    NameonCard = table.Column<string>(type: "text", nullable: true),
                    Paid = table.Column<bool>(type: "boolean", nullable: false),
                    TempCartsIdentity = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInformation", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "ShoppingCartItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 6, 27, 17, 38, 10, 800, DateTimeKind.Utc).AddTicks(8050));

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_CustomerInformation_CustomerInformationId",
                table: "ShoppingCartItems",
                column: "CustomerInformationId",
                principalTable: "CustomerInformation",
                principalColumn: "Id");
        }
    }
}
