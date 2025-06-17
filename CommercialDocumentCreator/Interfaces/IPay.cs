using CommercialDocumentCreator.Classes.CommercialModels;
using CommercialDocumentCreator.Enums;

namespace CommercialDocumentCreator.Interfaces
{
    public interface IPay
    {
        void CheckPayment(CommercialDocument commercialDocument, PaymentStatus? paymentStatus);
    }
}
