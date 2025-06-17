using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CommercialDocumentCreator.Classes
{
    public class ProductShipment
    {
    
        [JsonPropertyName("product-name")]
        public string? Name { get; set; }

        [JsonPropertyName("qty")]
        public int Quantity { get; set; }

        [JsonPropertyName("unit-price")]
        public double UnitPrice { get; set; }

        [JsonPropertyName("total-price")]
        public double TotalPrice { get; set; }

        [JsonPropertyName("weight")]
        public double ActualWeight { get; set; }

        [JsonPropertyName("freight-weight")]
        public double FreightWeight { get; set; }

        [JsonPropertyName("total-cost")]
        public double TotalCost { get; set; }

        [JsonPropertyName("landed-cost")]
        public double LandedCost { get; set; }

        [JsonPropertyName("delivered-checkbox")]
        public bool IsDelivered { get; set; }
    }

    public class Shipment
    {
        [Key]
        public int Id { get; set; }
        public string? ShipmentTitle { get; set; }
        public double OverAllTotal { get; set; }

        public double OverAllWeight { get; set; }

        public double OverAllCost { get; set; }

        public string? DetailsPath { get; set; }
        public double Freight { get; set; }
        public double MoneyTransferPercentage { get; set; }
    }
}
