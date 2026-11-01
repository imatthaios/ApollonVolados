using ApollonVolados.Mobile.Services;
using ApollonVolados.Mobile.Views;

namespace ApollonVolados.Mobile;

/// <summary>
/// Root Shell: a splash page followed by the four tabs (Home, News, Teams, Club).
/// Pushed pages are registered here as routes.
/// </summary>
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(Routes.NewsDetail, typeof(NewsDetailPage));
        Routing.RegisterRoute(Routes.TeamWeb, typeof(TeamWebPage));
    }
}
