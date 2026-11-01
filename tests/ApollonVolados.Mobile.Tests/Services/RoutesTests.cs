using System.Reflection;
using ApollonVolados.Mobile.Services;
using ApollonVolados.Mobile.Views;
using Microsoft.Maui.Controls;

namespace ApollonVolados.Mobile.Tests.Services;

/// <summary>
/// Shell query keys are case-sensitive; a mismatch silently leaves the page blank.
/// These tests pin the receiving pages to the keys the view models send.
/// </summary>
public class RoutesTests
{
    [Theory]
    [InlineData(typeof(NewsDetailPage))]
    [InlineData(typeof(TeamWebPage))]
    public void WebView_pages_receive_url_parameter(Type page)
    {
        var attribute = page.GetCustomAttribute<QueryPropertyAttribute>();

        Assert.NotNull(attribute);
        Assert.Equal(Routes.UrlParameter, attribute.QueryId);
        Assert.NotNull(page.GetProperty(attribute.Name));
    }

    [Fact]
    public void Pushed_page_routes_match_page_type_names()
    {
        Assert.Equal(nameof(NewsDetailPage), Routes.NewsDetail);
        Assert.Equal(nameof(TeamWebPage), Routes.TeamWeb);
    }

    [Fact]
    public void Home_route_is_absolute()
    {
        Assert.StartsWith("//", Routes.Home);
    }
}
