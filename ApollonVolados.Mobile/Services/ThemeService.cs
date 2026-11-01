namespace ApollonVolados.Mobile.Services;

/// <summary>The user's appearance choice. <see cref="System"/> follows the phone's setting.</summary>
public enum ThemePreference
{
    System,
    Light,
    Dark
}

/// <summary>Stores the appearance choice and applies it to the running app.</summary>
public interface IThemeService
{
    /// <summary>The saved choice; <see cref="ThemePreference.System"/> when nothing (or garbage) is stored.</summary>
    ThemePreference Current { get; }

    /// <summary>Saves <paramref name="preference"/> and applies it immediately.</summary>
    void Set(ThemePreference preference);

    /// <summary>Applies the saved choice; called once at startup.</summary>
    void ApplySaved();
}

/// <summary>
/// <see cref="IThemeService"/> backed by MAUI <see cref="IPreferences"/>. Applying a theme sets
/// <see cref="Application.UserAppTheme"/>, which every AppThemeBinding in Styles.xaml follows.
/// </summary>
public class ThemeService : IThemeService
{
    /// <summary>Preferences key holding the choice.</summary>
    public const string PreferenceKey = "app_theme";

    private readonly IPreferences _preferences;
    private readonly Action<AppTheme> _apply;

    /// <param name="preferences">Where the choice is stored (the app uses <c>Preferences.Default</c>).</param>
    /// <param name="apply">Applies a theme to the app; defaults to setting <see cref="Application.UserAppTheme"/>.</param>
    public ThemeService(IPreferences preferences, Action<AppTheme>? apply = null)
    {
        _preferences = preferences;
        _apply = apply ?? (theme =>
        {
            if (Application.Current is { } app)
                app.UserAppTheme = theme;
        });
    }

    public ThemePreference Current =>
        Enum.TryParse<ThemePreference>(_preferences.Get(PreferenceKey, nameof(ThemePreference.System)), out var value)
        && Enum.IsDefined(value)
            ? value
            : ThemePreference.System;

    public void Set(ThemePreference preference)
    {
        _preferences.Set(PreferenceKey, preference.ToString());
        _apply(ToAppTheme(preference));
    }

    public void ApplySaved() => _apply(ToAppTheme(Current));

    /// <summary>Maps a choice to MAUI's theme; <see cref="AppTheme.Unspecified"/> means "follow the system".</summary>
    public static AppTheme ToAppTheme(ThemePreference preference) => preference switch
    {
        ThemePreference.Light => AppTheme.Light,
        ThemePreference.Dark => AppTheme.Dark,
        _ => AppTheme.Unspecified
    };
}
