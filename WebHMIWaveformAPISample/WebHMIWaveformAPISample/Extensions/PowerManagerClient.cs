using ApiClient.PowerManagerClient;
using Esprima;
using Esprima.Ast;
using HtmlAgilityPack;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebHMIWaveformAPISample.Extensions;

public class PowerManagerClient
{
    private readonly HttpClient _httpClient;

    public PowerManagerClient()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = delegate { return true; }
        };
        _httpClient = new HttpClient(handler);
    }

    /// <summary>
    /// Get PowerOperation™ WebHMI Anti-Forgery Token.
    /// </summary>
    /// <param name="server">The PowerOperation™ Server.</param>
    /// <param name="path">The path.</param>
    /// <returns>The anti-forgery token.</returns>
    public async Task<string?> GetAntiForgeryTokenAsync(string server, string path)
    {

        // Streaming the page content.
        await using var authResponseStream = await _httpClient.GetStreamAsync($"https://{server}/{path}/Auth");

        // Locate the Javascript section.
        var authDocument = new HtmlDocument();
        authDocument.Load(authResponseStream);
        var authJs = authDocument.DocumentNode.SelectSingleNode("html/body/script[last()]");

        // Locate the validationCodeSubmit Javascript function.
        var validationCodeSubmitJsFunction = new JavaScriptParser(authJs.InnerHtml).ParseScript()
            .ChildNodes.FilterNodesByType<FunctionDeclaration>()
            .SingleOrDefault(function => function.Id!.Name == "validationCodeSubmit");

        // Locate the ajax call argument.
        var ajaxArgument = validationCodeSubmitJsFunction?.Body
            .ChildNodes.FilterNodesByType<ExpressionStatement>()
            .SelectMany(statement => statement.Expression.RecursiveFindCallExpression("ajax"))
            .SingleOrDefault()?
            .Arguments
            .SingleOrDefault() as ObjectExpression;

        // Locate the anti-forgery token.
        var token = ajaxArgument?
            .SingleOrDefaultObjectExpressionProperties<ObjectExpression>(property =>
                (property.Key as Identifier)?.Name == "headers")?
            .SingleOrDefaultObjectExpressionProperties<Literal>(property =>
                (property.Key as Literal)?.Value as string == "Anti-Forgery-Token")?
            .Value as string;

        return token;
    }

    /// <summary>
    /// Get PowerOperation™ WebHMI Cookie.
    /// </summary>
    /// <param name="server">The PowerOperation™ Server.</param>
    /// <param name="path">The path.</param>
    /// <param name="antiForgeryToken">The anti forgery token.</param>
    /// <param name="credential">The PowerOperation™ User Credential.</param>
    /// <returns>The cookie.</returns>
    public async Task<(PowerManagerValidateCredentialsResponse Body, string[] Cookies)> GetCookieAsync(string server, string path, string antiForgeryToken, PowerManagerValidateCredentialsRequest credential)
    {
        // Prepare payload.
        var requestContent = new StringContent(JsonSerializer.Serialize(credential, options: new JsonSerializerOptions(JsonSerializerDefaults.Web)));
        requestContent.Headers.TryAddWithoutValidation("Content-Type", "application/json");
        requestContent.Headers.TryAddWithoutValidation("Anti-Forgery-Token", antiForgeryToken);

        // Do auth.
        var response = await _httpClient.PostAsync($"https://{server}/{path}/Security/ValidateCredentials", requestContent);
        var responseContent = await response.Content.ReadFromJsonAsync<PowerManagerValidateCredentialsResponse>();
        var responseSetCookieHeader = response.Headers.GetValues("Set-Cookie");
        responseContent!.ReturnUrl = $"{server},{responseContent.ReturnUrl}";

        // Create IResult.
        return (responseContent, responseSetCookieHeader.ToArray());
    }
}