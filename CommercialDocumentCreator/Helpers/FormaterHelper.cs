using CommercialDocumentCreator.Classes;
using CommercialDocumentCreator.Enums;

namespace CommercialDocumentCreator.Helpers
{
    public static class FormaterHelper
    {
        public static string DateFormater(DateTime date)
        {
            return $"{date.Month}/{date.Day}/{date.Year}";
        }
        public static PaperType PaperTypeConverter(string type)
        {
            return type switch
            {
                "quotation" => PaperType.Quotation,
                "invoice" => PaperType.Invoice,
                "receipt" => PaperType.Receipt,
                "deliveryNote" => PaperType.DeliveryNote,
                _ => PaperType.DeliveryNote
            };
        }
    }
}
