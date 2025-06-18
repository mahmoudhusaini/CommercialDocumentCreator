using CommercialDocumentCreator.Classes;
using CommercialDocumentCreator.Classes.CommercialModels;
using CommercialDocumentCreator.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommercialDocumentCreator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryNoteController : ControllerBase
    {

        private readonly DeliveryNoteHelper _helper;
        public DeliveryNoteController(DeliveryNoteHelper helper)
        {
            this._helper = helper;
        }


        [HttpPost("/api/deliveryNote/print")]
        public async Task<IActionResult> Print([FromForm] List<ItemDetails> products)
        {
            string documentNumber = Request.Form["documentNumber"].ToString();
            var client = Request.Form["client"].ToString();
            DeliveryNote deliveryNote = new DeliveryNote()
            {
                ClientName = client,
                DocumentNumber = documentNumber,
            };

            var result = await this._helper.Create(deliveryNote);

            return Ok(deliveryNote);
        }
    }
}
