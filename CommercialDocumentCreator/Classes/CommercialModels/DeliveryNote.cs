using CommercialDocumentCreator.Enums;
using System.Text.Json.Serialization;

namespace CommercialDocumentCreator.Classes.CommercialModels
{
    public class DeliveryNote
    {
        public string? DocumentNumber { get; set; }
        public PaperType PaperType { get; set; } = PaperType.DeliveryNote;
        public DateOnly CreationDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string? ClientName { get; set; }

        public List<DeliveryNoteDetails>? Items { get; set; }
    }

    public class DeliveryNoteDetails
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("quantityInStock")]
        public string? QuantityInStock { get; set; }

    }

}
