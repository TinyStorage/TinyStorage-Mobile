namespace TinyStorage.Views.Popups;

public partial class CreateItemPopup : Popup
{
    private readonly Guid _itemId;
    private readonly Func<Guid, string, Task> _onConfirmed;

    public bool IsBusy { get; private set; }
    public bool IsNotBusy => !IsBusy;

    public CreateItemPopup(Guid itemId, Func<Guid, string, Task> onConfirmed)
    {
        InitializeComponent();
        BindingContext = this;

        _itemId = itemId;
        _onConfirmed = onConfirmed;
    }

    private void OnCancelClicked(object sender, EventArgs e) => Close();

    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        var name = ItemNameEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        SetBusy(true);

        try
        {
            await _onConfirmed(_itemId, name);
            Close();
        }
        catch (Exception exception)
        {
            await Toast.Make("Что-то пошло не так", ToastDuration.Long).Show();
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void SetBusy(bool value)
    {
        IsBusy = value;
        OnPropertyChanged(nameof(IsBusy));
        OnPropertyChanged(nameof(IsNotBusy));

        Loader.IsVisible = value;
        ConfirmButton.Text = value ? string.Empty : "Да";
    }
}