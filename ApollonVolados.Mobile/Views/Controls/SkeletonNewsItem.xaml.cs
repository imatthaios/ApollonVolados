namespace ApollonVolados.Mobile.Views.Controls;

public partial class SkeletonNewsItem : ContentView
{
    public SkeletonNewsItem()
    {
        InitializeComponent();
        StartShimmer();
    }

    private async void StartShimmer()
    {
        while (IsVisible)
        {
            await this.FadeTo(0.5, 600, Easing.CubicInOut);
            await this.FadeTo(1, 600, Easing.CubicInOut);
        }
    }
}