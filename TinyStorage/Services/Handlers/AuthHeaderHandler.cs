namespace TinyStorage.Services.Handlers;

public sealed class AuthHeaderHandler(IAuthenticationService authenticationService) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(authenticationService.AccessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authenticationService.AccessToken);
        }

        return base.SendAsync(request, cancellationToken);
    }
}