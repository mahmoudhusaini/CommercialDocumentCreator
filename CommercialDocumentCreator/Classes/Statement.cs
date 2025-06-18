using CommercialDocumentCreator.Classes.CommercialModels;

namespace CommercialDocumentCreator.Classes
{
    public class Statement
    {
        public Statement(DateTime startDate, DateTime endDate)
        {
            this.From = DateOnly.FromDateTime(startDate);
            this.To = DateOnly.FromDateTime(endDate);
        }
        public DateOnly Date => DateOnly.FromDateTime(DateTime.Now);
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }

        public List<CommercialInvoice>? Invoices { get; set; }

        public List<CommercialReceipt>? Receipts { get; set; }

        public double? CashFromReceipts { get; set; } = 0;

        public double? DepositFromInvoice { get; set; } = 0;
        public double? PendingFromInvoice { get; set; } = 0;
        public double? TotalFromInvoice { get; set; } = 0;

        public double? TotalIncome => this.CashFromReceipts + this.DepositFromInvoice;

        public string? Template { get; set; }
        public string? DetailedTemplate { get; set; }


    }
}
