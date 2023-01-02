﻿using Microsoft.AspNetCore.Components.WebView.Maui;
using Coursework.Data;
using MudBlazor.Services;

namespace Coursework;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
		#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Services.AddMudServices();
#endif
		
		

		return builder.Build();
	}
}
