using System.Text.Json.Serialization;

public class Item
{
    [JsonPropertyName("order_item_id")]
    public int OrderItemId { get; set; }

    [JsonPropertyName("product_name")]
    public required string ProductName { get; set; }

    [JsonPropertyName("purchase_order")]
    public required string PurchaseOrder { get; set; }

    [JsonPropertyName("item")]
    public required string ItemDescription { get; set; }

    [JsonPropertyName("month_name")]
    public required string MonthName { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("gross_price")]
    public decimal GrossPrice { get; set; }

    [JsonPropertyName("net_price")]
    public decimal NetPrice { get; set; }
}
