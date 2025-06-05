namespace TinyStorage.Services;

public sealed class AuthenticationService : IAuthenticationService
{
    public string? IdentityToken { get; private set; }
    public string? AccessToken { get; private set; }

    public void SetToken(string accessToken, string identityToken)
    {
        AccessToken = accessToken;
        IdentityToken = identityToken;
    }
}