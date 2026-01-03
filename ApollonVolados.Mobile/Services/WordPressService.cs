using System.Net.Http.Json;
using ApollonVolados.Mobile.Models;

namespace ApollonVolados.Mobile.Services;

public class WordPressService
{
    private const string BaseUrl =
        "https://apollonvolados.gr/wp-json/wp/v2/posts?_embed";

    private readonly HttpClient _httpClient;
    private readonly NewsCacheService _cache;

    public WordPressService(
        HttpClient httpClient,
        NewsCacheService cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<List<WpPost>> GetLatestPostsAsync()
    {
        var cached = await _cache.GetCachedAsync();
        if (cached != null) return cached;
        
        var posts = await _httpClient.GetFromJsonAsync<List<WpPost>>(BaseUrl);
        if (posts != null)
        {
            await _cache.SaveAsync(posts);
        }
        
        return posts ?? [];
    }
}