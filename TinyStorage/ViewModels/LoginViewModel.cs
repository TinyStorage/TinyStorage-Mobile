namespace TinyStorage.ViewModels;

public sealed class LoginViewModel(OidcClient client, IAuthenticationService authenticationService)
{
    public async Task OnLogin()
    {
        var result = await client.LoginAsync();

        if (result.IsError || string.IsNullOrWhiteSpace(result.AccessToken))
        {
            await Shell.Current.DisplayAlert("Ошибка входа", result.Error ?? "Неизвестная ошибка", "OK");
            return;
        }

        var isLab = result.User.Claims.Any(claim => claim is { Value: "Лаборант" });
        var isAdmin = result.User.Claims.Any(claim => claim is { Value: "Администратор" });

        authenticationService.SetToken(result.AccessToken, result.IdentityToken);

        if (isLab)
        {
            await Shell.Current.GoToAsync("//LaboratoryAssistantPage");
        }
        else if (isAdmin)
        {
            await Shell.Current.GoToAsync("//AdministratorPage");
        }
    }
}