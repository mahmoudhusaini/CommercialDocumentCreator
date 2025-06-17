using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommercialDocumentCreator.Migrations
{
    /// <inheritdoc />
    public partial class RemoveInvoicesDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashDeposit = table.Column<double>(type: "float", nullable: true),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryDelay = table.Column<int>(type: "int", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPayable = table.Column<bool>(type: "bit", nullable: false),
                    IsPosted = table.Column<bool>(type: "bit", nullable: false),
                    PaperType = table.Column<int>(type: "int", nullable: false),
                    ProductsPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate = table.Column<float>(type: "real", nullable: true),
                    RemainingBalance = table.Column<double>(type: "float", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<double>(type: "float", nullable: true),
                    ValidityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Warranty = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                });
        }
    }
}
