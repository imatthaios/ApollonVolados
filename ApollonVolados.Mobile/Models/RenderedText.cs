using System.Text.Json.Serialization;

namespace ApollonVolados.Mobile.Models;

public class RenderedText
{
    [JsonPropertyName("rendered")]
    public string Rendered { get; set; }
}