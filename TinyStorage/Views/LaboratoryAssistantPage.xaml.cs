namespace TinyStorage.Views;

public partial class LaboratoryAssistantPage
{
    public LaboratoryAssistantPage(LaboratoryAssistantViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.Camera>();
        }

        if (status != PermissionStatus.Granted)
        {
            await Toast.Make("Камера недоступна — разрешение не получено", ToastDuration.Long).Show();
        }
    }
}