using CommercialDocumentCreator.Enums;

namespace CommercialDocumentCreator.Classes
{
    public class Customer
    {
        public string? Name { get; set; }
        public CustomerType CustomerType { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? BankAccount { get; set; }
    }
}
