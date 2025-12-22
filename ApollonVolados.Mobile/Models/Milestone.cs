namespace ApollonVolados.Mobile.Models;

public class Milestone
{
    public int Year { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Icon { get; set; } = "🏆"; // emoji for now
    public bool IsMajor { get; set; }
}