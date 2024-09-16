public class Address
{
    public Address(Request request)
    {
        CompanyName = request.InvoiceCompanyName;
        AddressLine = request.InvoiceAddress1 + " " + request.InvoiceAddress2 + " " + request.InvoiceAddress3;
        City = request.InvoiceCity;
        State = request.InvoiceStateCounty;
        Email = request.InvoiceContactEmailAddress;
    }

    public Address(string companyName, string addressLine, string city, string state, string email)
    {
        CompanyName = companyName;
        AddressLine = addressLine;
        City = city;
        State = state;
        Email = email;
    }

    public string CompanyName { get; set; }
    public string AddressLine { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Email { get; set; }
}
