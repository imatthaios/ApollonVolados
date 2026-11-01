namespace ApollonVolados.Mobile.Models;

/// <summary>A club department (men's, women's, academy, …) listed in the Teams tab.</summary>
public class TeamCategory
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public string Subtitle { get; set; } = "";

    /// <summary>Emoji shown in the round badge.</summary>
    public string Icon { get; set; } = "";

    /// <summary>Page on the club website opened in <c>TeamWebPage</c>.</summary>
    public string Url { get; set; } = "";
}
