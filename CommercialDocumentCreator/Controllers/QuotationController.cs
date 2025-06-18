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



            string template =
                            "<div class=\"popup-content\">\r\n    \r\n" +
                            "<button class=\"close-popup-btn\" onclick=\"closePopup()\">✖</button>\r\n\r\n" +
                            "<div class=\"popup-sidebar\">\r\n" +
                            "<label for=\"client-name\">Client Name:</label>\r\n" +
                            $"<input type=\"text\" value=\"{quote.ClientName}\" id=\"client-name\" name=\"client-name\" placeholder=\"Enter client name\" />\r\n\r\n" +
                            "<label for=\"total\">Total:</label>\r\n" +
                            $"<input type=\"text\" value=\"{quote.TotalAmount}\" id=\"total\" name=\"total\" readonly />\r\n\r\n" +
                            "<label for=\"warranty\">Warranty:</label>\r\n" +
                            $"<input type=\"text\" value=\"{quote.Warranty}\" id=\"warranty\" name=\"warranty\" />\r\n\r\n" +
                            "<label for=\"rate\">Rate:</label>\r\n" +
                            $"<input type=\"number\" value=\"{quote.Rate}\" id=\"rate\" name=\"rate\" />\r\n\r\n" +
                            "<label for=\"delivery-delay\">Delay:</label>\r\n" +
                            $"<input type=\"number\" value=\"{quote.DeliveryDelay}\" id=\"delivery-delay\" name=\"delivery-delay\" />\r\n\r\n";




            return Ok(new { quotation = quote, details = jsonText, template = template });
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
