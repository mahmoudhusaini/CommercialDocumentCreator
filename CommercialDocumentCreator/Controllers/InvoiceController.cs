using CommercialDocumentCreator.Classes.CommercialModels;
using CommercialDocumentCreator.Helpers;
using CommercialDocumentCreator.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CommercialDocumentCreator.Enums;

namespace CommercialDocumentCreator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {

        private readonly InvoiceHelper _helper;
        public InvoiceController(InvoiceHelper helper)
        {
            this._helper = helper;
        }



        #region Create Update Invoice

        [HttpPost("/api/Create/Invoice")]
        public async Task<IActionResult> CreateInvoice([FromForm] decimal rate, [FromForm] int delay, [FromForm] double overAllAmount, [FromForm] double cashDeposit)

        {
            var warranty = Request.Form["warranty"].ToString();
            var clientName = Request.Form["client"].ToString();
            var products = Request.Form["products"];
            var idStr = Request.Form["id"].ToString();

            int id = 0;
            int.TryParse(idStr, out id);

            CommercialInvoice invoice = new CommercialInvoice();

            if (id == 0)
            {
                invoice = await _helper.NewInvoice(rate, warranty, delay, clientName, overAllAmount, cashDeposit);
            }


            else
            {
                invoice = await _helper.Update(id, warranty, clientName, rate, delay, overAllAmount, cashDeposit);
            }

            if (invoice is null)
            {
                return NotFound();
            }

            var path = Path.Combine(invoice.ProductsPath!, invoice.DocumentNumber);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path!);
            }

            var file = Path.Combine(path, $"{invoice.DocumentNumber}.json");

            using (var sw = new StreamWriter(file))
            {
                try
                {
                    await sw.WriteAsync(products);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write to file {invoice.DocumentNumber} - {ex.Message}");
                    throw;
                }
            }

            return Ok(
                new
                {
                    id = invoice.Id,
                    invoiceNumber = invoice.DocumentNumber,
                    creationDate = DateOnly.FromDateTime(invoice.CreationDate),
                    clientName = invoice.ClientName,
                    type = invoice.PaperType,
                    validityDate = DateOnly.FromDateTime(invoice.ValidityDate),
                    rate = invoice.Rate,
                    isPayable = invoice.IsPayable,
                    warranty = invoice.Warranty,
                    deliveryDelay = invoice.DeliveryDelay,
                    totalAmount = invoice.TotalAmount,
                    remainingBalance = invoice.RemainingBalance,
                    cashDeposit = invoice.CashDeposit,
                });
        }

        #endregion


        #region Pay Invoice

        [HttpPost("/api/pay/invoice/{id}")]
        public async Task<IActionResult> PayInvoice([FromRoute] int id)
        {
            if (id <= 0) { return NotFound(); }

            var pay = await this._helper.PayInvoice(id);

            if (!pay)
            {
                return Conflict("Invoice Should be Posted Before Doing This Action");
            }

            return Ok(new { message = $" Invoice number is closed" });
        }

        #endregion


        #region ALL INVOICES

        [HttpGet("/api/all/invoices/{withPaid}")]
        public async Task<IActionResult> GetInvoices([FromRoute] bool withPaid)
        {
            var invoices = await _helper.GetAllInvoices(withPaid);
            return Ok(invoices);
        }
        #endregion


        #region Post Invoice

        [HttpPost("/api/post/invoice/{id}")]
        public async Task<IActionResult> PostInvoice([FromRoute] int id)
        {
            if (id <= 0) return NotFound();

            var result = await this._helper.PostInvvoice(id);

            if (!result)
            {
                return BadRequest();
            }
            return Ok(new { message = "Invoice Posted" });
        }

        #endregion



        #region Delete Invoice


        [HttpDelete("/api/delete/invoice/{id}")]
        public async Task<IActionResult> DeleteInvoice([FromRoute] int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var result = await _helper.DeleteRecord(id);
            if (!result)
            {
                return BadRequest();
            }
            return Ok(new { message = "Record Deleted" });
        }

        #endregion


        #region LOAD INVOICE

        [HttpGet("/api/get/invoice/{id}")]
        public async Task<IActionResult> GetInvoice([FromRoute] int id)
        {
            var invoice = await this._helper.GetInvoice(id);
            string state = invoice.Status == PaymentStatus.PaidCompletely ? "Paid" : "Pending";
            var jsonText = await this._helper.ReturnInvoiceFile(id, invoice.ClientName ?? "", invoice.DocumentNumber, state);
            return Ok(new { invoice = invoice, details = jsonText });
        }

        #endregion

    }
}
