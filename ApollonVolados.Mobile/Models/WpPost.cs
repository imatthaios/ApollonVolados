using System.Net;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ApollonVolados.Mobile.Models;

/// <summary>
/// A post from the WordPress REST API (<c>/wp-json/wp/v2/posts?_embed</c>).
/// Only the fields the app uses are mapped.
/// </summary>
public partial class WpPost
{
    /// <summary>Bundled image shown when a post has no featured image.</summary>
    public const string PlaceholderImage = "crest.png";

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public RenderedText? Title { get; set; }

    [JsonPropertyName("excerpt")]
    public RenderedText? Excerpt { get; set; }

    [JsonPropertyName("link")]
    public string? Link { get; set; }

    [JsonPropertyName("_embedded")]
    public Embedded? Embedded { get; set; }

    /// <summary>Publish date as reported by WordPress (site-local time, no offset).</summary>
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    /// <summary>Title as plain text: HTML tags stripped and entities (e.g. <c>&amp;#8211;</c>) decoded.</summary>
    [JsonIgnore]
    public string TitleText => ToPlainText(Title?.Rendered);

    /// <summary>Excerpt as plain text: HTML tags stripped and entities decoded.</summary>
    [JsonIgnore]
    public string ExcerptText => ToPlainText(Excerpt?.Rendered);

    /// <summary>URL of the featured image, or <see cref="PlaceholderImage"/> if there is none.</summary>
    [JsonIgnore]
    public string ImageUrl
    {
        get
        {
            var url = Embedded?.FeaturedMedia?.FirstOrDefault()?.SourceUrl;
            return string.IsNullOrWhiteSpace(url) ? PlaceholderImage : url;
        }
    }

    /// <summary>Strips HTML tags and decodes HTML entities. Returns an empty string for null/blank input.</summary>
    internal static string ToPlainText(string? html)
    {
        if (string.IsNullOrWhiteSpace(html))
            return string.Empty;

        return WebUtility.HtmlDecode(HtmlTag().Replace(html, string.Empty)).Trim();
    }

    [GeneratedRegex("<.*?>", RegexOptions.Singleline)]
    private static partial Regex HtmlTag();
}
