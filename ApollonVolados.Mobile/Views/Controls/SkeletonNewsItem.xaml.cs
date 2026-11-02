namespace ApollonVolados.Mobile.Views.Controls;

/// <summary>News-row placeholder that pulses only while it is attached to a window.</summary>
public partial class SkeletonNewsItem : ContentView
{
    private CancellationTokenSource? _pulse;

    public SkeletonNewsItem()
    {
        InitializeComponent();
        Loaded += (_, _) => StartPulse();
        Unloaded += (_, _) => _pulse?.Cancel();
    }

    private async void StartPulse()
    {
        _pulse?.Cancel();
        _pulse = new CancellationTokenSource();
        var token = _pulse.Token;

        try
        {
            while (!token.IsCancellationRequested)
            {
                await this.FadeToAsync(0.5, 600, Easing.CubicInOut);
                await this.FadeToAsync(1, 600, Easing.CubicInOut);
            }
        }
        catch (TaskCanceledException)
        {
            // View detached mid-animation.
        }
    }
}
