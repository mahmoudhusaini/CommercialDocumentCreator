using CommercialDocumentCreator.Classes.CommercialModels;
using CommercialDocumentCreator.Classes;
using CommercialDocumentCreator.Classes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommercialDocumentCreator.Helpers
{
    public class StatementHelper
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StatementHelper(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this._context = dbContext;
            this._webHostEnvironment = webHostEnvironment;
        }

        #region GET STATEMENT

        public async Task<Statement?> Statement(DateTime start, DateTime end)
        {
            Statement statement = new Statement(start, end);

            try
            {
                var receipts = await this._context.Receipts.Where(recs => recs.CreationDate >= start && recs.CreationDate <= end).ToListAsync();
                var invoices = await this._context.Invoices.Where(invcs => invcs.CreationDate >= start && invcs.CreationDate <= end).ToListAsync();

                statement.Invoices = invoices;
                statement.Receipts = receipts;
            }
            catch (Exception)
            {
                throw;
            }


            foreach (var receipt in statement.Receipts)
            {
                statement.CashFromReceipts += receipt.CashDeposit;
            }

            foreach (var invoice in statement.Invoices)
            {
                statement.DepositFromInvoice += invoice.CashDeposit;
                statement.PendingFromInvoice += invoice.RemainingBalance;
            }
            statement.TotalFromInvoice = statement.DepositFromInvoice + statement.PendingFromInvoice;

            return statement;
        }

        #endregion




    }
}
