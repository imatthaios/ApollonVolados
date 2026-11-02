using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ApollonVolados.Mobile.Services;

namespace ApollonVolados.Mobile.ViewModels;

/// <summary>Backs the "Εμφάνιση" (appearance) switch on the Club tab: Automatic, Light or Dark.</summary>
public partial class AppearanceViewModel : ObservableObject
{
    private readonly IThemeService _themes;

    public AppearanceViewModel(IThemeService themes)
    {
        _themes = themes;
        Options =
        [
            new ThemeOption("Αυτόματο", ThemePreference.System, Select),
            new ThemeOption("Φωτεινό", ThemePreference.Light, Select),
            new ThemeOption("Σκοτεινό", ThemePreference.Dark, Select)
        ];
        MarkSelected(_themes.Current);
    }

    /// <summary>The three choices, in display order.</summary>
    public IReadOnlyList<ThemeOption> Options { get; }

    public ThemeOption System => Options[0];
    public ThemeOption Light => Options[1];
    public ThemeOption Dark => Options[2];

    private void Select(ThemePreference preference)
    {
        _themes.Set(preference);
        MarkSelected(preference);
    }

    private void MarkSelected(ThemePreference preference)
    {
        foreach (var option in Options)
            option.IsSelected = option.Value == preference;
    }
}

/// <summary>One segment of the appearance switch.</summary>
public partial class ThemeOption : ObservableObject
{
    private readonly Action<ThemePreference> _select;

    public ThemeOption(string label, ThemePreference value, Action<ThemePreference> select)
    {
        Label = label;
        Value = value;
        _select = select;
    }

    public string Label { get; }
    public ThemePreference Value { get; }

    [ObservableProperty]
    private bool isSelected;

    [RelayCommand]
    private void Select() => _select(Value);
}
