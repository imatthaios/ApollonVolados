namespace ApollonVolados.Mobile.Views;

/// <summary>Drives a thin <see cref="ProgressBar"/> from a <see cref="WebView"/>'s navigation events.</summary>
internal static class WebProgress
{
    public static void Attach(WebView webView, ProgressBar progress)
    {
        webView.Navigating += (_, _) =>
        {
            progress.IsVisible = true;
            progress.Progress = 0.3; // WebView reports no real progress; show activity immediately
        };

        webView.Navigated += (_, _) =>
        {
            progress.Progress = 1.0;
            webView.Dispatcher.StartTimer(TimeSpan.FromMilliseconds(200), () =>
            {
                progress.IsVisible = false;
                progress.Progress = 0;
                return false;
            });
        };
    }
}
