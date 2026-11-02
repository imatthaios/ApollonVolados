using System.Net;
using System.Text.Json;
using ApollonVolados.Mobile.Models;
using ApollonVolados.Mobile.Services;
using ApollonVolados.Mobile.Tests.Fakes;
using Microsoft.Extensions.Time.Testing;

namespace ApollonVolados.Mobile.Tests.Services;

public sealed class WordPressServiceTests : IDisposable
{
    private readonly TempDirectory _dir = new();
    private readonly FakeTimeProvider _clock = new(DateTimeOffset.UtcNow);
    private readonly NewsCacheService _cache;

    public WordPressServiceTests()
    {
        _cache = new NewsCacheService(_dir.Path, _clock);
    }

    public void Dispose() => _dir.Dispose();

    private static string PostsJson(params int[] ids) =>
        JsonSerializer.Serialize(ids.Select(id => TestPosts.Create(id)).ToList());

    private WordPressService CreateSut(StubHttpHandler handler) => new(new HttpClient(handler), _cache);

    private async Task SeedCacheAsync(params int[] ids) =>
        await _cache.SaveAsync(ids.Select(id => TestPosts.Create(id)).ToList());

    private void ExpireCache() => _clock.Advance(NewsCacheService.DefaultCacheDuration + TimeSpan.FromMinutes(1));

    [Fact]
    public async Task Fetches_from_posts_endpoint_when_no_cache()
    {
        var http = StubHttpHandler.Json(PostsJson(1, 2));

        var posts = await CreateSut(http).GetLatestPostsAsync();

        Assert.Equal([1, 2], posts.Select(p => p.Id));
        var request = Assert.Single(http.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal(WordPressService.PostsUrl, request.RequestUri!.ToString());
    }

    [Fact]
    public void Posts_url_requests_embedded_media()
    {
        Assert.StartsWith("https://", WordPressService.PostsUrl);
        Assert.Contains("_embed", WordPressService.PostsUrl);
    }

    [Fact]
    public async Task Successful_fetch_is_written_to_cache()
    {
        await CreateSut(StubHttpHandler.Json(PostsJson(10))).GetLatestPostsAsync();

        var cached = await _cache.GetCachedAsync();
        Assert.Equal([10], cached!.Select(p => p.Id));
    }

    [Fact]
    public async Task Fresh_cache_is_served_without_network()
    {
        await SeedCacheAsync(5);
        var http = StubHttpHandler.Throws(new InvalidOperationException("network must not be called"));

        var posts = await CreateSut(http).GetLatestPostsAsync();

        Assert.Equal([5], posts.Select(p => p.Id));
        Assert.Empty(http.Requests);
    }

    [Fact]
    public async Task Expired_cache_triggers_network_fetch()
    {
        await SeedCacheAsync(5);
        ExpireCache();
        var http = StubHttpHandler.Json(PostsJson(6));

        var posts = await CreateSut(http).GetLatestPostsAsync();

        Assert.Equal([6], posts.Select(p => p.Id));
        Assert.Single(http.Requests);
    }

    [Fact]
    public async Task Force_refresh_bypasses_fresh_cache()
    {
        await SeedCacheAsync(5);
        var http = StubHttpHandler.Json(PostsJson(6, 7));

        var posts = await CreateSut(http).GetLatestPostsAsync(forceRefresh: true);

        Assert.Equal([6, 7], posts.Select(p => p.Id));
        Assert.Single(http.Requests);
    }

    [Fact]
    public async Task Json_null_body_returns_empty_list()
    {
        var posts = await CreateSut(StubHttpHandler.Json("null")).GetLatestPostsAsync();

        Assert.Empty(posts);
    }

    public static TheoryData<StubHttpHandler> FailingHandlers => new()
    {
        StubHttpHandler.Throws(new HttpRequestException("offline")),
        StubHttpHandler.Throws(new TaskCanceledException("timeout")),
        StubHttpHandler.Json("{}", HttpStatusCode.InternalServerError),
        StubHttpHandler.Json("<html>maintenance</html>"),
    };

    [Theory]
    [MemberData(nameof(FailingHandlers))]
    public async Task Network_failure_falls_back_to_expired_cache(StubHttpHandler http)
    {
        await SeedCacheAsync(3, 4);
        ExpireCache();

        var posts = await CreateSut(http).GetLatestPostsAsync();

        Assert.Equal([3, 4], posts.Select(p => p.Id));
    }

    [Theory]
    [MemberData(nameof(FailingHandlers))]
    public async Task Network_failure_on_force_refresh_falls_back_to_cache(StubHttpHandler http)
    {
        await SeedCacheAsync(8);

        var posts = await CreateSut(http).GetLatestPostsAsync(forceRefresh: true);

        Assert.Equal([8], posts.Select(p => p.Id));
    }

    [Fact]
    public async Task Network_failure_without_cache_throws()
    {
        var http = StubHttpHandler.Throws(new HttpRequestException("offline"));

        await Assert.ThrowsAsync<HttpRequestException>(() => CreateSut(http).GetLatestPostsAsync());
    }

    [Fact]
    public async Task Server_error_without_cache_throws()
    {
        var http = StubHttpHandler.Json("{}", HttpStatusCode.ServiceUnavailable);

        await Assert.ThrowsAsync<HttpRequestException>(() => CreateSut(http).GetLatestPostsAsync());
    }

    [Fact]
    public async Task Invalid_json_without_cache_throws()
    {
        var http = StubHttpHandler.Json("<html/>");

        await Assert.ThrowsAsync<JsonException>(() => CreateSut(http).GetLatestPostsAsync());
    }

    [Fact]
    public async Task Caller_cancellation_is_not_swallowed()
    {
        await SeedCacheAsync(1);
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        var http = new StubHttpHandler((_, ct) => Task.FromCanceled<HttpResponseMessage>(ct));

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => CreateSut(http).GetLatestPostsAsync(forceRefresh: true, cancellationToken: cts.Token));
    }

    [Fact]
    public async Task Cache_write_failure_still_returns_posts()
    {
        // Point the cache at a path that is an existing *file*, so creating the directory fails.
        var blocker = Path.Combine(_dir.Path, "blocker");
        await File.WriteAllTextAsync(blocker, "x");
        var brokenCache = new NewsCacheService(Path.Combine(blocker, "sub"), _clock);
        var sut = new WordPressService(new HttpClient(StubHttpHandler.Json(PostsJson(1))), brokenCache);

        var posts = await sut.GetLatestPostsAsync();

        Assert.Equal([1], posts.Select(p => p.Id));
    }

    [Fact]
    public async Task Parses_wordpress_fields_from_network()
    {
        const string json = """
            [{ "id": 99, "link": "https://x/", "date": "2025-06-01T10:00:00",
               "title": { "rendered": "A &amp; B" },
               "_embedded": { "wp:featuredmedia": [ { "source_url": "https://x/i.png" } ] } }]
            """;

        var post = Assert.Single(await CreateSut(StubHttpHandler.Json(json)).GetLatestPostsAsync());

        Assert.Equal(99, post.Id);
        Assert.Equal("A & B", post.TitleText);
        Assert.Equal("https://x/i.png", post.ImageUrl);
    }

    [Fact]
    public void Implements_interface()
    {
        Assert.IsAssignableFrom<IWordPressService>(CreateSut(StubHttpHandler.Json("[]")));
    }
}
