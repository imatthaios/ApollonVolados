using ApollonVolados.Mobile.Services;
using ApollonVolados.Mobile.ViewModels;
using ApollonVolados.Mobile.Views;
using Microsoft.Extensions.Logging;

namespace ApollonVolados.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddHttpClient();

        builder.Services.AddSingleton<WordPressService>();
        builder.Services.AddTransient<NewsViewModel>();
        builder.Services.AddTransient<NewsPage>();

        return builder.Build();
    }
}