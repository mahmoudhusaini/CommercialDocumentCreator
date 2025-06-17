using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommercialDocumentCreator.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceColsX1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductsPath",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RemainingBalance",
                table: "Invoices",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductsPath",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "RemainingBalance",
                table: "Invoices");
        }
    }
}
