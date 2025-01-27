﻿using Microsoft.Extensions.Logging;


namespace ExpenseTracker
{
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
            //builder.Services.AddSingleton<UserService>();  // Register UserService for DI
            builder.Services.AddMauiBlazorWebView();
     

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<TransactionService>();
            builder.Services.AddSingleton<DebtService>();
            return builder.Build();
        }
    }
}
