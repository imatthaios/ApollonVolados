using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using AndroidX.Core.View;

namespace ApollonVolados.Mobile;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        UpdateSystemBarIcons();

        // The in-app Automatic / Light / Dark switch changes the theme without a configuration change.
        if (Microsoft.Maui.Controls.Application.Current is { } app)
            app.RequestedThemeChanged += (_, _) => UpdateSystemBarIcons();
    }

    // UiMode is in ConfigurationChanges, so a system light/dark switch arrives here instead of recreating the activity.
    public override void OnConfigurationChanged(Configuration newConfig)
    {
        base.OnConfigurationChanged(newConfig);
        UpdateSystemBarIcons();
    }

    /// <summary>
    /// The app draws edge-to-edge on parchment (light) or pine night (dark), so the status- and
    /// navigation-bar icons must follow the app's theme: dark icons in light mode, light icons in dark mode.
    /// </summary>
    private void UpdateSystemBarIcons()
    {
        if (Window is null)
            return;

        var controller = WindowCompat.GetInsetsController(Window, Window.DecorView);
        if (controller is null)
            return;

        var isDark = Microsoft.Maui.Controls.Application.Current?.RequestedTheme is AppTheme theme
            ? theme == AppTheme.Dark
            : (Resources?.Configuration?.UiMode & UiMode.NightMask) == UiMode.NightYes;

        controller.AppearanceLightStatusBars = !isDark;
        controller.AppearanceLightNavigationBars = !isDark;
    }
}
