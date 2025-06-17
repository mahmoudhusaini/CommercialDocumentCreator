using CommercialDocumentCreator.Classes;
using CommercialDocumentCreator.Classes.Data;
using Microsoft.EntityFrameworkCore;

namespace CommercialDocumentCreator.Helpers
{
    public class CategoryHelper
    {
        private readonly AppDbContext _context;

        public CategoryHelper(AppDbContext context)
        {
            this._context = context;
        }


        public async Task<List<ProductCategory>> AllAsync()
        {
            List<ProductCategory> categories = null;
            try
            {
                categories = await this._context.Categories.ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Error Fetching Data");
            }
            return categories ?? new List<ProductCategory>();
        }
        public async Task AddCategory(string name, int? parentId, int categoryLevel)
        {

            ProductCategory category = new ProductCategory()
            {
                CategoryLevel = categoryLevel,
                CategoryName = name,
                ParentCategoryId = parentId == 0 ? null : parentId,
                ParentCategory = parentId == 0 ? null : new ProductCategory(),
            };

            try
            {
                await this._context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception("Unable to add a new category" + ex.Message);
            }
        }

    }
}
