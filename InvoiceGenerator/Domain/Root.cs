using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Routing.Constraints;

public class Root
{
    [JsonPropertyName("request")]
    public required List<Request> Requests { get; set; }
    public string FileName { get; set; }
}
