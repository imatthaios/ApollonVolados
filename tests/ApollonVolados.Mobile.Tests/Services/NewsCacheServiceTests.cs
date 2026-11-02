using ApollonVolados.Mobile.Services;
using ApollonVolados.Mobile.Tests.Fakes;
using Microsoft.Extensions.Time.Testing;

namespace ApollonVolados.Mobile.Tests.Services;

public sealed class NewsCacheServiceTests : IDisposable
{
    private readonly TempDirectory _dir = new();
    private readonly FakeTimeProvider _clock = new(new DateTimeOffset(2025, 1, 1, 12, 0, 0, TimeSpan.Zero));

    public void Dispose() => _dir.Dispose();

    private NewsCacheService CreateSut(TimeSpan? duration = null) => new(_dir.Path, _clock, duration);

    private string CachePath => Path.Combine(_dir.Path, NewsCacheService.CacheFileName);

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Constructor_rejects_blank_directory(string? dir)
    {
        Assert.ThrowsAny<ArgumentException>(() => new NewsCacheService(dir!));
    }

    [Fact]
    public void Default_duration_is_six_hours()
    {
        Assert.Equal(TimeSpan.FromHours(6), NewsCacheService.DefaultCacheDuration);
    }

    [Fact]
    public async Task Returns_null_when_no_cache_file()
    {
        Assert.Null(await CreateSut().GetCachedAsync());
        Assert.Null(await CreateSut().GetCachedAsync(allowExpired: true));
    }

    [Fact]
    public async Task Save_then_get_round_trips_posts()
    {
        var sut = CreateSut();
        await sut.SaveAsync([TestPosts.Create(1, "Πρώτο"), TestPosts.Create(2, "Δεύτερο")]);

        var cached = await sut.GetCachedAsync();

        Assert.NotNull(cached);
        Assert.Equal([1, 2], cached.Select(p => p.Id));
        Assert.Equal("Πρώτο", cached[0].TitleText);
    }

    [Fact]
    public async Task Empty_list_round_trips()
    {
        var sut = CreateSut();
        await sut.SaveAsync([]);

        var cached = await sut.GetCachedAsync();

        Assert.NotNull(cached);
        Assert.Empty(cached);
    }

    [Fact]
    public async Task Save_overwrites_previous_cache()
    {
        var sut = CreateSut();
        await sut.SaveAsync([TestPosts.Create(1), TestPosts.Create(2), TestPosts.Create(3)]);
        await sut.SaveAsync([TestPosts.Create(9)]);

        var cached = await sut.GetCachedAsync();

        Assert.Equal([9], cached!.Select(p => p.Id));
    }

    [Fact]
    public async Task Save_rejects_null()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => CreateSut().SaveAsync(null!));
    }

    [Fact]
    public async Task Save_creates_missing_directory()
    {
        var nested = Path.Combine(_dir.Path, "a", "b");
        var sut = new NewsCacheService(nested, _clock);

        await sut.SaveAsync([TestPosts.Create(1)]);

        Assert.True(File.Exists(Path.Combine(nested, NewsCacheService.CacheFileName)));
    }

    [Fact]
    public async Task Save_records_timestamp_and_latest_post_id()
    {
        await CreateSut().SaveAsync([TestPosts.Create(5), TestPosts.Create(42), TestPosts.Create(17)]);

        var json = await File.ReadAllTextAsync(CachePath);

        Assert.Contains("\"LatestPostId\":42", json);
        Assert.Contains("\"CachedAt\":\"2025-01-01T12:00:00", json);
    }

    [Fact]
    public async Task Cache_is_fresh_just_inside_duration()
    {
        var sut = CreateSut(TimeSpan.FromHours(6));
        await sut.SaveAsync([TestPosts.Create(1)]);

        _clock.Advance(TimeSpan.FromHours(6));

        Assert.NotNull(await sut.GetCachedAsync());
    }

    [Fact]
    public async Task Cache_expires_after_duration()
    {
        var sut = CreateSut(TimeSpan.FromHours(6));
        await sut.SaveAsync([TestPosts.Create(1)]);

        _clock.Advance(TimeSpan.FromHours(6) + TimeSpan.FromSeconds(1));

        Assert.Null(await sut.GetCachedAsync());
    }

    [Fact]
    public async Task Expired_cache_is_returned_when_allowed()
    {
        var sut = CreateSut(TimeSpan.FromMinutes(1));
        await sut.SaveAsync([TestPosts.Create(1)]);

        _clock.Advance(TimeSpan.FromDays(30));

        var stale = await sut.GetCachedAsync(allowExpired: true);
        Assert.Equal([1], stale!.Select(p => p.Id));
    }

    [Fact]
    public async Task Custom_duration_is_respected()
    {
        var sut = CreateSut(TimeSpan.FromMinutes(5));
        await sut.SaveAsync([TestPosts.Create(1)]);

        _clock.Advance(TimeSpan.FromMinutes(4));
        Assert.NotNull(await sut.GetCachedAsync());

        _clock.Advance(TimeSpan.FromMinutes(2));
        Assert.Null(await sut.GetCachedAsync());
    }

    [Theory]
    [InlineData("not json at all")]
    [InlineData("{\"CachedAt\": 12")]
    [InlineData("")]
    public async Task Corrupt_cache_is_treated_as_miss(string content)
    {
        await File.WriteAllTextAsync(CachePath, content);

        Assert.Null(await CreateSut().GetCachedAsync());
        Assert.Null(await CreateSut().GetCachedAsync(allowExpired: true));
    }

    [Fact]
    public async Task Json_null_cache_is_treated_as_miss()
    {
        await File.WriteAllTextAsync(CachePath, "null");

        Assert.Null(await CreateSut().GetCachedAsync());
    }

    [Fact]
    public async Task Save_recovers_from_corrupt_cache()
    {
        await File.WriteAllTextAsync(CachePath, "garbage that is much longer than the replacement json will be ..........................................................................................................................................................................................................................................................................................");
        var sut = CreateSut();

        await sut.SaveAsync([TestPosts.Create(3)]);

        Assert.Equal([3], (await sut.GetCachedAsync())!.Select(p => p.Id));
    }
}
