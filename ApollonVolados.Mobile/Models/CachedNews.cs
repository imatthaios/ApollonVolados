namespace ApollonVolados.Mobile.Models;

/// <summary>On-disk shape of the news cache written by <c>NewsCacheService</c>.</summary>
public class CachedNews
{
    /// <summary>UTC time the cache was written.</summary>
    public DateTime CachedAt { get; set; }

    /// <summary>Highest post id in <see cref="Items"/> (0 when empty).</summary>
    public int LatestPostId { get; set; }

    public List<WpPost> Items { get; set; } = [];
}
