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

        public List<ItemDetails>? Items { get; set; }
    }

    public class ItemDetails
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("quantityInStock")]
        public int? QuantityInStock { get; set; }

        [JsonPropertyName("sellingPrice")]
        public double? SellingPrice { get; set; }

        [JsonPropertyName("productTotalPrice")]
        public double? TotalRaw => this.SellingPrice * this.QuantityInStock;
    }

}
