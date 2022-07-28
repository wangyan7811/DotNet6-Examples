using ApiClient.PowerManagerClient;
using ExternalSystem.API.Model;
using WebHMIWaveformAPISample.Extensions;

namespace ExternalSystem.API.Extension;

/// <summary>
/// The functions.
/// </summary>
public static class PowerManagerClientExtension
{
    /// <summary>
    /// Login Logic to PO/PME Web System.
    /// </summary>
    /// <param name="client">The HttpClient.</param>
    /// <param name="keycloakApiClient">The keycloak api client.</param>
    /// <param name="modelingApiClient">The modeling api client.</param>
    /// <param name="menuId">The menu id.</param>
    /// <param name="server">The server.</param>
    /// <param name="authPath">The auth path.</param>
    /// <param name="dataPath">The data path.</param>
    /// <returns>The login result.</returns>
    public static async Task<PoLoginResult> WebLoginAsync(this PowerManagerClient client)
    {
        var server = "10.10.1.225";
        // Get access from keycloak for access modeling api.
        

        // Get AntiForgeryToken from PO.
        var antiForgeryToken = await client.GetAntiForgeryTokenAsync(server, "WebHmi");

        if (string.IsNullOrWhiteSpace(antiForgeryToken))
        {
            throw new ExternalSystemException(StatusCodes.Status404NotFound, "防伪令牌取得失败", $"防伪令牌为空");
        }

        // Get Cookie
        try
        {
            var response = await client.GetCookieAsync(server, "PsoDataService", antiForgeryToken,
                new PowerManagerValidateCredentialsRequest { UserName = "admin", Password = "admin", ReturnUrl = "/PsoDataService/AlarmData" });
            return new PoLoginResult(response.Body, response.Cookies);
        }
        catch (FormatException ex)
        {
            throw new ExternalSystemException(StatusCodes.Status400BadRequest, $"{server} 登录密码解密失败", ex.Message);
        }
        catch (InvalidOperationException ex) when (ex.Message == "The given header was not found.")
        {
            throw new ExternalSystemException(StatusCodes.Status400BadRequest, $"{server} 登录失败", ex.Message);
        }
    }
}