using CommercialDocumentCreator.Classes.CommercialModels;
using Microsoft.EntityFrameworkCore;

namespace CommercialDocumentCreator.Classes.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //public DbSet<Product> Products { get; set; }

        public DbSet<CommercialInvoice> Invoices { get; set; }
        public DbSet<CommercialReceipt> Receipts { get; set; }
        public DbSet<CommercialQuotation> Quotations { get; set; }

        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ProductCategory> Categories { get; set; }
    }
}
