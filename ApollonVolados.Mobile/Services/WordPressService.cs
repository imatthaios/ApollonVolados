using System.Net.Http.Json;
using ApollonVolados.Mobile.Models;

namespace ApollonVolados.Mobile.Services;

public class WordPressService
{
    private const string BaseUrl =
        "https://apollonvolados.gr/wp-json/wp/v2/posts";

    private readonly HttpClient _httpClient;

    public WordPressService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<WpPost>> GetLatestPostsAsync()
    {
        var posts = await _httpClient.GetFromJsonAsync<List<WpPost>>(BaseUrl);
        return posts ?? [];
    }
}