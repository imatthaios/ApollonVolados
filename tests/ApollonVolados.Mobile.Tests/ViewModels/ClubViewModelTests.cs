using ApollonVolados.Mobile.Tests.Fakes;
using ApollonVolados.Mobile.ViewModels;

namespace ApollonVolados.Mobile.Tests.ViewModels;

public class ClubViewModelTests
{
    private readonly FakeLauncher _launcher = new();
    private readonly FakePhoneDialer _dialer = new();
    private readonly FakeDialogService _dialogs = new();

    private ClubViewModel CreateSut() => new(_launcher, _dialer, _dialogs);

    [Fact]
    public async Task Call_uses_dialer_when_supported()
    {
        _dialer.IsSupported = true;

        await CreateSut().CallAsync();

        Assert.Equal([ClubViewModel.PhoneNumber], _dialer.Dialled);
        Assert.Empty(_launcher.Opened);
        Assert.Empty(_dialogs.Alerts);
    }

    [Fact]
    public async Task Call_falls_back_to_tel_link_without_dialer()
    {
        _dialer.IsSupported = false;

        await CreateSut().CallAsync();

        var uri = Assert.Single(_launcher.Opened);
        Assert.Equal("tel:" + ClubViewModel.PhoneNumber, uri.OriginalString);
        Assert.Empty(_dialer.Dialled);
        Assert.Empty(_dialogs.Alerts);
    }

    [Fact]
    public async Task Call_alerts_when_tel_link_cannot_open()
    {
        _launcher.Result = false;

        await CreateSut().CallAsync();

        Assert.Single(_dialogs.Alerts);
    }

    [Fact]
    public async Task Call_alerts_when_dialer_throws()
    {
        _dialer.IsSupported = true;
        _dialer.ThrowOnOpen = new InvalidOperationException("no dialer");

        await CreateSut().CallAsync();

        Assert.Single(_dialogs.Alerts);
    }

    [Fact]
    public async Task Email_opens_mailto()
    {
        await CreateSut().EmailAsync();

        var uri = Assert.Single(_launcher.Opened);
        Assert.Equal("mailto", uri.Scheme);
        Assert.Equal("mailto:" + ClubViewModel.EmailAddress, uri.OriginalString);
        Assert.Empty(_dialogs.Alerts);
    }

    [Fact]
    public async Task Website_opens_club_site()
    {
        await CreateSut().WebsiteAsync();

        Assert.Equal(new Uri(ClubViewModel.WebsiteUrl), Assert.Single(_launcher.Opened));
        Assert.Empty(_dialogs.Alerts);
    }

    [Fact]
    public async Task Map_opens_google_maps_search()
    {
        await CreateSut().MapAsync();

        var uri = Assert.Single(_launcher.Opened);
        Assert.Equal("www.google.com", uri.Host);
        Assert.Equal("/maps/search/", uri.AbsolutePath);
    }

    [Fact]
    public void Map_url_escapes_greek_query()
    {
        var uri = new Uri(ClubViewModel.MapUrl);

        Assert.DoesNotContain(" ", ClubViewModel.MapUrl);
        Assert.Contains("api=1", uri.Query);
        var query = uri.Query.TrimStart('?').Split('&')
            .Select(p => p.Split('=', 2))
            .ToDictionary(p => p[0], p => Uri.UnescapeDataString(p[1]));
        Assert.Equal(ClubViewModel.StadiumQuery, query["query"]);
    }

    public static TheoryData<string> LauncherActions => new() { "Email", "Website", "Map" };

    [Theory]
    [MemberData(nameof(LauncherActions))]
    public async Task Launcher_actions_alert_when_launcher_returns_false(string action)
    {
        _launcher.Result = false;

        await Run(CreateSut(), action);

        Assert.Single(_dialogs.Alerts);
    }

    [Theory]
    [MemberData(nameof(LauncherActions))]
    public async Task Launcher_actions_alert_instead_of_crashing_when_launcher_throws(string action)
    {
        _launcher.ThrowOnOpen = new InvalidOperationException("simulator error -10814");

        await Run(CreateSut(), action);

        Assert.Single(_dialogs.Alerts);
    }

    [Fact]
    public async Task Commands_are_bound_to_actions()
    {
        var sut = CreateSut();

        await sut.EmailCommand.ExecuteAsync(null);
        await sut.WebsiteCommand.ExecuteAsync(null);
        await sut.MapCommand.ExecuteAsync(null);
        await sut.CallCommand.ExecuteAsync(null);

        Assert.Equal(4, _launcher.Opened.Count);
    }

    [Fact]
    public void History_text_mentions_founding_year()
    {
        Assert.Contains("1924", CreateSut().HistoryText);
    }

    private static Task Run(ClubViewModel sut, string action) => action switch
    {
        "Email" => sut.EmailAsync(),
        "Website" => sut.WebsiteAsync(),
        "Map" => sut.MapAsync(),
        _ => throw new ArgumentOutOfRangeException(nameof(action))
    };
}
