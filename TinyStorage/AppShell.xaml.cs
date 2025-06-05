namespace TinyStorage;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
		Routing.RegisterRoute(nameof(LaboratoryAssistantPage), typeof(LaboratoryAssistantPage));
		Routing.RegisterRoute(nameof(AdministratorPage), typeof(AdministratorPage));
		Routing.RegisterRoute(nameof(RoleSelectionPage), typeof(RoleSelectionPage));
	}
}
