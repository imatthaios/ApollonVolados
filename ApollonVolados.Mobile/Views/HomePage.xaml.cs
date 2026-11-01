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

        if (HeroLogo == null)
            return;

        await HeroLogo.FadeToAsync(1, 400, Easing.CubicOut);
    }
}