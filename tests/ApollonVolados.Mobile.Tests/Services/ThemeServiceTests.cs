using ApollonVolados.Mobile.Services;
using ApollonVolados.Mobile.Tests.Fakes;
using Microsoft.Maui.ApplicationModel;

namespace ApollonVolados.Mobile.Tests.Services;

public class ThemeServiceTests
{
    private readonly FakePreferences _preferences = new();
    private readonly List<AppTheme> _applied = [];

    private ThemeService CreateSut() => new(_preferences, _applied.Add);

    [Fact]
    public void Defaults_to_following_the_system()
    {
        Assert.Equal(ThemePreference.System, CreateSut().Current);
    }

    [Theory]
    [InlineData(ThemePreference.System, AppTheme.Unspecified)]
    [InlineData(ThemePreference.Light, AppTheme.Light)]
    [InlineData(ThemePreference.Dark, AppTheme.Dark)]
    public void Set_saves_and_applies(ThemePreference preference, AppTheme expected)
    {
        var sut = CreateSut();

        sut.Set(preference);

        Assert.Equal(preference, sut.Current);
        Assert.Equal(preference.ToString(), _preferences.Values[ThemeService.PreferenceKey]);
        Assert.Equal([expected], _applied);
    }

    [Fact]
    public void Choice_survives_a_restart()
    {
        CreateSut().Set(ThemePreference.Dark);
        _applied.Clear();

        var afterRestart = CreateSut();
        afterRestart.ApplySaved();

        Assert.Equal(ThemePreference.Dark, afterRestart.Current);
        Assert.Equal([AppTheme.Dark], _applied);
    }

    [Fact]
    public void ApplySaved_with_nothing_stored_follows_the_system()
    {
        CreateSut().ApplySaved();

        Assert.Equal([AppTheme.Unspecified], _applied);
    }

    [Theory]
    [InlineData("garbage")]
    [InlineData("")]
    [InlineData("42")]
    public void Unknown_stored_values_fall_back_to_system(string stored)
    {
        _preferences.Values[ThemeService.PreferenceKey] = stored;

        Assert.Equal(ThemePreference.System, CreateSut().Current);
    }

    [Theory]
    [InlineData(ThemePreference.System, AppTheme.Unspecified)]
    [InlineData(ThemePreference.Light, AppTheme.Light)]
    [InlineData(ThemePreference.Dark, AppTheme.Dark)]
    public void Maps_preferences_to_app_themes(ThemePreference preference, AppTheme expected)
    {
        Assert.Equal(expected, ThemeService.ToAppTheme(preference));
    }
}
