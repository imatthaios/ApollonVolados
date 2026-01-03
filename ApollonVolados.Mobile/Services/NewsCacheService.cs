using System.Text.Json;
using ApollonVolados.Mobile.Models;

namespace ApollonVolados.Mobile.Services;

public class NewsCacheService
{
    private const string CacheFile = "news_cache.json";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(6);

    public async Task<List<WpPost>?> GetCachedAsync()
    {
        var path = Path.Combine(FileSystem.CacheDirectory, CacheFile);

        if (!File.Exists(path))
            return null;

        var json = await File.ReadAllTextAsync(path);
        var cache = JsonSerializer.Deserialize<CachedNews>(json);

        if (cache == null)
            return null;

        if (DateTime.UtcNow - cache.CachedAt > CacheDuration)
            return null;

        return cache.Items;
    }

    public async Task SaveAsync(List<WpPost> items)
    {
        var cache = new CachedNews
        {
            CachedAt = DateTime.UtcNow,
            Items = items
        };

        var json = JsonSerializer.Serialize(cache);
        var path = Path.Combine(FileSystem.CacheDirectory, CacheFile);

        await File.WriteAllTextAsync(path, json);
    }
}