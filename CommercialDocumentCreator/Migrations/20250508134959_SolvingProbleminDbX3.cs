using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommercialDocumentCreator.Migrations
{
    /// <inheritdoc />
    public partial class SolvingProbleminDbX3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductsPath",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductsPath",
                table: "Invoices");
        }
    }
}
