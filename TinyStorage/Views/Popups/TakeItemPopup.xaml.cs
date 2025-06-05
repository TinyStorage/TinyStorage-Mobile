namespace TinyStorage.Views.Popups;

public partial class TakeItemPopup : Popup
{
    private readonly Guid _itemId;
    private readonly Func<Guid, Task> _onConfirmed;

    public bool IsBusy { get; private set; }
    public bool IsNotBusy => !IsBusy;

    public TakeItemPopup(Guid itemId, Func<Guid, Task> onConfirmed)
    {
        InitializeComponent();
        BindingContext = this;

        _itemId = itemId;
        _onConfirmed = onConfirmed;
    }

    private void OnCancelClicked(object sender, EventArgs e) => Close();

    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        SetBusy(true);

        try
        {
            await _onConfirmed(_itemId);
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