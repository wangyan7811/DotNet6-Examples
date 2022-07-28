using System.Text.Json.Serialization;

namespace ExternalSystem.API.Model;

public class ExternalSystemError
{
    public int Code { get; set; }

    [JsonPropertyName("msg")]
    public string? Message { get; set; }

    public string? Detail { get; set; }
}
