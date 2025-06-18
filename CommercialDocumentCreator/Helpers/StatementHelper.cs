using CommercialDocumentCreator.Classes.CommercialModels;
using CommercialDocumentCreator.Classes;
using CommercialDocumentCreator.Classes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommercialDocumentCreator.Enums;

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

            var receipts = await this._context.Receipts
                            .Where(recs => recs.CreationDate >= start && recs.CreationDate <= end).ToListAsync();

            var invoices = await this._context.Invoices
                            .Where(invcs => invcs.CreationDate >= start && invcs.CreationDate <= end)
                            .ToListAsync();



            statement.Invoices = invoices;
            statement.Receipts = receipts;


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



            var groupedInvoices = invoices
                    .GroupBy(x => x.ClientName)
                    .Select(group => new
                    {
                        ClientName = group.Key,
                        Invoices = group.ToList(),
                        TotalDeposited = group.Sum(invoice => invoice.CashDeposit),
                        TotalPending = group.Sum(invoice => invoice.RemainingBalance),
                    });




            string template =
                $"<div class=\"statement-summary\">\r\n" +
                   $"<h3>Detailed Summary</h3>\r\n" +
                   $"<p><strong>Period:</strong> {statement.From} to {statement.To}</p>\r\n\r\n"

                ;




            foreach (var group in groupedInvoices)
            {

                template +=
                   $"\n<p><strong>{group.ClientName}</strong></p>\r\n\r\n" +
                    $"<table class=\"statement-table\">\r\n" +
                     $"<thead>\r\n" +
                       $"<tr>\r\n" +
                         $"<th>Deposited ($)</th>\r\n" +
                         $"<th>Pending ($)</th>\r\n" +
                         $"<th>Total($)</th>\r\n" +
                         $"<th></th>\r\n" +
                       $"</tr>\r\n" +
                     $"</thead>\r\n";

                foreach (var invoice in group.Invoices)
                {
                    template +=
                     $"<tbody>\r\n" +
                       $"<tr>\r\n" +
                         $"<td>${invoice.CashDeposit}</td>\r\n" +
                         $"<td>${invoice.RemainingBalance}</td>\r\n" +
                         $"<td>${invoice.TotalAmount}</td>\r\n" +
                         $"<td</td>\r\n" +
                       $"</tr>\r\n" +
                     $"</tbody>\r\n";
                }


                template +=
                    $"<tfoot>\r\n" +
                        $"<tr>\r\n" +
                            $"<td><strong>Total Deposit: {group.TotalDeposited}</strong></td>\r\n" +
                            $"<td><strong>Total Pending: {group.TotalPending}</strong></td>\r\n" +
                            $"<td><strong>Overall: {group.TotalPending + group.TotalDeposited}</strong></td>\r\n" +
                        $"</tr>\r\n" +
                     $"</tfoot>\r\n" +
                   $"</table>\r\n" +
                $"</div>";


            }

            statement.DetailedTemplate = template;
            statement.Template = GetTemplate(statement);
            return statement;
        }



        private string GetTemplate(Statement statement)
        {
            string template =

                "<div class=\"statement-summary\">\r\n" +
                "<h3>Statement Summary</h3>\r\n" +
                $"<p><strong>{statement.Date}</strong></p>\r\n\r\n" +
                $"<p><strong>Period:</strong> {statement.From} to {statement.To}</p>\r\n\r\n" +
                "<table class=\"statement-table\">\r\n" +
                    "<thead>\r\n" +
                        "<tr>\r\n" +
                            "<th>Description</th>\r\n" +
                            "<th>Amount ($)</th>\r\n" +
                            "<th></th>\r\n" +
                        "</tr>\r\n" +
                    "</thead>\r\n" +

                    "<tbody>\r\n" +
                        "<tr>\r\n" +
                            "<td>Cash From Receipts</td>\r\n" +
                            $"<td>${statement.CashFromReceipts}</td>\r\n" +
                            "<td>\r\n" +
                                "<a href=\"/clientapp/view-reciepts/index.html\">\r\n" +
                                    "<div class=\"green-circle\"></div></td>\r\n" +
                                "</a>\r\n" +
                            "</td>\r\n" +
                        "</tr>\r\n" +
                        "<tr>\r\n" +
                            "<td>Deposit From Invoices</td>\r\n" +
                            $"<td>{statement.DepositFromInvoice}</td>\r\n" +
                            "<td><div class=\"green-circle\"></div></td>\r\n" +
                        "</tr>\r\n" +
                        "<tr>\r\n" +
                            "<td>Pending From Invoices</td>\r\n" +
                            $"<td>{statement.PendingFromInvoice}</td>\r\n" +
                            "<td>\r\n" +
                                "<a href=\"/clientapp/view-invoices/index.html\">\r\n" +
                                    "<div class=\"red-circle\"></div></td>\r\n" +
                                "</a>\r\n" +
                        "</tr>\r\n" +
                    "</tbody>\r\n" +

                    "<tfoot>\r\n" +
                        "<tr>\r\n" +
                            "<td colspan=\"2\"><strong>Total Income</strong></td>\r\n" +
                            $"<td colspan=\"2\"><strong>{statement.TotalIncome}</strong></td>\r\n" +
                        "</tr>\r\n" +
                    "</tfoot>\r\n" +
                "</table>\r\n" +
              "</div>";


            return template;
        }


        #endregion




    }
}
