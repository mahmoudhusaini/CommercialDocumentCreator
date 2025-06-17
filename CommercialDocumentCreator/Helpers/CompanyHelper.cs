using CommercialDocumentCreator.Classes;
using System.Text;
using id = CommercialDocumentCreator.Classes.BusinessIdentificationNumbers;

namespace CommercialDocumentCreator.Helpers
{
    public class CompanyHelper
    {
        public string FormatInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(CompanyProfile.Name + "\n");
            sb.AppendLine(CompanyProfile.AbbreviationName + "\n");
            sb.AppendLine(CompanyProfile.Address + "\n");
            sb.AppendLine(CompanyProfile.Email + "\n");
            sb.AppendLine(CompanyProfile.PhoneNumber + "\n");
            sb.AppendLine(CompanyProfile.Website + "\n");
            sb.AppendLine(CompanyProfile.BankAccount + "\n");

            return sb.ToString();
        }

        public string FormatIdentificationInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(id.IdNat + "\n");
            sb.AppendLine(id.Rccm + "\n");
            sb.AppendLine(id.NImpot + "\n");

            return sb.ToString();
        }

        List<CompanyPartnership> Partnerships = new List<CompanyPartnership>()
            {
                 new CompanyPartnership("Dell", "Dell", DateTime.Now),
                 new CompanyPartnership("Lenovo", "Lenovo", DateTime.Now),
            };
        public List<CompanyPartnership> GetPartnerships()
        {
            return Partnerships;
        }

        public void AddPartnership(CompanyPartnership cp)
        {
            Partnerships?.Add(cp);
        }
    }
}
