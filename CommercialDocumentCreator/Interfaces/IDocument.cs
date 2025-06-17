using CommercialDocumentCreator.Classes;
using CommercialDocumentCreator.Classes.CommercialModels;
using CommercialDocumentCreator.Classes.Data;
using CommercialDocumentCreator.Enums;
using Microsoft.EntityFrameworkCore;

namespace CommercialDocumentCreator.Interfaces
{
    public interface IDocument

    {
        CommercialDocument ConvertCommercialDocument<T>(T document, PaperType newDocumentType) where T : CommercialDocument;
    }
}
