using CommercialDocumentCreator.Classes.CommercialModels;
using CommercialDocumentCreator.Enums;
using CommercialDocumentCreator.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommercialDocumentCreator.Controllers
{
    [ApiController]
    public class QuotationController : ControllerBase
    {
        private readonly QuotationHelper _quotationHelper;
        public QuotationController(QuotationHelper quotationHelper)
        {
            this._quotationHelper = quotationHelper;
        }

        #region Create Update Quotation

        [HttpPost("/api/Create/Quotation")]
        public async Task<IActionResult> CreateQuotation([FromForm] decimal rate, [FromForm] int delay, [FromForm] double overAllAmount)

        {
            var warranty = Request.Form["warranty"].ToString();
            var clientName = Request.Form["client"].ToString();
            var products = Request.Form["products"];
            var idStr = Request.Form["id"].ToString();

            int id = 0;
            int.TryParse(idStr, out id);

            CommercialQuotation quotation = new CommercialQuotation();

            quotation = await _quotationHelper.CreateOrUpdate(id, rate, warranty, delay, clientName, overAllAmount);

            if (quotation is null)
            {
                return NotFound();
            }

            string path = quotation.ProductsPath!;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path!);
            }

            var file = Path.Combine(path, $"{quotation.DocumentNumber}.json");

            using (var sw = new StreamWriter(file))
            {
                try
                {
                    await sw.WriteAsync(products);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write to file {quotation.DocumentNumber} - {ex.Message}");
                    throw;
                }
            }

            return Ok(
                new
                {
                    id = quotation.Id,
                    quotationNumber = quotation.DocumentNumber,
                    creationDate = DateOnly.FromDateTime(quotation.CreationDate),
                    clientName = quotation.ClientName,
                    type = quotation.PaperType,
                    validityDate = DateOnly.FromDateTime(quotation.ValidityDate),
                    rate = quotation.Rate,
                    isPayable = quotation.IsPayable,
                    warranty = quotation.Warranty,
                    deliveryDelay = quotation.DeliveryDelay,
                    totalAmount = quotation.TotalAmount,
                    remainingBalance = quotation.RemainingBalance,
                });
        }

        #endregion


        #region ALL QUOTATIONS

        [HttpGet("/api/all/quots")]
        public async Task<IActionResult> GetQuotations()
        {
            var all = await this._quotationHelper.All();
            return Ok(all);
        }
        #endregion


        #region LOAD QUOTATION

        [HttpGet("/api/get/quotation/{id}")]
        public async Task<IActionResult> GetQuotation([FromRoute] int id)
        {
            var quote = await this._quotationHelper.Get(id);
            if (quote is null)
            {
                return NotFound();
            }

            var jsonText = string.Empty;
            var fullPath = Path.Combine(quote.ProductsPath ?? "Errors", $"{quote.DocumentNumber}.json");

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


            return Ok(new { quotation = quote, details = jsonText });
        }

        #endregion



        #region Delete QUOTATION


        [HttpDelete("/api/delete/quotaion/{id}")]
        public async Task<IActionResult> DeleteInvoice([FromRoute] int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var result = await _quotationHelper.Delete(id);

            if (!result)
            {
                return NotFound();
            }
            return Ok(new { message = "Record Deleted" });
        }

        #endregion


    }
}
