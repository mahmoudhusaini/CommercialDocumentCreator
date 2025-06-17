using CommercialDocumentCreator.Classes;

namespace CommercialDocumentCreator.Helpers
{
    public class ProductHelper
    {
        List<Product> products = new List<Product>()
       {
        new Product(1, "Samsung TV", "33-inch Smart FHD TV", 250, 400, 10),
        new Product(2, "Lenovo E14 Laptop Core i7", "14-inch Screen\nCPU: Core i7-13th Gen\nRAM: 8GB DDR4\nStorage: 1TB SSD", 900, 1200, 5),
        new Product(3, "HP Dual Core Laptop", "15.6-inch Screen\nCPU: Celeron\nRAM: 4GB DDR4\nStorage: 1TB HDD", 400, 550, 8),
        new Product(4, "HP Printer 107W", "Laserjet Wireless Printer", 80, 120, 15),
        new Product(5, "Lenovo 300 Mouse", "Lenovo Wireless Mouse", 10, 20, 50),
        new Product(6, "Logitech H111 Headset", "Wired Headset", 15, 30, 25),
        new Product(7, "Gaming Mousepad", "Mousepad", 5, 10, 100),
       };

        public List<Product> GetProducts()
        {
            return products;
        }

    }
}
