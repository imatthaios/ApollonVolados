using System.Net.Http.Json;
using System.Text.Json;
using ApollonVolados.Mobile.Models;

namespace ApollonVolados.Mobile.Services;

/// <summary>Source of club news posts.</summary>
public interface IWordPressService
{
    /// <summary>
    /// Returns the latest posts. Serves a fresh cache when available, otherwise fetches
    /// from the website; if the network fails, falls back to an expired cache.
    /// </summary>
    /// <param name="forceRefresh">Skip the fresh-cache check and always try the network first.</param>
    /// <exception cref="HttpRequestException">The network failed and there is no cache at all.</exception>
    Task<List<WpPost>> GetLatestPostsAsync(bool forceRefresh = false, CancellationToken cancellationToken = default);
}

/// <summary>Reads posts from the club's WordPress REST API (<c>/wp-json/wp/v2/posts</c>).</summary>
public class WordPressService : IWordPressService
{
    /// <summary>Posts endpoint; <c>_embed</c> includes the featured image in the response.</summary>
    public const string PostsUrl = "https://apollonvolados.gr/wp-json/wp/v2/posts?_embed";

    private readonly HttpClient _httpClient;
    private readonly NewsCacheService _cache;

    public WordPressService(HttpClient httpClient, NewsCacheService cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<List<WpPost>> GetLatestPostsAsync(
        bool forceRefresh = false,
        CancellationToken cancellationToken = default)
    {
        if (!forceRefresh)
        {
            var cached = await _cache.GetCachedAsync();
            if (cached != null)
                return cached;
        }

        List<WpPost> posts;
        try
        {
            posts = await _httpClient.GetFromJsonAsync<List<WpPost>>(PostsUrl, cancellationToken) ?? [];
        }
        catch (Exception ex) when (IsNetworkFailure(ex, cancellationToken))
        {
            var stale = await _cache.GetCachedAsync(allowExpired: true);
            if (stale != null)
                return stale;

            throw;
        }

        try
        {
            await _cache.SaveAsync(posts);
        }
        catch (IOException)
        {
            // Failing to write the cache is not a reason to hide freshly downloaded news.
        }

        return posts;
    }

    // TaskCanceledException without a cancelled token means an HttpClient timeout.
    private static bool IsNetworkFailure(Exception ex, CancellationToken token) =>
        ex is HttpRequestException or JsonException
        || (ex is TaskCanceledException && !token.IsCancellationRequested);
}
