using System.Text;
using System.Text.Json;
using ApiClient.PowerManagerClient;
using Microsoft.Extensions.Primitives;

namespace ExternalSystem.API.Model;

/// <summary>
/// The po login result.
/// </summary>
public class PoLoginResult : IResult
{
    /// <summary>
    /// Create a new instance of <see cref="PoLoginResult"/>.
    /// </summary>
    /// <param name="body">The <see cref="PowerManagerValidateCredentialsResponse"/>.</param>
    /// <param name="cookie">The cookie.</param>
    public PoLoginResult(PowerManagerValidateCredentialsResponse? body, params string[] cookie)
    {
        Cookie = cookie;
        Body = body;
    }

    /// <summary>
    /// The cookie.
    /// </summary>
    private IEnumerable<string> Cookie;

    /// <summary>
    /// The <see cref="PowerManagerValidateCredentialsResponse"/>.
    /// </summary>
    private PowerManagerValidateCredentialsResponse? Body;

    /// <inheritdoc />
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.Headers.TryAdd("Content-Type", "application/json");
        httpContext.Response.Headers.TryAdd("Set-Cookie", new StringValues(Cookie.ToArray()));
        await httpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(Body)));
    }
}