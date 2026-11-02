using ApollonVolados.Mobile.Views;

namespace ApollonVolados.Mobile.Services;

/// <summary>
/// Shell routes and query-parameter names used across the app. Keeping them in one place
/// guarantees that the sender and the receiving page's <see cref="QueryPropertyAttribute"/>
/// agree on the exact (case-sensitive) key.
/// </summary>
public static class Routes
{
    /// <summary>Absolute route of the Home tab (declared in AppShell.xaml).</summary>
    public const string Home = "//home";

    /// <summary>Pushed page that shows a news article in a WebView.</summary>
    public const string NewsDetail = nameof(NewsDetailPage);

    /// <summary>Pushed page that shows a team's page from the club website in a WebView.</summary>
    public const string TeamWeb = nameof(TeamWebPage);

    /// <summary>Query parameter carrying the URL to load in a WebView page.</summary>
    public const string UrlParameter = "url";
}
