using CommercialDocumentCreator.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CommercialDocumentCreator.Classes
{
    public class Product
    {
        public Product(int id, string? name, string? description, decimal? costPrice, decimal? sellingPrice, int? quantityInStock)
        {
            Id = Id;
            Name = name;
            Description = description;
            CostPrice = costPrice;
            SellingPrice = sellingPrice;
            QuantityInStock = quantityInStock;
        }

        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
        public decimal? CostPrice { get; set; }

        [JsonPropertyName("sellingPrice")]
        public decimal? SellingPrice { get; set; }

        [JsonPropertyName("quantityInStock")]
        public int? QuantityInStock { get; set; }
        public Brand Brand { get; set; }
        public int CategoryID { get; set; } // Foreign Key

        [ForeignKey("CategoryID")]
        public virtual ProductCategory Category { get; set; } // Navigation Property

    }
}
