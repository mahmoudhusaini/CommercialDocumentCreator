using CommercialDocumentCreator.Classes.CommercialModels;
using CommercialDocumentCreator.Helpers;
using CommercialDocumentCreator.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommercialDocumentCreator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharedUtilitiesController : ControllerBase
    {
        private readonly SharedUtilitiesHelper _helper;
        public SharedUtilitiesController(SharedUtilitiesHelper helper)
        {
            this._helper = helper;
        }


        #region Convert Commercial Document

        [HttpPost("/api/Convert/Document/{type}")]
        public async Task<IActionResult> ConvertInvoice([FromRoute] string type, [FromForm] decimal rate, [FromForm] int delay,
                                                        [FromForm] double overAllAmount, [FromForm] double cashDeposit)

        {
            var clientName = Request.Form["clientName"].ToString();
            var warranty = Request.Form["warranty"].ToString();
            string olDocumentId = Request.Form["olDocumentId"].ToString();
            var products = Request.Form["products"];

            var paperType = FormaterHelper.PaperTypeConverter(type);

            CommercialDocument commercialDocument = new CommercialDocument()
            {
                ClientName = clientName,
                Rate = rate,
                Warranty = warranty,
                DeliveryDelay = delay,
                TotalAmount = overAllAmount,
                CashDeposit = cashDeposit,
            };

            var result = _helper.ConvertCommercialDocument(commercialDocument, paperType);

            string path = result.ProductsPath!;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path!);
            }

            var file = Path.Combine(path, $"{result.DocumentNumber}.json");

            using (var sw = new StreamWriter(file))
            {
                try
                {
                    await sw.WriteAsync(products);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write to file {result.DocumentNumber} - {ex.Message}");
                    throw;
                }
            }

            return Ok(result);
        }

        #endregion


    }
}
