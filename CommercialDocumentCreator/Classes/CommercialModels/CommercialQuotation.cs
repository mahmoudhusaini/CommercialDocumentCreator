using CommercialDocumentCreator.Enums;

namespace CommercialDocumentCreator.Classes.CommercialModels
{
    public class CommercialQuotation: CommercialDocument
    {
        public CommercialQuotation()
        {
            PaperType = PaperType.Quotation;
            ValidityDate = CreationDate.AddDays(45);
            IsPayable = false;
            Status = PaymentStatus.NoStatus;
            CashDeposit = null;
            RemainingBalance = null;
            ProductsPath = $"wwwroot\\server-resources\\CommercialDocuments\\Quotations";
        }
    }
}
