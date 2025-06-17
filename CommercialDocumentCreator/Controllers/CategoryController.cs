using CommercialDocumentCreator.Classes;
using CommercialDocumentCreator.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommercialDocumentCreator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryHelper _categoryHelper;
        public CategoryController(CategoryHelper categoryHelper)
        {
            this._categoryHelper = categoryHelper;
        }

        [HttpGet("/api/categories/all")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await this._categoryHelper.AllAsync();
            return Ok(categories);
        }

        [HttpPost("/api/categories/new")]
        public async Task<IActionResult> AddNewCategory()
        {
            var categoryName = Request.Form["categoryName"].ToString();
            var categoryLevel = Convert.ToInt32(Request.Form["categoryLevel"]);
            var parentCategoryID = Convert.ToInt32(Request.Form["parentCategoryID"]);

            if (string.IsNullOrWhiteSpace(categoryName) || categoryLevel < 1 || categoryLevel > 3)
            {
                return BadRequest("Invalid category data");
            }
            await _categoryHelper.AddCategory(categoryName, parentCategoryID, categoryLevel);
            return Ok(new { message = "Category Added Successfully" });
        }
    }
}
