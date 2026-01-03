using ApollonVolados.Mobile.Views;
using Microsoft.Maui.Controls;

namespace ApollonVolados.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(NewsDetailPage), typeof(NewsDetailPage));
    }
}