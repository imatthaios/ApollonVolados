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
        // Show loader immediately
        LoadingProgress.IsVisible = true;
        LoadingProgress.Progress = 0.3; // Jump to start to show activity
    }

    private void OnNavigated(object sender, WebNavigatedEventArgs e)
    {
        // Finish progress
        LoadingProgress.Progress = 1.0;

        // Hide after a short delay using modern Dispatcher
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(200), () =>
        {
            LoadingProgress.IsVisible = false;
            LoadingProgress.Progress = 0;
            return false; // Stop timer
        });
    }

    // REMOVED: AnimateProgressAsync() - Faking progress is bad UX and causes infinite loops.
    // The webview will naturally fire OnNavigated when done.

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}