using ApollonVolados.Mobile.Services;

namespace ApollonVolados.Mobile;

public partial class App : Application
{
	public App(IThemeService themes)
	{
		InitializeComponent();
		// Apply the saved Automatic / Light / Dark choice before the first page renders.
		themes.ApplySaved();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}
