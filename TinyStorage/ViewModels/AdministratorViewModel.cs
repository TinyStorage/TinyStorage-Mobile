namespace TinyStorage.ViewModels;

public sealed class AdministratorViewModel : BaseViewModel
{
    private readonly ITinyStorageClient _tinyStorageClient;
    private string _lastScannedCode = "Ожидание...";
    private bool _isScanning;
    private string? _pendingAction;

    private string LastScannedCode
    {
        get => _lastScannedCode;
        set => SetProperty(ref _lastScannedCode, value);
    }

    public Action<bool>? SetCameraEnabled { get; set; }
    public ICommand DetectionFinishedCommand { get; }
    public ICommand CreateItemCommand { get; }
    public ICommand DeleteItemCommand { get; }
    public ICommand LogoutCommand { get; }

    public AdministratorViewModel(ITinyStorageClient tinyStorageClient, OidcClient client,
        IAuthenticationService authenticationService)
    {
        _tinyStorageClient = tinyStorageClient;

        DetectionFinishedCommand = new Command<IReadOnlySet<BarcodeResult>>(OnBarcodeDetected);
        CreateItemCommand = new Command(() => StartScan(nameof(CreateItemCommand)));
        DeleteItemCommand = new Command(() => StartScan(nameof(DeleteItemCommand)));
        LogoutCommand = new Command(async () =>
        {
            await client.LogoutAsync(new LogoutRequest()
            {
                IdTokenHint = authenticationService.IdentityToken
            });
            await Shell.Current.GoToAsync("//LoginPage", true);
        });
    }

    private void StartScan(string action)
    {
        if (!_isScanning)
        {
            _isScanning = true;
            _pendingAction = action;
            LastScannedCode = "Сканирование...";
            SetCameraEnabled?.Invoke(true);
        }
    }

    private async void OnBarcodeDetected(IReadOnlySet<BarcodeResult> results)
    {
        if (!_isScanning)
        {
            return;
        }

        _isScanning = false;
        SetCameraEnabled?.Invoke(false);

        var result = results.FirstOrDefault();
        if (result is null || string.IsNullOrWhiteSpace(result.DisplayValue))
        {
            LastScannedCode = "Не удалось распознать код";
            return;
        }

        LastScannedCode = result.DisplayValue;

        if (!Guid.TryParse(LastScannedCode, out var itemId))
        {
            await Toast.Make("Неверный формат UUID").Show();
            return;
        }

        try
        {
            if (_pendingAction == nameof(CreateItemCommand))
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var popup = new CreateItemPopup(itemId, async (id, name) =>
                    {
                        var apiResponse = await _tinyStorageClient.CreateItemAsync(new CreateItemRequest(id, name));
                        if (apiResponse.IsSuccessStatusCode)
                        {
                            await Toast.Make("[Успех] Предмет успешно добавлен", ToastDuration.Long).Show();
                        }
                        else
                        {
                            switch (apiResponse.StatusCode)
                            {
                                case HttpStatusCode.Conflict:
                                    await Toast.Make("[Ошибка] Предмет уже добавлен", ToastDuration.Long).Show();
                                    break;
                                case HttpStatusCode.BadRequest:
                                    await Toast.Make("[Ошибка] Неверный формат названия предмета", ToastDuration.Long).Show();
                                    break;
                                case HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden:
                                    await Toast.Make("[Ошибка] Нет доступа", ToastDuration.Long).Show();
                                    break;
                                default:
                                    await Toast.Make("[Ошибка] Что-то пошло не так", ToastDuration.Long).Show();
                                    break;
                            }
                        }
                    });

                    await Application.Current?.MainPage?.ShowPopupAsync(popup)!;
                });
            }
            else if (_pendingAction == nameof(DeleteItemCommand))
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var popup = new DeleteItemPopup(itemId, async (id) =>
                    {
                        var apiResponse = await _tinyStorageClient.DeleteItemAsync(id);
                        if (apiResponse.IsSuccessStatusCode)
                        {
                            await Toast.Make("[Успех] Предмет успешно удален", ToastDuration.Long).Show();
                        }
                        else
                        {
                            switch (apiResponse.StatusCode)
                            {
                                case HttpStatusCode.Conflict:
                                case HttpStatusCode.NotFound:
                                    await Toast.Make("[Ошибка] Предмет не был добавлен", ToastDuration.Long).Show();
                                    break;
                                case HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden:
                                    await Toast.Make("[Ошибка] Нет доступа", ToastDuration.Long).Show();
                                    break;
                                default:
                                    await Toast.Make("[Ошибка] Что-то пошло не так", ToastDuration.Long).Show();
                                    break;
                            }
                        }
                    });

                    await Application.Current?.MainPage?.ShowPopupAsync(popup)!;
                });
            }
        }
        catch (Exception exception)
        {
            await Toast.Make("[Ошибка] Что-то пошло не так", ToastDuration.Long).Show();
        }

        _pendingAction = null;
    }
}