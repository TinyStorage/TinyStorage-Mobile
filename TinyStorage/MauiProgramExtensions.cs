namespace TinyStorage;

public static class MauiProgramExtensions
{
    public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder)
    {
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddHttpClients()
            .AddAuthenticationServices();

        builder.Services.AddTransient<LoginPage>()
            .AddTransient<LaboratoryAssistantPage>()
            .AddTransient<AdministratorPage>()
            .AddTransient<RoleSelectionPage>();

        builder.Services.AddTransient<LoginViewModel>()
            .AddTransient<LaboratoryAssistantViewModel>()
            .AddTransient<AdministratorViewModel>();

        return builder;
    }
}