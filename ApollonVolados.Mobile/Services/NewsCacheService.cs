using System.Text.Json;
using ApollonVolados.Mobile.Models;

namespace ApollonVolados.Mobile.Services;

/// <summary>
/// Persists the latest WordPress posts as JSON on disk so the news tab opens instantly
/// and still has something to show when the device is offline.
/// </summary>
public class NewsCacheService
{
    /// <summary>File name of the cache inside the cache directory.</summary>
    public const string CacheFileName = "news_cache.json";

    /// <summary>How long a cached copy counts as fresh when no duration is supplied.</summary>
    public static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromHours(6);

    private readonly string _path;
    private readonly TimeProvider _timeProvider;
    private readonly TimeSpan _cacheDuration;

    /// <param name="cacheDirectory">Directory to store the cache in (the app uses <c>FileSystem.CacheDirectory</c>).</param>
    /// <param name="timeProvider">Clock used to stamp and expire entries; defaults to the system clock.</param>
    /// <param name="cacheDuration">Freshness window; defaults to <see cref="DefaultCacheDuration"/>.</param>
    public NewsCacheService(
        string cacheDirectory,
        TimeProvider? timeProvider = null,
        TimeSpan? cacheDuration = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cacheDirectory);

        _path = Path.Combine(cacheDirectory, CacheFileName);
        _timeProvider = timeProvider ?? TimeProvider.System;
        _cacheDuration = cacheDuration ?? DefaultCacheDuration;
    }

    /// <summary>
    /// Returns the cached posts, or <c>null</c> if there is no cache, it cannot be read,
    /// or it has expired (unless <paramref name="allowExpired"/> is <c>true</c>).
    /// </summary>
    /// <param name="allowExpired">Return expired data too; used as an offline fallback.</param>
    public async Task<List<WpPost>?> GetCachedAsync(bool allowExpired = false)
    {
        if (!File.Exists(_path))
            return null;

        CachedNews? cache;
        try
        {
            await using var stream = File.OpenRead(_path);
            cache = await JsonSerializer.DeserializeAsync<CachedNews>(stream);
        }
        catch (Exception ex) when (ex is JsonException or IOException)
        {
            // A corrupt or locked cache must never break the news tab; treat it as a miss.
            return null;
        }

        if (cache is null)
            return null;

        if (!allowExpired && _timeProvider.GetUtcNow().UtcDateTime - cache.CachedAt > _cacheDuration)
            return null;

        return cache.Items;
    }

    /// <summary>Overwrites the cache with <paramref name="items"/>, stamped with the current time.</summary>
    public async Task SaveAsync(List<WpPost> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        var cache = new CachedNews
        {
            CachedAt = _timeProvider.GetUtcNow().UtcDateTime,
            LatestPostId = items.Count == 0 ? 0 : items.Max(p => p.Id),
            Items = items
        };

        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);

        await using var stream = File.Create(_path);
        await JsonSerializer.SerializeAsync(stream, cache);
    }
}
