using CommercialDocumentCreator.Classes;
using CommercialDocumentCreator.Classes.CommercialModels;
using CommercialDocumentCreator.Classes.Data;
using CommercialDocumentCreator.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CommercialDocumentCreator.Helpers
{
    public class QuotationHelper
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public QuotationHelper(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this._context = dbContext;
            this._webHostEnvironment = webHostEnvironment;
        }


        #region CREATE - UPDATE QUOTATION
        public async Task<CommercialQuotation> CreateOrUpdate(int id, decimal? rate, string? warranty, int? deliveryDelay, string clientName, double? totalAmount)
        {
            CommercialQuotation? quotation = default;


            if (id == 0)
            {
                quotation = new CommercialQuotation();

                var newFolderPath = Path.Combine("wwwroot\\server-resources\\CommercialDocuments\\Quotations", $"{clientName} {quotation.DocumentNumber}", $"{quotation.DocumentNumber}");

                quotation.ClientName = clientName;
                quotation.Rate = rate;
                quotation.Warranty = warranty;
                quotation.DeliveryDelay = deliveryDelay;
                quotation.TotalAmount = totalAmount;
                quotation.ProductsPath = newFolderPath;

                try
                {
                    _context.Quotations.Add(quotation);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                quotation = await _context.Quotations.FirstOrDefaultAsync(x => x.Id == id);

                if (quotation is not null)
                {
                    var oldFolder = Path.Combine("wwwroot\\server-resources\\CommercialDocuments\\Quotations", $"{quotation.ClientName} {quotation.DocumentNumber}");

                    var newFolderPath = Path.Combine("wwwroot\\server-resources\\CommercialDocuments\\Quotations", $"{clientName} {quotation.DocumentNumber}", $"{quotation.DocumentNumber}");

                    quotation.ClientName = clientName;
                    quotation.Rate = rate;
                    quotation.Warranty = warranty;
                    quotation.DeliveryDelay = deliveryDelay;
                    quotation.TotalAmount = totalAmount;
                    quotation.ProductsPath = newFolderPath;

                    try
                    {
                        _context.Quotations.Update(quotation);

                        Directory.Delete(oldFolder, recursive: true);

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }

            }

            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return quotation ?? new CommercialQuotation();
        }
        #endregion

       

        #region ALL QUOTATIONS

        public async Task<List<CommercialQuotation>> All()
        {
            List<CommercialQuotation> quots = new List<CommercialQuotation>();

            try
            {
                quots = await this._context.Quotations.OrderByDescending(q => q.CreationDate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return quots;
        }

        #endregion



        #region GET QUOTATION

        public async Task<CommercialQuotation?> Get(int id)
        {
            var quote = await this._context.Quotations.FirstOrDefaultAsync(q => q.Id == id);
            return quote;
        }

        #endregion




        #region DELETE QUOTATION

        public async Task<bool> Delete(int id)
        {

            var quote = await this._context.Quotations.FirstOrDefaultAsync(q => q.Id == id);

            if (quote is null)
            {
                return false;
            }

            var path = $"wwwroot\\server-resources\\CommercialDocuments\\Quotations\\{quote.ClientName} {quote.DocumentNumber}";

            try
            {
                this._context.Quotations.Remove(quote);
                await this._context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            try
            {
                Directory.Delete(path, recursive: true);   
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #endregion


    }
}
