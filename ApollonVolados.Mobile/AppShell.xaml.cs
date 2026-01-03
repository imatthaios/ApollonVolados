using ApollonVolados.Mobile.Views;

namespace ApollonVolados.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(NewsDetailPage), typeof(NewsDetailPage));
    }
}