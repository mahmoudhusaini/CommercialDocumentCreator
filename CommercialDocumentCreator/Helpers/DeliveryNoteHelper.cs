using CommercialDocumentCreator.Classes.CommercialModels;
using CommercialDocumentCreator.Classes.Data;
using System.Text.Json;

namespace CommercialDocumentCreator.Helpers
{
    public class DeliveryNoteHelper
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DeliveryNoteHelper(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this._context = dbContext;
            this._webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> Create(DeliveryNote deliveryNote)
        {
            //"description": "83 gran paper box of 5 packs ",
            //"name": "A4 Paper Box",
            //"productTotalPrice": 275,
            //"quantityInStock": "11",
            //"sellingPrice": "25"

            //try
            //{
            //    var result = JsonSerializer.Deserialize<List<ItemDetails>>(products);
            //}
            //catch (Exception)
            //{

            //    throw;
            //}

            return "";
        }

    }
}
