using System.Text.Json.Serialization;

namespace ApollonVolados.Mobile.Models;

public class Embedded
{
    [JsonPropertyName("wp:featuredmedia")]
    public List<FeaturedMedia> FeaturedMedia { get; set; }
}