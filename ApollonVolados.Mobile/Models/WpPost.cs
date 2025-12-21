using System.Text.Json.Serialization;

namespace ApollonVolados.Mobile.Models;

public class WpPost
{
    public int Id { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("title")]
    public RenderedText Title { get; set; }

    [JsonPropertyName("excerpt")]
    public RenderedText Excerpt { get; set; }
}