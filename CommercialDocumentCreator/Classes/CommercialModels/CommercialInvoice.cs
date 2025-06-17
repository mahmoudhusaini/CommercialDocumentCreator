using CommercialDocumentCreator.Enums;
using System.ComponentModel.DataAnnotations;

namespace CommercialDocumentCreator.Classes.CommercialModels
{
    public class CommercialInvoice: CommercialDocument
    {
        public CommercialInvoice()
        {
            PaperType = PaperType.Invoice;
            ValidityDate = CreationDate.AddDays(45);
            IsPayable = true;
            Status = PaymentStatus.Unpaid;
            ProductsPath = $"wwwroot\\server-resources\\CommercialDocuments\\Invoices";
        }
        public bool IsPosted { get; set; } = false;

    }
}



//    public class CommercialInvoice
//    {
//        [Key]
//        public int Id { get; set; }
//        public string InvoiceNumber { get; set; } = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
//        public PaperType PaperType { get; set; } = PaperType.Invoice;
//        public DateTime CreationDate { get; set; } = DateTime.Now;
//        public string? ClientName { get; set; }
//        public DateTime ValidityDate { get; set; } = DateTime.Now.AddDays(45);
//        public float? Rate { get; set; }
//        public bool IsPayable { get; set; } = true;
//        public string? Warranty { get; set; }
//        public int? DeliveryDelay { get; set; }
//        public PaymentStatus Status { get; set; } = PaymentStatus.Unpaid;
//        public double? TotalAmount { get; set; }
//        public double? CashDeposit { get; set; }

//        public double? RemainingBalance { get; set; }

//        //[NotMapped]
//        //public double? RemainingBalance => this.TotalAmount - this.CashDeposit;

//        public bool IsPosted { get; set; } = false;

//        public string ProductsPath { get; set; } = $"wwwroot\\server-resources\\CommercialDocuments\\Invoices";
//    }
