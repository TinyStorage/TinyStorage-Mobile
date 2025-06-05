namespace TinyStorage.Clients;

public static class DependencyInjection
{
    private const string TinyStorageUrl = "https://tiny-storage.online/api";

    public static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddRefitClient<ITinyStorageClient>()
            .ConfigureHttpClient(client => client.BaseAddress = new Uri(TinyStorageUrl))
            .AddHttpMessageHandler<AuthHeaderHandler>();

        return services;
    }
}