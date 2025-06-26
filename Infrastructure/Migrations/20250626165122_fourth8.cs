using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resturant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fourth8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_CustomerInformation_CustomerInformationId",
                table: "ShoppingCartItems");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerInformationId",
                table: "ShoppingCartItems",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_CustomerInformation_CustomerInformationId",
                table: "ShoppingCartItems",
                column: "CustomerInformationId",
                principalTable: "CustomerInformation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_CustomerInformation_CustomerInformationId",
                table: "ShoppingCartItems");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerInformationId",
                table: "ShoppingCartItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_CustomerInformation_CustomerInformationId",
                table: "ShoppingCartItems",
                column: "CustomerInformationId",
                principalTable: "CustomerInformation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
