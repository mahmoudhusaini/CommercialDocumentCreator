using CommercialDocumentCreator.Enums;
using System.ComponentModel.DataAnnotations;

namespace CommercialDocumentCreator.Classes.CommercialModels
{

    public class CommercialDocument
    {
        [Key]
        public int Id { get; protected set; }
        public string DocumentNumber { get; set; } = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        public PaperType PaperType { get; protected set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string? ClientName { get; set; }
        public DateTime ValidityDate { get; protected set; }
        public decimal? Rate { get; set; }
        public bool IsPayable { get; protected set; }
        public string? Warranty { get; set; }
        public int? DeliveryDelay { get; set; }
        public PaymentStatus Status { get; set; }
        public double? TotalAmount { get; set; }
        public double? CashDeposit { get; set; }
        public double? RemainingBalance { get; set; }
        public string? ProductsPath { get; set; }
    }
}
