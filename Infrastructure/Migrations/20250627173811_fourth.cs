using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Resturant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fourth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookingInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<string>(type: "text", nullable: true),
                    Time = table.Column<string>(type: "text", nullable: true),
                    People = table.Column<int>(type: "integer", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerInformation",
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
                    table.PrimaryKey("PK_CustomerInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerInquiryInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Subject = table.Column<string>(type: "text", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInquiryInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StreamId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Identity = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    CustomerInformationId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartItems", x => x.Id);
                    table.UniqueConstraint("AK_ShoppingCartItems_Identity", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_CustomerInformation_CustomerInformationId",
                        column: x => x.CustomerInformationId,
                        principalTable: "CustomerInformation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    ShoppingCartItemsIdentity = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItems_ShoppingCartItems_ShoppingCartItemsIdentity",
                        column: x => x.ShoppingCartItemsIdentity,
                        principalTable: "ShoppingCartItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ShoppingCartItems",
                columns: new[] { "Id", "Created", "CustomerInformationId", "Identity", "TotalPrice" },
                values: new object[] { 1, new DateTime(2025, 6, 27, 17, 38, 10, 800, DateTimeKind.Utc).AddTicks(8050), null, new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"), 0m });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Name", "Price", "ShoppingCartItemsIdentity" },
                values: new object[,]
                {
                    { 1, "Egg Roll Platter", 14.95m, new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6") },
                    { 2, "Papaya Salad", 8.95m, new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6") },
                    { 3, "Tofu", 10.5m, new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6") },
                    { 4, "Chopped Beef", 12.95m, new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6") },
                    { 5, "Veggie Platter", 8.95m, new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_ShoppingCartItemsIdentity",
                table: "MenuItems",
                column: "ShoppingCartItemsIdentity");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_CustomerInformationId",
                table: "ShoppingCartItems",
                column: "CustomerInformationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingInformation");

            migrationBuilder.DropTable(
                name: "CustomerInquiryInformation");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "ShoppingCartItems");

            migrationBuilder.DropTable(
                name: "CustomerInformation");
        }
    }
}
