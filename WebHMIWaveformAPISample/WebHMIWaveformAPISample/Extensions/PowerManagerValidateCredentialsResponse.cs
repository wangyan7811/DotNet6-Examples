namespace ApiClient.PowerManagerClient;

/// <summary>
/// The PoValidateCredentials Response
/// </summary>
public class PowerManagerValidateCredentialsResponse
{
    /// <summary>
    /// The return url.
    /// </summary>
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// Is valid?
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Is Validation Code Required?
    /// </summary>
    public bool ValidationCodeRequired { get; set; }

    /// <summary>
    /// The IdentityId.
    /// </summary>
    public string? IdentityId { get; set; }
}