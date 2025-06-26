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
                name: "FK_MenuItems_ShoppingCartItems_ShoppingCartItemsId",
                table: "MenuItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_ShoppingCartItems_ShoppingCartItemsId1",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_ShoppingCartItemsId",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_ShoppingCartItemsId1",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "ShoppingCartItemsId",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "ShoppingCartItemsId1",
                table: "MenuItems");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ShoppingCartItems_Identity",
                table: "ShoppingCartItems",
                column: "Identity");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_ShoppingCartItemsIdentity",
                table: "MenuItems",
                column: "ShoppingCartItemsIdentity");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_ShoppingCartItems_ShoppingCartItemsIdentity",
                table: "MenuItems",
                column: "ShoppingCartItemsIdentity",
                principalTable: "ShoppingCartItems",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_ShoppingCartItems_ShoppingCartItemsIdentity",
                table: "MenuItems");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ShoppingCartItems_Identity",
                table: "ShoppingCartItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_ShoppingCartItemsIdentity",
                table: "MenuItems");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartItemsId",
                table: "MenuItems",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartItemsId1",
                table: "MenuItems",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_ShoppingCartItemsId",
                table: "MenuItems",
                column: "ShoppingCartItemsId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_ShoppingCartItemsId1",
                table: "MenuItems",
                column: "ShoppingCartItemsId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_ShoppingCartItems_ShoppingCartItemsId",
                table: "MenuItems",
                column: "ShoppingCartItemsId",
                principalTable: "ShoppingCartItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_ShoppingCartItems_ShoppingCartItemsId1",
                table: "MenuItems",
                column: "ShoppingCartItemsId1",
                principalTable: "ShoppingCartItems",
                principalColumn: "Id");
        }
    }
}
