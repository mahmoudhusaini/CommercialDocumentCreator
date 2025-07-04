using CommercialDocumentCreator.Classes;
using CommercialDocumentCreator.Classes.Data;
using Microsoft.AspNetCore.Hosting;


namespace CommercialDocumentCreator.Helpers
{
    //https://learn.microsoft.com/en-us/java/openjdk/download#openjdk-11
    //https://developer.ifs.com/tools/repdesigner
    public class TradeTallyHelper
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TradeTallyHelper(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this._dbContext = dbContext;
            this._webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> AddNewShipment(double sumFreightWeight, double sumTotalPrice, double sumTotalCost, 
                                                 string details, double freightRate, double percentage, string shipmentTitle)
        {
            Shipment newShipment = new Shipment()
            {
                OverAllWeight = sumFreightWeight,
                OverAllCost = sumTotalCost,
                OverAllTotal = sumTotalPrice,
                Freight = freightRate,
                MoneyTransferPercentage = percentage,
                ShipmentTitle = shipmentTitle
            };
          
            // For shipment pdf template

            string htmlContent = $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <link rel='preconnect' href='https://fonts.googleapis.com'>
            <link rel='preconnect' href='https://fonts.gstatic.com' crossorigin>
            <link href='https://fonts.googleapis.com/css2?family=Cairo:wght@200;400&display=swap' rel='stylesheet'>
            <title>Shipment Report</title>
            <style>
                body {{
                    font-family: 'Cairo', sans-serif;
                    box-sizing: border-box;
                    padding: 0;
                    margin: 0;
                }}
                .header {{
                    text-align: center;
                    font-size: 24px;
                    font-weight: bold;
                }}
                .details {{
                    margin: 80px auto;
                    width: 50%;
                }}
            </style>
        </head>
        <body>
            <div class='header'>
                <h2>Shipment Report</h2>
            </div>
            <div class='details'>
                <h3>Generated Date: {DateTime.Now}</h3>
                <h3>From Source Overall Amount: {newShipment.OverAllTotal:C}$</h3>
                <h3>Overall Weight: {newShipment.OverAllWeight} kg</h3>
                <h3>Freight Rate: {newShipment.Freight}$/1Kg</h3>
                <h3>Money Transfer Percentage: {newShipment.MoneyTransferPercentage}</h3>
                <h3>Landed Overall Amount: {newShipment.OverAllCost:C}$</h3>
            </div>
        </body>
        </html>";


            string webRootPath = _webHostEnvironment.WebRootPath;

            string path = Path.Combine(webRootPath, "server-resources", "Shipments", $"{newShipment.ShipmentTitle}");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var file = Path.Combine(path, $"{newShipment.ShipmentTitle}.json");
            using (StreamWriter sw = new StreamWriter(file))
            {
                try
                {
                    await sw.WriteAsync(details);
                }
                catch (Exception)
                {
                    throw new Exception("Unable to save shipment products");
                }
            }

            newShipment.DetailsPath = file;
            try
            {
                this._dbContext.Shipments.Add(newShipment);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Unable to add a new shipment");
            }
            return htmlContent;
        }
    }
}
