using CommercialDocumentCreator.Classes;
using CommercialDocumentCreator.Classes.CommercialModels;
using CommercialDocumentCreator.Classes.Data;
using CommercialDocumentCreator.Enums;
using CommercialDocumentCreator.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using System.Text.Json;

namespace CommercialDocumentCreator.Helpers
{
    public class InvoiceHelper
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public InvoiceHelper(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this._dbContext = dbContext;
            this._webHostEnvironment = webHostEnvironment;
        }

        #region  Invoice Create
        public async Task<CommercialInvoice> NewInvoice(decimal? rate, string? warranty, int? deliveryDelay,
                                                           string clientName, double totalAmount, double? cashDeposit)
        {
            CommercialInvoice invoice = new CommercialInvoice()
            {
                ClientName = clientName,
                Rate = rate,
                Warranty = warranty,
                DeliveryDelay = deliveryDelay,
                TotalAmount = totalAmount,
                CashDeposit = cashDeposit,
            };

            string state = "Pending";

            invoice.RemainingBalance = invoice.TotalAmount - invoice.CashDeposit;

            if (cashDeposit == totalAmount)
            {
                invoice.Status = PaymentStatus.PaidCompletely;
                state = "Paid";
            }

            if (cashDeposit > 0 && cashDeposit < totalAmount)
            {
                invoice.Status = PaymentStatus.PaidPartially;
            }

            string path = Path.Combine(invoice.ProductsPath!, state, $"{invoice.ClientName} {invoice.DocumentNumber}");
            invoice.ProductsPath = path;

            try
            {
                _dbContext.Invoices.Add(invoice);
                await this._dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return invoice;
        }

        #endregion


        #region  Invoice Update
        public async Task<CommercialInvoice> Update(int id, string? warranty, string? clientName, decimal? rate, int? delay, double? total, double? cashDeposit)
        {
            CommercialInvoice? invoiceiInDb = new CommercialInvoice();
            string state = "Pending";


            invoiceiInDb = await _dbContext.Invoices.FirstOrDefaultAsync(x => x.Id == id);

            if (invoiceiInDb != null)
            {
                invoiceiInDb.Warranty = warranty;
                invoiceiInDb.ClientName = clientName;
                invoiceiInDb.Rate = rate;
                invoiceiInDb.DeliveryDelay = delay;
                invoiceiInDb.TotalAmount = total;
                invoiceiInDb.CashDeposit = cashDeposit;
                invoiceiInDb.RemainingBalance = total - cashDeposit;

                if (cashDeposit == total)
                {
                    invoiceiInDb.Status = PaymentStatus.PaidCompletely;
                    state = "Paid";
                }

                if (cashDeposit > 0 && cashDeposit < total)
                {
                    invoiceiInDb.Status = PaymentStatus.PaidPartially;
                }

                Directory.Delete(invoiceiInDb.ProductsPath!, recursive: true);

                string path = Path.Combine("wwwroot\\server-resources\\CommercialDocuments\\Invoices", state, $"{invoiceiInDb.ClientName} {invoiceiInDb.DocumentNumber}");
                invoiceiInDb.ProductsPath = path;

                try
                {
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                throw new NullReferenceException("Invoice You Are Updating is Null");
            }


            return invoiceiInDb;
        }

        #endregion


        #region Pay Invoice

        public async Task<bool> PayInvoice(int id)
        {
            try
            {
                var invoice = await this._dbContext.Invoices.FirstOrDefaultAsync(invoice => invoice.Id == id);

                if (invoice is null)
                {
                    throw new Exception($"No Invoice Found with Id no: {id}");
                }

                if (!invoice.IsPosted)
                {
                    return false;
                }

                invoice.Status = PaymentStatus.PaidCompletely;
                invoice.CashDeposit += invoice.RemainingBalance;
                invoice.RemainingBalance = 0;

                string wwwRoot = this._webHostEnvironment.WebRootPath;
                string commonPath = Path.Combine(wwwRoot, "server-resources", "CommercialDocuments", "Invoices");

                string documentId = Path.Combine($"{invoice.ClientName} {invoice.DocumentNumber}"); //$"{invoice.DocumentNumber}");
                string destPath = Path.Combine(commonPath, "Paid", documentId);
                invoice.ProductsPath = destPath;

                try
                {
                    await this._dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error while saving changes to database - {ex.Message}");
                }

                var sourceFile = Path.Combine(commonPath, "Pending", documentId);

                if (!Directory.Exists(sourceFile))
                {
                    return false;
                }

                var destFolder = Path.Combine(destPath, $"{invoice.DocumentNumber}");

                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }

                var destinationFile = Path.Combine(destFolder, $"{invoice.DocumentNumber}.json");

                try
                {
                    File.Move(Path.Combine(sourceFile, $"{invoice.DocumentNumber}", $"{invoice.DocumentNumber}.json"), destinationFile);
                    Directory.Delete(sourceFile, recursive: true);
                }
                catch (Exception ex)
                {
                    throw new Exception($"{ex.Message} - Unable to move file or unable to delete old path");
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}.");
            }

            return true;
        }

        #endregion


        #region ALL INVOICES

        public async Task<IEnumerable<CommercialInvoice>> GetAllInvoices(bool withPaid)
        {
            try
            {
                var invoices = await this._dbContext.Invoices.OrderByDescending(instance => instance.CreationDate).ToListAsync();
                if (withPaid)
                {
                    return invoices;
                }
                return invoices.FindAll(invoice => invoice.Status == PaymentStatus.PaidPartially || invoice.Status == PaymentStatus.Unpaid);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion


        #region POST INVOICE


        public async Task<bool> PostInvvoice(int id)
        {
            var invoice = await this._dbContext.Invoices.FirstOrDefaultAsync(invoice => invoice.Id == id);

            if (invoice == null)
            {
                return false;
                throw new Exception($"No Invoice Found with Id no: {id}");

            }

            invoice.IsPosted = true;
            try
            {
                await this._dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return true;
        }





        #endregion


        #region DELETE INVOICE

        public async Task<bool> DeleteRecord(int id)
        {
            var invoice = await this._dbContext.Invoices.FirstOrDefaultAsync(invoice => invoice.Id == id);

            string name = string.Empty;
            string state = string.Empty;
            string documentNumber = string.Empty;

            if (invoice is not null)
            {
                name = invoice.ClientName ?? "N/A";
                state = invoice.Status == PaymentStatus.PaidCompletely ? "Paid" : "Pending";
                documentNumber = invoice.DocumentNumber;

                try
                {
                    this._dbContext.Invoices.Remove(invoice);
                    await this._dbContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw new Exception("Unable to delete record from database");
                }
            }
            DeleteFromFile(name, documentNumber, state);

            return true;
        }

        public void DeleteFromFile(string name, string documentNumber, string state)
        {
            string documentId = $"{name} {documentNumber}";

            var rootPath = this._webHostEnvironment.WebRootPath;
            string path = Path.Combine(rootPath, "server-resources", "CommercialDocuments", "Invoices", $"{state}", documentId);

            if (!Directory.Exists(path))
            {
                throw new Exception("Directory Not Found");
            }

            try
            {
                Directory.Delete(path, recursive: true);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to Delete Directory - {ex.Message}");
            }
        }

        #endregion


        #region GET INVOICE PRODUCTS

        public async Task<string> ReturnInvoiceFile(int id, string clientName, string documentNumber, string state)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string path = Path.Combine(webRootPath, "server-resources", "CommercialDocuments", "Invoices", $"{state}", $"{clientName} {documentNumber}", $"{documentNumber}");

            if (!Directory.Exists(path))
            {
                throw new Exception("No Such Invoice Available");
            }


            string body = "";

            try
            {
                var file = Path.Combine(path, $"{documentNumber}.json");

                using (var sr = new StreamReader(file))
                {
                    body = await sr.ReadToEndAsync();
                }
            }
            catch (IOException)
            {
                throw new Exception("Something went wrong while reading data");
            }

            return body;
        }

        #endregion


        #region GET INVOICE 

        public async Task<CommercialInvoice> GetInvoice(int id)
        {
            var invoice = await this._dbContext.Invoices.FirstOrDefaultAsync(invoice => invoice.Id == id);
            if (invoice == null)
            {
                throw new Exception($"No Invoice Found");
            }
            return invoice;
        }

        #endregion

    }
}
