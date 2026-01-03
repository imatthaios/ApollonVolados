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
        LoadingProgress.Progress = 0;
        LoadingProgress.IsVisible = true;

        _ = AnimateProgressAsync();
    }

    private void OnNavigated(object sender, WebNavigatedEventArgs e)
    {
        LoadingProgress.Progress = 1;

        Device.StartTimer(TimeSpan.FromMilliseconds(200), () =>
        {
            LoadingProgress.IsVisible = false;
            LoadingProgress.Progress = 0;
            return false;
        });
    }

    private async Task AnimateProgressAsync()
    {
        // fake smooth progress μέχρι ~85%
        while (LoadingProgress.IsVisible && LoadingProgress.Progress < 0.85)
        {
            LoadingProgress.Progress += 0.03;
            await Task.Delay(40);
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}