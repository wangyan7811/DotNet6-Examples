using System.Text.Json.Serialization;

namespace ApiClient.KeycloakApiClient;

/// <summary>
/// The Keycloak Token
/// </summary>
public class KeycloakToken
{
    /// <summary>
    /// The scheme.
    /// </summary>
    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }

    /// <summary>
    /// The token.
    /// </summary>
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    /// <summary>
    /// The error.
    /// </summary>
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    /// <summary>
    /// The error description.
    /// </summary>
    [JsonPropertyName("error_description")]
    public string? ErrorDescription { get; set; }

    /// <inheritdoc />
    public override string ToString() => $"{TokenType} {AccessToken}";
}