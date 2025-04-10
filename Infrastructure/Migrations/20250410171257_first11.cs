using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resturant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class first11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemsVO_TemporaryCartItems_TemporaryCartItemsId",
                table: "MenuItemsVO");

            migrationBuilder.AlterColumn<int>(
                name: "TemporaryCartItemsId",
                table: "MenuItemsVO",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemsVO_TemporaryCartItems_TemporaryCartItemsId",
                table: "MenuItemsVO",
                column: "TemporaryCartItemsId",
                principalTable: "TemporaryCartItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemsVO_TemporaryCartItems_TemporaryCartItemsId",
                table: "MenuItemsVO");

            migrationBuilder.AlterColumn<int>(
                name: "TemporaryCartItemsId",
                table: "MenuItemsVO",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemsVO_TemporaryCartItems_TemporaryCartItemsId",
                table: "MenuItemsVO",
                column: "TemporaryCartItemsId",
                principalTable: "TemporaryCartItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
