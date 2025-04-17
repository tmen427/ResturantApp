using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resturant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seven1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "OrderInformation",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paid",
                table: "OrderInformation");
        }
    }
}
