using Microsoft.Maui.Controls;

namespace ApollonVolados.Mobile.Views;

[QueryProperty(nameof(Url), "url")]
public partial class TeamWebPage : ContentPage
{
    private string _url;

    public string Url
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
    }

    private void Load()
    {
        if (!string.IsNullOrWhiteSpace(_url))
        {
            Browser.Source = _url;
        }
    }

    private void OnNavigating(object sender, WebNavigatingEventArgs e)
    {
        Loader.IsRunning = true;
        Loader.IsVisible = true;
    }

    private void OnNavigated(object sender, WebNavigatedEventArgs e)
    {
        Loader.IsRunning = false;
        Loader.IsVisible = false;
    }
}
