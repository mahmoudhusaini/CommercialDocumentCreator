using CommercialDocumentCreator.Classes.CommercialModels;
using CommercialDocumentCreator.Enums;

namespace CommercialDocumentCreator.Classes.CommercialModels
{
    public class CommercialReceipt : CommercialDocument
    {
        public CommercialReceipt()
        {
            PaperType = PaperType.Receipt;
            ValidityDate = CreationDate.AddDays(45);
            IsPayable = true;
            ProductsPath = $"wwwroot\\server-resources\\CommercialDocuments\\Receipts";
            RemainingBalance = 0;
            Status = PaymentStatus.PaidCompletely;
        }

    }
}

