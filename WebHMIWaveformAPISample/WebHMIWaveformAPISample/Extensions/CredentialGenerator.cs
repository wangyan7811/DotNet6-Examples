namespace ApiClient.KeycloakApiClient;

public static class CredentialGenerator
{
    public static readonly Func<string, string, string> ClientCredential = (clientId, clientSecret) =>
        string.Join("&", $"client_id={clientId}", $"grant_type=client_credentials", $"client_secret={clientSecret}");
}