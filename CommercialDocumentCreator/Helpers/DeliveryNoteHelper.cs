using CommercialDocumentCreator.Classes;
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

        public async Task<string> Create(DeliveryNote deliveryNote, string products)
        {
            List<DeliveryNoteDetails> documentDetails = new List<DeliveryNoteDetails>();

            try
            {
                documentDetails = JsonSerializer.Deserialize<List<DeliveryNoteDetails>>(products);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            if (documentDetails is null)
            {
                return "";
            }


            string result = "<html>\r\n" +
                                "<head>\r\n" +
                                "<meta charset=\"UTF-8\">\r\n" +
                                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n" +
                                "<title>Delivery Note</title>\r\n" +
                                "<style>\r\n" +
                                    "* {\r\n" +
                                        "box-sizing: border-box;\r\n" +
                                    "}\r\n\r\n" +
                                    "body {\r\n" +
                                    "font-family: 'Cairo', sans-serif;\r\n" +
                                    "margin: 0;\r\n" +
                                    "padding: 1rem;\r\n" +
                                    "background: #f9f9f9;\r\n" +
                                    "}\r\n\r\n" +
                                    ".delivery-note {\r\n" +
                                    "background: #fff;\r\n" +
                                    "max-width: 800px;\r\n" +
                                    "margin: auto;\r\n" +
                                    "padding: 2rem;\r\n" +
                                    "box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\r\n" +
                                    "}\r\n\r\n    .header, .footer {\r\n" +
                                    "text-align: center;\r\n" +
                                    "}\r\n\r\n" +
                                    ".company-info, .client-info {\r\n" +
                                    "display: flex;\r\n" +
                                    "justify-content: space-between;\r\n" +
                                    "flex-wrap: wrap;\r\n" +
                                    "margin-bottom: 1.5rem;\r\n" +
                                    "}\r\n\r\n" +
                                    ".company-info div, .client-info div {\r\n" +
                                    "flex: 1 1 45%;\r\n" +
                                    "}\r\n\r\n" +
                                    "h2 {\r\n" +
                                    "text-align: center;\r\n" +
                                    "margin-bottom: 1.5rem;\r\n" +
                                    "}\r\n\r\n" +
                                    "table {\r\n" +
                                    "width: 100%;\r\n" +
                                    "border-collapse: collapse;\r\n" +
                                    "margin-bottom: 2rem;\r\n" +
                                    "}\r\n\r\n" +
                                    "th, td {\r\n" +
                                    "padding: 0.75rem;\r\n" +
                                    "border: 1px solid #ccc;\r\n" +
                                    "text-align: left;\r\n" +
                                    "}\r\n\r\n" +
                                    "th {\r\n" +
                                    "background: #f0f0f0;\r\n" +
                                    "}\r\n\r\n" +
                                    ".note {\r\n" +
                                    "font-size: 0.9rem;\r\n" +
                                    "color: #555;\r\n" +
                                    "}\r\n\r\n" +
                                    "@media print {\r\n" +
                                    "body {\r\n" +
                                    "background: none;\r\n" +
                                    "}\r\n" +
                                    ".delivery-note {\r\n" +
                                    "box-shadow: none;\r\n" +
                                    "margin: 0;\r\n" +
                                    "padding: 0;\r\n" +
                                    "}\r\n" +
                                    "}\r\n\r\n" +
                                    "@media (max-width: 600px) {\r\n" +
                                    ".company-info div, .client-info div {\r\n" +
                                    "flex: 1 1 100%;\r\n" +
                                    "margin-bottom: 1rem;\r\n" +
                                    "}\r\n\r\n" +
                                    "table, th, td {\r\n" +
                                    "font-size: 0.9rem;\r\n" +
                                    "}\r\n" +
                                    "}\r\n" +
                                    "</style>\r\n</head>\r\n<body>\r\n\r\n" +
                                    "<div class=\"delivery-note\">\r\n" +
                                    "<div class=\"header\">\r\n" +
                                    "<h1>Delivery Note</h1>\r\n" +
                                    "</div>\r\n\r\n" +
                                    "<div class=\"company-info\">\r\n" +
                                    "<div>\r\n" +
                                    "<strong>From:</strong><br>\r\n" +
                                    $"{CompanyProfile.Name}<br>\r\n" +
                                    $"{CompanyProfile.Address}<br>\r\n" +
                                    $"Phone: {CompanyProfile.PhoneNumber}\r\n" +
                                    "</div>\r\n" +
                                    "<div>\r\n" +
                                    "<strong>To:</strong><br>\r\n" +
                                    $"{deliveryNote.ClientName}<br>\r\n" +
                                    "</div>\r\n" +
                                    "</div>\r\n\r\n" +
                                    "<div class=\"client-info\">\r\n" +
                                    "<div>\r\n" +
                                    $"<strong>Document No:</strong> {deliveryNote.DocumentNumber}\r\n" +
                                    "</div>\r\n" +
                                    "<div>\r\n" +
                                    $"<strong>Date:</strong> {deliveryNote.CreationDate}\r\n" +
                                    "</div>\r\n" +
                                    "</div>\r\n\r\n" +
                                    "<table>\r\n" +
                                    "<thead>\r\n" +
                                    "<tr>\r\n" +
                                    "<th>#</th>\r\n" +
                                    "<th>Product Name</th>\r\n" +
                                    "<th>Description</th>\r\n" +
                                    "<th>Qty</th>\r\n" +
                                    "</tr>\r\n" +
                                    "</thead>\r\n" +
                                    "<tbody>\r\n";
            foreach (var item in documentDetails)
            {
                result += "<tr>\r\n\r\n" +
                             "<td></td>\r\n\r\n" +
                            $"<td>{item.Name}</td>\r\n\r\n" +
                            $"<td>{item.Description}</td>\r\n\r\n" +
                            $"<td>{item.QuantityInStock}</td>\r\n\r\n" +
                          "</tr>\r\n";
            }


            result += "</tbody>\r\n" +
                                    "</table>\r\n\r\n" +
                                    "<div class=\"footer\">\r\n" +
                                    "<p class=\"note\">Please check the items upon delivery. Contact us for any discrepancies.</p>\r\n" +
                                    "<p>Signature: ______________________</p>\r\n" +
                                    "</div>\r\n" +
                                    "</div>\r\n\r\n" +
                                    "</body>\r\n" +
                                    "</html>\r\n";


            return result;
        }

    }
}
