using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CommercialDocumentCreator.Classes
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }

        [JsonPropertyName("categoryName")]
        [Required]
        public string CategoryName { get; set; }


        [JsonPropertyName("parentCategoryID")]
        public int? ParentCategoryId { get; set; } // Nullable for top-level categories


        [ForeignKey(nameof(ParentCategoryId))]
        public ProductCategory? ParentCategory { get; set; } // Self-referencing relationship


        public List<ProductCategory> Subcategories { get; set; } = new List<ProductCategory>(); // Navigation property

        [JsonPropertyName("categoryLevel")]
        [Required]
        [Range(1, 3, ErrorMessage = "Category level must be between 1 and 3.")]
        public int CategoryLevel { get; set; } // 1 = Main, 2 = Subcategory, 3 = Sub-subcategory
    }

}
