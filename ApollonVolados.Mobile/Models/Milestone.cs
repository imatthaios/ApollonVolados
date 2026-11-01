namespace ApollonVolados.Mobile.Models;

/// <summary>A highlight in the club's history, shown in the Club tab's milestones card.</summary>
public class Milestone
{
    public int Year { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";

    /// <summary>Emoji shown next to the milestone.</summary>
    public string Icon { get; set; } = "🏆";

    /// <summary>Major milestones are emphasised in the UI.</summary>
    public bool IsMajor { get; set; }
}
