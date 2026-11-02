using ApollonVolados.Mobile.Services;

namespace ApollonVolados.Mobile.Views;

public partial class AnimatedSplashPage : ContentPage
{
    public AnimatedSplashPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // INITIAL STATE
            LogoContainer.Opacity = 0;
            LogoContainer.Scale = 0.45;
            LogoContainer.Rotation = -18;
            LightOverlay.Opacity = 0;

            // 1️⃣ FADE + SCALE IN (cinematic entrance)
            await Task.WhenAll(
                LogoContainer.FadeToAsync(1, 900, Easing.CubicOut),
                LogoContainer.ScaleToAsync(1.05, 900, Easing.CubicOut),
                LogoContainer.RotateToAsync(6, 900, Easing.CubicOut)
            );

            // 2️⃣ SETTLE BACK (natural physics feel)
            await Task.WhenAll(
                LogoContainer.ScaleToAsync(1, 450, Easing.SpringOut),
                LogoContainer.RotateToAsync(0, 450, Easing.SpringOut)
            );

            // 3️⃣ SOFT LIGHT WAVE
            await LightOverlay.FadeToAsync(0.08, 350, Easing.CubicInOut);
            await LightOverlay.FadeToAsync(0, 350, Easing.CubicInOut);

            // 4️⃣ HOLD (branding moment)
            await Task.Delay(600);

            // 5️⃣ EXIT FADE
            await Task.WhenAll(
                LogoContainer.FadeToAsync(0, 400, Easing.CubicIn),
                this.FadeToAsync(0, 400, Easing.CubicIn)
            );

            // GO HOME TAB
            await Shell.Current.GoToAsync(Routes.Home);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
    }
}