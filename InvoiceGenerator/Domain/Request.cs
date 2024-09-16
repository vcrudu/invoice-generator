using System.Text.Json.Serialization;
public class Request
{
    [JsonPropertyName("order_id")]
    public int OrderId { get; set; }

    [JsonPropertyName("sales_person")]
    public required string SalesPerson { get; set; }

    [JsonPropertyName("order_confirmed_date")]
    public DateTime OrderConfirmedDate { get; set; }

    [JsonPropertyName("currency_name")]
    public required string CurrencyName { get; set; }

    [JsonPropertyName("special_instructions")]
    public required string SpecialInstructions { get; set; }

    [JsonPropertyName("invoice_advertiser")]
    public required string InvoiceAdvertiser { get; set; }

    [JsonPropertyName("invoice_company_name")]
    public required string InvoiceCompanyName { get; set; }

    [JsonPropertyName("invoice_address1")]
    public required string InvoiceAddress1 { get; set; }

    [JsonPropertyName("invoice_address2")]
    public required string InvoiceAddress2 { get; set; }

    [JsonPropertyName("invoice_address3")]
    public required  string InvoiceAddress3 { get; set; }

    [JsonPropertyName("invoice_city")]
    public required string InvoiceCity { get; set; }

    [JsonPropertyName("invoice_state_county")]
    public required string InvoiceStateCounty { get; set; }

    [JsonPropertyName("invoice_post_code")]
    public required string InvoicePostCode { get; set; }

    [JsonPropertyName("invoice_country_name")]
    public required string InvoiceCountryName { get; set; }

    [JsonPropertyName("invoice_contact_name")]
    public required string InvoiceContactName { get; set; }

    [JsonPropertyName("invoice_contact_email_address")]
    public required string InvoiceContactEmailAddress { get; set; }

    [JsonPropertyName("items")]
    public required List<Item> Items { get; set; }
}

