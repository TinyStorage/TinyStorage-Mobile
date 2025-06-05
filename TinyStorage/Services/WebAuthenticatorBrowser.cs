namespace TinyStorage.Services;

public sealed class WebAuthenticatorBrowser : IBrowser
{
    public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
    {
        try
        {
            var authResult = await WebAuthenticator.AuthenticateAsync(new Uri(options.StartUrl), new Uri("tinystorage://localhost"));
            var authorizeResponse = ToRawIdentityUrl(options.EndUrl, authResult);

            return new BrowserResult
            {
                Response = authorizeResponse
            };
        }
        catch (Exception ex)
        {
            return new BrowserResult()
            {
                ResultType = BrowserResultType.UnknownError,
                Error = ex.ToString()
            };
        }
    }

    private static string ToRawIdentityUrl(string redirectUrl, WebAuthenticatorResult result)
    {
        var parameters = result.Properties.Select(pair => $"{pair.Key}={pair.Value}");
        var values = string.Join("&", parameters);

        return $"{redirectUrl}#{values}";
    }
}