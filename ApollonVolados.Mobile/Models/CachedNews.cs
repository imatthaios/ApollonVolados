namespace ApollonVolados.Mobile.Models;

public class CachedNews
{
    public DateTime CachedAt { get; set; }
    public int LatestPostId { get; set; }
    public List<WpPost> Items { get; set; } = [];
}