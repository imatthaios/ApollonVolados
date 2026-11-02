using ApollonVolados.Mobile.Services;

namespace ApollonVolados.Mobile.Views;

[QueryProperty(nameof(Url), Routes.UrlParameter)]
public partial class TeamWebPage : ContentPage
{
    private string? _url;

    public string? Url
    {
        get => _url;
        set
        {
            _url = Uri.UnescapeDataString(value ?? string.Empty);
            Load();
        }
    }

    public TeamWebPage()
    {
        InitializeComponent();
        WebProgress.Attach(Browser, LoadingProgress);
    }

    private void Load()
    {
        if (!string.IsNullOrWhiteSpace(_url))
        {
            Browser.Source = _url;
        }
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}