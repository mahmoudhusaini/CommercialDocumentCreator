using CommercialDocumentCreator.Classes.CommercialModels;
using CommercialDocumentCreator.Classes.Data;
using CommercialDocumentCreator.Enums;
using CommercialDocumentCreator.Interfaces;

namespace CommercialDocumentCreator.Helpers
{
    public class SharedUtilitiesHelper : IDocument
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SharedUtilitiesHelper(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this._dbContext = dbContext;
            this._webHostEnvironment = webHostEnvironment;
        }

        public CommercialDocument ConvertCommercialDocument<T>(T document, PaperType newDocumentType) where T : CommercialDocument
        {
            if (document is null) throw new ArgumentNullException(nameof(document));

            switch (newDocumentType)
            {
                case PaperType.Quotation:

                    CommercialQuotation newQuotation = new CommercialQuotation();
                    newQuotation.DocumentNumber = document.DocumentNumber;
                    newQuotation.ClientName = document.ClientName;
                    newQuotation.Rate = document.Rate;
                    newQuotation.Warranty = document.Warranty;
                    newQuotation.DeliveryDelay = document.DeliveryDelay;
                    newQuotation.Status = PaymentStatus.NoStatus;
                    newQuotation.TotalAmount = document.TotalAmount;
                    newQuotation.CashDeposit = null;
                    newQuotation.RemainingBalance = null;

                    string fullPath = Path.Combine(newQuotation.ProductsPath ?? "Q Errors", $"{document.ClientName} {document.DocumentNumber}", $"{document.DocumentNumber}");
                    newQuotation.ProductsPath = fullPath;

                    try
                    {
                        _dbContext.Quotations.Add(newQuotation);
                        this._dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return newQuotation;

                case PaperType.Invoice:
                    CommercialInvoice newInvoice = new CommercialInvoice();
                    newInvoice.DocumentNumber = document.DocumentNumber;
                    newInvoice.ClientName = document.ClientName;
                    newInvoice.Rate = document.Rate;
                    newInvoice.Warranty = document.Warranty;
                    newInvoice.DeliveryDelay = document.DeliveryDelay;
                    newInvoice.TotalAmount = document.TotalAmount;
                    newInvoice.CashDeposit = document.CashDeposit ?? 0;
                    newInvoice.RemainingBalance = document.TotalAmount - (document.CashDeposit ?? 0);

                    fullPath = Path.Combine("wwwroot\\server-resources\\CommercialDocuments\\Invoices", "Pending", $"{document.ClientName} {document.DocumentNumber}", $"{document.DocumentNumber}");
                    newInvoice.ProductsPath = fullPath;

                    if (document.CashDeposit > 0)
                    {
                        newInvoice.Status = PaymentStatus.PaidPartially;
                    }

                    try
                    {
                        _dbContext.Invoices.Add(newInvoice);
                        this._dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    return newInvoice;

                case PaperType.Receipt:
                    CommercialReceipt receipt = new CommercialReceipt();
                    receipt.DocumentNumber = document.DocumentNumber;
                    receipt.ClientName = document.ClientName;
                    receipt.Rate = document.Rate;
                    receipt.Warranty = document.Warranty;
                    receipt.DeliveryDelay = document.DeliveryDelay;
                    receipt.TotalAmount = document.TotalAmount;
                    receipt.CashDeposit = document.TotalAmount;
                  
                    
                    fullPath = Path.Combine(receipt.ProductsPath ?? "Errors", $"{document.ClientName} {document.DocumentNumber}", $"{document.DocumentNumber}");
                    receipt.ProductsPath = fullPath;

                    try
                    {
                        _dbContext.Receipts.Add(receipt);
                        _dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                   return receipt;

                case PaperType.DeliveryNote:
                    break;
                default:
                    throw new Exception("Type to Convert to is not available");
            }


            return document;

        }
    }
}
