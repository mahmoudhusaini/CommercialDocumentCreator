using CommercialDocumentCreator.Classes.CommercialModels;
using CommercialDocumentCreator.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommercialDocumentCreator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {

        private readonly ReceiptHelper _receiptHelper;
        public ReceiptController(ReceiptHelper receiptHelper)
        {
            this._receiptHelper = receiptHelper;
        }


        #region Create Update Receipt

        [HttpPost("/api/Create/Receipt")]
        public async Task<IActionResult> Create([FromForm] decimal rate, [FromForm] int delay, [FromForm] double overAllAmount)
        {
            var warranty = Request.Form["warranty"].ToString();
            var clientName = Request.Form["client"].ToString();
            var products = Request.Form["products"];
            var idStr = Request.Form["id"].ToString();

            int id = 0;
            int.TryParse(idStr, out id);

            CommercialReceipt receipt = new CommercialReceipt();
            receipt = await _receiptHelper.CreateOrUpdate(id, rate, warranty, delay, clientName, overAllAmount);

            if (receipt is null)
            {
                return NotFound();
            }


            string path = receipt.ProductsPath!;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path!);
            }

            var file = Path.Combine(path, $"{receipt.DocumentNumber}.json");

            using (var sw = new StreamWriter(file))
            {
                try
                {
                    await sw.WriteAsync(products);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write to file {receipt.DocumentNumber} - {ex.Message}");
                    throw;
                }
            }


            return Ok(
            new
            {
                id = receipt.Id,
                receiptNumber = receipt.DocumentNumber,
                creationDate = DateOnly.FromDateTime(receipt.CreationDate),
                clientName = receipt.ClientName,
                type = receipt.PaperType,
                validityDate = DateOnly.FromDateTime(receipt.ValidityDate),
                rate = receipt.Rate,
                isPayable = receipt.IsPayable,
                warranty = receipt.Warranty,
                deliveryDelay = receipt.DeliveryDelay,
                totalAmount = receipt.TotalAmount,
            });
        }
        #endregion



        #region ALL RECEIPTS

        [HttpGet("/api/All/Reciepts")]
        public async Task<IActionResult> GetReceipts()
        {
            var all = await this._receiptHelper.All();
            return Ok(all);
        }
        #endregion



        #region LOAD RECEIPT

        [HttpGet("/api/get/receipt/{id}")]
        public async Task<IActionResult> GetReceipt([FromRoute] int id)
        {
            var receipt = await this._receiptHelper.Get(id);
            if (receipt is null)
            {
                return NotFound();
            }

            var jsonText = string.Empty;
            var fullPath = Path.Combine(receipt.ProductsPath ?? "Errors", $"{receipt.DocumentNumber}.json");

            try
            {
                using (StreamReader sr = new StreamReader(fullPath))
                {
                    jsonText = await sr.ReadToEndAsync();
                }
            }
            catch (Exception)
            {
                return NotFound("Product Details Not Found");
            }


            return Ok(new { receipt = receipt, details = jsonText });
        }

        #endregion



        #region DELETE RECEIPT


        [HttpDelete("/api/delete/reciept/{id}")]
        public async Task<IActionResult> DeleteReceipt([FromRoute] int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var result = await _receiptHelper.Delete(id);

            if (!result)
            {
                return NotFound();
            }
            return Ok(new { message = "Record Deleted" });
        }

        #endregion




    }
}
