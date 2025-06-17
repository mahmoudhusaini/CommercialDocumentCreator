using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommercialDocumentCreator.Migrations
{
    /// <inheritdoc />
    public partial class PaperType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPosted",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ProductsPath",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "RemainingBalance",
                table: "Invoices");

            migrationBuilder.AddColumn<int>(
                name: "PaperType",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaperType",
                table: "Invoices");

            migrationBuilder.AddColumn<bool>(
                name: "IsPosted",
                table: "Invoices",
                type: "bit",
                nullable: true);

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
    }
}
