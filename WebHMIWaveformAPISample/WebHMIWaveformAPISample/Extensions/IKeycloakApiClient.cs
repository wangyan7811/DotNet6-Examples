using Refit;

namespace ApiClient.KeycloakApiClient;

public interface IKeycloakApiClient
{
    [Post("/auth/realms/poem/protocol/openid-connect/token")]
    [Headers("Content-Type: application/x-www-form-urlencoded")]
    Task<KeycloakToken> TokenAsync([Body] string credential);
}

