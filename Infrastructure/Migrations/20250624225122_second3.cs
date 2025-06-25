using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resturant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class second3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactsCustomerInquiryInformation",
                table: "ContactsCustomerInquiryInformation");

            migrationBuilder.RenameTable(
                name: "ContactsCustomerInquiryInformation",
                newName: "CustomerInquiryInformation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerInquiryInformation",
                table: "CustomerInquiryInformation",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerInquiryInformation",
                table: "CustomerInquiryInformation");

            migrationBuilder.RenameTable(
                name: "CustomerInquiryInformation",
                newName: "ContactsCustomerInquiryInformation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactsCustomerInquiryInformation",
                table: "ContactsCustomerInquiryInformation",
                column: "Id");
        }
    }
}
