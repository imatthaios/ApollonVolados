using System.Text.Json.Serialization;

namespace ApollonVolados.Mobile.Models;

/// <summary>The <c>_embedded</c> block WordPress adds when a request uses <c>?_embed</c>.</summary>
public class Embedded
{
    [JsonPropertyName("wp:featuredmedia")]
    public List<FeaturedMedia>? FeaturedMedia { get; set; }
}
