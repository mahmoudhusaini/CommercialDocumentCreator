using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommercialDocumentCreator.Migrations
{
    /// <inheritdoc />
    public partial class SolvingProbleminDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPosted",
                table: "Invoices",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPosted",
                table: "Invoices");
        }
    }
}
