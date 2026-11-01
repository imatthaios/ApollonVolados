using System.Text.Json.Serialization;

namespace ApollonVolados.Mobile.Models;

/// <summary>WordPress wraps HTML fields (title, excerpt, content) as <c>{ "rendered": "..." }</c>.</summary>
public class RenderedText
{
    [JsonPropertyName("rendered")]
    public string? Rendered { get; set; }
}
