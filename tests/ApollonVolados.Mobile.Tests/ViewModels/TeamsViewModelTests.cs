using ApollonVolados.Mobile.Models;
using ApollonVolados.Mobile.Services;
using ApollonVolados.Mobile.Tests.Fakes;
using ApollonVolados.Mobile.ViewModels;

namespace ApollonVolados.Mobile.Tests.ViewModels;

public class TeamsViewModelTests
{
    private readonly FakeNavigationService _navigation = new();

    private TeamsViewModel CreateSut() => new(_navigation);

    [Fact]
    public void Lists_six_departments()
    {
        Assert.Equal(6, CreateSut().Categories.Count);
    }

    [Fact]
    public void Every_department_has_title_subtitle_and_icon()
    {
        Assert.All(CreateSut().Categories, c =>
        {
            Assert.False(string.IsNullOrWhiteSpace(c.Title));
            Assert.False(string.IsNullOrWhiteSpace(c.Subtitle));
            Assert.False(string.IsNullOrWhiteSpace(c.Icon));
        });
    }

    [Fact]
    public void Every_department_links_to_the_club_website_over_https()
    {
        Assert.All(CreateSut().Categories, c =>
        {
            var uri = new Uri(c.Url);
            Assert.Equal("https", uri.Scheme);
            Assert.Equal("apollonvolados.gr", uri.Host);
        });
    }

    [Fact]
    public void Department_titles_and_urls_are_unique()
    {
        var categories = CreateSut().Categories;

        Assert.Equal(categories.Count, categories.Select(c => c.Title).Distinct().Count());
        Assert.Equal(categories.Count, categories.Select(c => c.Url).Distinct().Count());
    }

    [Fact]
    public async Task OpenTeam_navigates_to_team_web_page_with_url()
    {
        var sut = CreateSut();
        var team = sut.Categories[0];

        await sut.OpenTeamAsync(team);

        var (route, parameters) = Assert.Single(_navigation.Navigations);
        Assert.Equal(Routes.TeamWeb, route);
        Assert.Equal(team.Url, parameters![Routes.UrlParameter]);
    }

    [Fact]
    public async Task OpenTeam_command_navigates()
    {
        var sut = CreateSut();

        await sut.OpenTeamCommand.ExecuteAsync(sut.Categories[1]);

        Assert.Single(_navigation.Navigations);
    }

    [Fact]
    public async Task OpenTeam_ignores_null()
    {
        await CreateSut().OpenTeamAsync(null);

        Assert.Empty(_navigation.Navigations);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task OpenTeam_ignores_team_without_url(string url)
    {
        await CreateSut().OpenTeamAsync(new TeamCategory { Title = "X", Url = url });

        Assert.Empty(_navigation.Navigations);
    }
}
