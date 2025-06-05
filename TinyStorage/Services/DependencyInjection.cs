namespace TinyStorage.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        services
            .AddTransient<AuthHeaderHandler>()
            .AddTransient<WebAuthenticatorBrowser>()
            .AddSingleton<IAuthenticationService, AuthenticationService>()
            .AddSingleton<OidcClient>(provider =>
            {
                var browser = provider.GetRequiredService<WebAuthenticatorBrowser>();

                var options = new OidcClientOptions
                {
                    Authority = "https://tiny-storage.online/keycloak/realms/tiny-storage-realm",
                    ClientId = "DEV",
                    Scope = "openid profile",
                    RedirectUri = "tinystorage://localhost",
                    PostLogoutRedirectUri = "tinystorage://localhost",
                    Browser = browser,
                    DisablePushedAuthorization = true
                };

                return new OidcClient(options);
            });

        return services;
    }
}