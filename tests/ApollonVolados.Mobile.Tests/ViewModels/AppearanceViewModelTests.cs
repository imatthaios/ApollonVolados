using ApollonVolados.Mobile.Services;
using ApollonVolados.Mobile.Tests.Fakes;
using ApollonVolados.Mobile.ViewModels;

namespace ApollonVolados.Mobile.Tests.ViewModels;

public class AppearanceViewModelTests
{
    private readonly FakeThemeService _themes = new();

    private AppearanceViewModel CreateSut() => new(_themes);

    [Fact]
    public void Offers_automatic_light_and_dark_in_order()
    {
        var sut = CreateSut();

        Assert.Equal([ThemePreference.System, ThemePreference.Light, ThemePreference.Dark], sut.Options.Select(o => o.Value));
        Assert.Equal(["Αυτόματο", "Φωτεινό", "Σκοτεινό"], sut.Options.Select(o => o.Label));
        Assert.Same(sut.Options[0], sut.System);
        Assert.Same(sut.Options[1], sut.Light);
        Assert.Same(sut.Options[2], sut.Dark);
    }

    [Theory]
    [InlineData(ThemePreference.System)]
    [InlineData(ThemePreference.Light)]
    [InlineData(ThemePreference.Dark)]
    public void Marks_the_saved_choice_as_selected(ThemePreference saved)
    {
        _themes.Current = saved;

        var sut = CreateSut();

        Assert.Equal(saved, Assert.Single(sut.Options, o => o.IsSelected).Value);
    }

    [Fact]
    public void Selecting_an_option_applies_it_and_moves_the_selection()
    {
        var sut = CreateSut();

        sut.Dark.SelectCommand.Execute(null);

        Assert.Equal([ThemePreference.Dark], _themes.Changes);
        Assert.True(sut.Dark.IsSelected);
        Assert.False(sut.System.IsSelected);
        Assert.False(sut.Light.IsSelected);
    }

    [Fact]
    public void Selection_change_is_notified()
    {
        var sut = CreateSut();
        var changed = new List<string?>();
        sut.Light.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        sut.Light.SelectCommand.Execute(null);

        Assert.Contains(nameof(ThemeOption.IsSelected), changed);
    }

    [Fact]
    public void Switching_back_to_automatic_follows_the_system_again()
    {
        var sut = CreateSut();
        sut.Light.SelectCommand.Execute(null);

        sut.System.SelectCommand.Execute(null);

        Assert.Equal([ThemePreference.Light, ThemePreference.System], _themes.Changes);
        Assert.True(sut.System.IsSelected);
    }
}
