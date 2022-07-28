namespace ApiClient.PowerManagerClient;

/// <summary>
/// The PoValidateCredentials Request
/// </summary>
public class PowerManagerValidateCredentialsRequest
{
    /// <summary>
    /// The username.
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// The password.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// The return url.
    /// </summary>
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// Set auth cookie flag.
    /// </summary>
    public bool SetAuthCookie { get; set; } = true;
}