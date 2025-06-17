using CommercialDocumentCreator.Classes;
using CommercialDocumentCreator.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CommercialDocumentCreator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradeTallyController : ControllerBase
    {
        private readonly TradeTallyHelper _tradeTallyHelper;
        public TradeTallyController(TradeTallyHelper tradeTallyHelper)
        {
            this._tradeTallyHelper = tradeTallyHelper;
        }

        //string streamedData = "";
        //using (StreamReader sr = new StreamReader(Request.Body))
        //{
        //    streamedData = await sr.ReadToEndAsync();
        //}

        //try
        //{
        //    var products = JsonSerializer.Deserialize<ProductShipment>(streamedData);
        //}
        [HttpPost("/api/tradetally/validate/shipment/{percentage}/{freightRate}/{shipmentTitle}")]
        public async Task<IActionResult> ValidateAndPrintShipment([FromRoute] double percentage, [FromRoute] double freightRate,
                                                                  [FromRoute] string shipmentTitle, [FromBody] List<ProductShipment> prods)

        {
            double sumActualWeight = 0, sumFreightWeight = 0, sumTotalPrice = 0, sumTotalCost = 0
                , totalBillCheck = 0;
            foreach (ProductShipment product in prods)
            {
                sumActualWeight += product.ActualWeight;
                sumFreightWeight += product.FreightWeight;
                sumTotalPrice += product.TotalPrice;
                sumTotalCost += product.TotalCost;
            }
            totalBillCheck = ((freightRate * sumActualWeight) + sumTotalPrice) * percentage;
            if (sumActualWeight == sumFreightWeight && totalBillCheck == sumTotalCost)
            {
                var details = JsonSerializer.Serialize(prods);
                
                string res = await this._tradeTallyHelper.AddNewShipment(sumActualWeight, sumTotalPrice, sumTotalCost, details, freightRate, percentage, shipmentTitle);
                return Ok(new { message = "Shipment Calculation Valid", content = res });
            }
            return NotFound();
        }
    }
}
