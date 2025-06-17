using CommercialDocumentCreator.Classes.CommercialModels;
using CommercialDocumentCreator.Classes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommercialDocumentCreator.Helpers
{
    public class ReceiptHelper
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReceiptHelper(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this._context = dbContext;
            this._webHostEnvironment = webHostEnvironment;
        }



        #region CREATE - UPDATE Receipt
        public async Task<CommercialReceipt> CreateOrUpdate(int id, decimal? rate, string? warranty, int? deliveryDelay, string clientName, double? totalAmount)
        {
            CommercialReceipt? receipt = default;


            if (id == 0)
            {
                receipt = new CommercialReceipt();

                var newFolderPath = Path.Combine("wwwroot\\server-resources\\CommercialDocuments\\Receipts", $"{clientName} {receipt.DocumentNumber}", $"{receipt.DocumentNumber}");

                receipt.ClientName = clientName;
                receipt.Rate = rate;
                receipt.Warranty = warranty;
                receipt.DeliveryDelay = deliveryDelay;
                receipt.TotalAmount = totalAmount;
                receipt.ProductsPath = newFolderPath;
                receipt.CashDeposit = totalAmount;

                try
                {
                    _context.Receipts.Add(receipt);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                receipt = await _context.Receipts.FirstOrDefaultAsync(x => x.Id == id);

                if (receipt is not null)
                {
                    var oldFolder = Path.Combine("wwwroot\\server-resources\\CommercialDocuments\\Receipts", $"{receipt.ClientName} {receipt.DocumentNumber}");

                    var newFolderPath = Path.Combine("wwwroot\\server-resources\\CommercialDocuments\\Receipts", $"{clientName} {receipt.DocumentNumber}", $"{receipt.DocumentNumber}");

                    receipt.ClientName = clientName;
                    receipt.Rate = rate;
                    receipt.Warranty = warranty;
                    receipt.DeliveryDelay = deliveryDelay;
                    receipt.TotalAmount = totalAmount;
                    receipt.ProductsPath = newFolderPath;
                    receipt.CashDeposit = totalAmount;


                    try
                    {
                        _context.Receipts.Update(receipt);

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


            return receipt ?? new CommercialReceipt();
        }
        #endregion





        #region ALL Receipts

        public async Task<List<CommercialReceipt>> All()
        {
            List<CommercialReceipt> receipts = new List<CommercialReceipt>();

            try
            {
                receipts = await this._context.Receipts.OrderByDescending(q => q.CreationDate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return receipts;
        }

        #endregion






        #region GET Receipt

        public async Task<CommercialReceipt?> Get(int id)
        {
            var receipt = await this._context.Receipts.FirstOrDefaultAsync(q => q.Id == id);
            return receipt;
        }

        #endregion




        #region DELETE RECEIPT

        public async Task<bool> Delete(int id)
        {

            var receipt = await this._context.Receipts.FirstOrDefaultAsync(q => q.Id == id);

            if (receipt is null)
            {
                return false;
            }

            var path = $"wwwroot\\server-resources\\CommercialDocuments\\Receipts\\{receipt.ClientName} {receipt.DocumentNumber}";

            try
            {
                this._context.Receipts.Remove(receipt);
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
