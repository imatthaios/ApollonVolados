using ApollonVolados.Mobile.Services;
using ApollonVolados.Mobile.ViewModels;
using ApollonVolados.Mobile.Views;
using Microsoft.Extensions.Logging;

namespace ApollonVolados.Mobile;

/// <summary>App entry point: registers services, view models, pages and fonts.</summary>
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Platform services, registered behind interfaces so view models can be unit-tested.
        builder.Services.AddSingleton(Launcher.Default);
        builder.Services.AddSingleton(PhoneDialer.Default);
        builder.Services.AddSingleton<INavigationService, ShellNavigationService>();
        builder.Services.AddSingleton<IDialogService, ShellDialogService>();
        builder.Services.AddSingleton(Preferences.Default);
        builder.Services.AddSingleton<IThemeService>(sp => new ThemeService(sp.GetRequiredService<IPreferences>()));

        // News
        builder.Services.AddHttpClient();
        builder.Services.AddSingleton(_ => new NewsCacheService(FileSystem.CacheDirectory));
        builder.Services.AddSingleton<IWordPressService, WordPressService>();
        builder.Services.AddTransient<NewsViewModel>();
        builder.Services.AddTransient<NewsPage>();

        // Club
        builder.Services.AddSingleton<ClubViewModel>();
        builder.Services.AddSingleton<AppearanceViewModel>();
        builder.Services.AddTransient<ClubPage>();

        // Teams
        builder.Services.AddTransient<TeamsViewModel>();
        builder.Services.AddTransient<TeamsPage>();
        builder.Services.AddTransient<TeamWebPage>();

        // No custom fonts: the design system uses the platform serif and sans, which cover Greek
        // (see FontSerif in Resources/Styles/Styles.xaml and docs/DESIGN.md §3).
        builder.UseMauiApp<App>();

        return builder.Build();
    }
}
