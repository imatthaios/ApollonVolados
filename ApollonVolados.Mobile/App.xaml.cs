using ApollonVolados.Mobile.Views;

namespace ApollonVolados.Mobile;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(TeamWebPage), typeof(TeamWebPage));
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}