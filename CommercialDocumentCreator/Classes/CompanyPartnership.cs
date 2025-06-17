namespace CommercialDocumentCreator.Classes
{
    public class CompanyPartnership
    {
        public CompanyPartnership(string? title, string? companyName, DateTime date)
        {
            Title = title;
            CompanyName = companyName;
            Date = date;
        }

        public string? Title { get; set; }
        public string? CompanyName { get; set; }
        public string? Image { get; set; }
        public DateTime Date { get; set; }
    }
}
