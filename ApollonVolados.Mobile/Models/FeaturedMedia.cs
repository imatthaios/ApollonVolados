using System.Text.Json.Serialization;

namespace ApollonVolados.Mobile.Models;

public class FeaturedMedia
{
    [JsonPropertyName("source_url")]
    public string SourceUrl { get; set; }
}