using System.Text.Json.Serialization;
using System.Net;
using System.Text.RegularExpressions;

namespace ApollonVolados.Mobile.Models;

public class WpPost
{
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public RenderedText Title { get; set; }

    [JsonPropertyName("excerpt")]
    public RenderedText Excerpt { get; set; }

    [JsonPropertyName("link")]
    public string Link { get; set; }

    [JsonPropertyName("_embedded")]
    public Embedded Embedded { get; set; }
    
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    public string TitleText =>
        WebUtility.HtmlDecode(StripHtml(Title?.Rendered));

    public string ExcerptText =>
        WebUtility.HtmlDecode(StripHtml(Excerpt?.Rendered));

    public string ImageUrl
    {
        get
        {
            var url = Embedded?.FeaturedMedia?.FirstOrDefault()?.SourceUrl;

            System.Diagnostics.Debug.WriteLine(
                $"[IMAGE] Post {Id} -> {url}"
            );

            return string.IsNullOrWhiteSpace(url)
                ? "https://via.placeholder.com/150x150.png?text=Apollon"
                : url;
        }
    }

    private static string StripHtml(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
            return string.Empty;

        return Regex.Replace(html, "<.*?>", string.Empty).Trim();
    }
}
