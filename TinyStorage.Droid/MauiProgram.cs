using BarcodeScanning;

namespace TinyStorage;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

		builder
			.UseSharedMauiApp()
			.UseBarcodeScanning();

		return builder.Build();
	}
}
