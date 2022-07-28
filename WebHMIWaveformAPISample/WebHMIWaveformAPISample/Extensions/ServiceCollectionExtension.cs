using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace ApiClient.KeycloakApiClient;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddKeycloakApiRefitClient(this IServiceCollection services, string baseUri)
    {
        services.AddRefitClient<IKeycloakApiClient>().ConfigureHttpClient(httpClient =>
        {
            httpClient.BaseAddress = new Uri(baseUri);
        });
        return services;
    }
}