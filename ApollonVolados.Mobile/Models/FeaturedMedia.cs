using System.Text.Json.Serialization;

namespace ApollonVolados.Mobile.Models;

/// <summary>A post's featured image.</summary>
public class FeaturedMedia
{
    [JsonPropertyName("source_url")]
    public string? SourceUrl { get; set; }
}
