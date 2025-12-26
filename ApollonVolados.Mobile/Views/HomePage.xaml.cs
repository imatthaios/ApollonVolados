using Microsoft.Maui.Controls;

namespace ApollonVolados.Mobile.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        HeroLogo.Opacity = 0;
        HeroLogo.Scale = 0.9;

        await Task.WhenAll(
            HeroLogo.FadeTo(1, 250, Easing.CubicOut),
            HeroLogo.ScaleTo(1, 250, Easing.CubicOut)
        );

        await AnimateCard(CardMatch, 0);
        await AnimateCard(CardAnnouncement, 80);
    }

    private async Task AnimateCard(VisualElement element, int delay)
    {
        element.Opacity = 0;
        element.TranslationY = 20;

        if (delay > 0)
            await Task.Delay(delay);

        await Task.WhenAll(
            element.FadeTo(1, 220, Easing.CubicOut),
            element.TranslateTo(0, 0, 220, Easing.CubicOut)
        );
    }

    private async void OnButtonPressed(object sender, EventArgs e)
    {
        if (sender is VisualElement v)
            await v.ScaleTo(0.96, 80, Easing.CubicOut);
    }

    private async void OnButtonReleased(object sender, EventArgs e)
    {
        if (sender is VisualElement v)
            await v.ScaleTo(1, 80, Easing.CubicOut);
    }
}