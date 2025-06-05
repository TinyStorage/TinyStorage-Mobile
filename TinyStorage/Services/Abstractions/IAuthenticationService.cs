namespace TinyStorage.Services.Abstractions;

public interface IAuthenticationService
{
    string? IdentityToken { get; }
    string? AccessToken { get; }
    void SetToken(string accessToken, string identityToken);
}