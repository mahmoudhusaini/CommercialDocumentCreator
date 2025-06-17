using CommercialDocumentCreator.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommercialDocumentCreator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public ProductHelper helper { get; set; } = new ProductHelper();
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IWebHostEnvironment webHostEnvironment)
        {
            this._webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("/api/products/all")]
        public IActionResult GetAll()
        {
            var products = helper.GetProducts();
            return Ok(products);
        }


        [HttpPost("/api/products/add/new")]
        public async Task<IActionResult> Addproduct()
        {
            return Ok();
        }
    }
}
