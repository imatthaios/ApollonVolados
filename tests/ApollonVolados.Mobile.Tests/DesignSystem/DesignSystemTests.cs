using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ApollonVolados.Mobile.Tests.DesignSystem;

/// <summary>
/// Guards the rules in docs/DESIGN.md by reading the XAML sources:
/// every colour token is themed, and pages use styles instead of inline styling.
/// </summary>
public class DesignSystemTests
{
    private static readonly XNamespace X = "http://schemas.microsoft.com/winfx/2009/xaml";

    private static string AppDir([CallerFilePath] string here = "") =>
        Path.GetFullPath(Path.Combine(Path.GetDirectoryName(here)!, "..", "..", "..", "ApollonVolados.Mobile"));

    private static IEnumerable<string> Keys(string file) =>
        XDocument.Load(Path.Combine(AppDir(), file)).Descendants()
            .Select(e => (string?)e.Attribute(X + "Key"))
            .OfType<string>();

    public static TheoryData<string> ViewFiles()
    {
        var data = new TheoryData<string>();
        foreach (var file in Directory.EnumerateFiles(Path.Combine(AppDir(), "Views"), "*.xaml", SearchOption.AllDirectories))
            data.Add(Path.GetRelativePath(AppDir(), file));
        return data;
    }

    [Fact]
    public void Every_light_colour_has_a_dark_counterpart()
    {
        var keys = Keys("Resources/Styles/Colors.xaml").ToHashSet();
        var light = keys.Where(k => k.EndsWith("Light")).ToList();

        Assert.NotEmpty(light);
        Assert.All(light, k => Assert.Contains(k[..^"Light".Length] + "Dark", keys));
        Assert.All(keys.Where(k => k.EndsWith("Dark")), k => Assert.Contains(k[..^"Dark".Length] + "Light", keys));
    }

    [Theory]
    [InlineData("Text.Display")]
    [InlineData("Text.Title1")]
    [InlineData("Text.Title2")]
    [InlineData("Text.Title3")]
    [InlineData("Text.Headline")]
    [InlineData("Text.Body")]
    [InlineData("Text.Callout")]
    [InlineData("Text.Subhead")]
    [InlineData("Text.Footnote")]
    [InlineData("Text.Caption")]
    [InlineData("Text.Overline")]
    [InlineData("Text.Year")]
    [InlineData("Button.Primary")]
    [InlineData("Button.Secondary")]
    [InlineData("Button.Tertiary")]
    [InlineData("Card")]
    [InlineData("Card.Lead")]
    [InlineData("CrestRule.Bar")]
    [InlineData("CrestRule.Line")]
    [InlineData("CrestDisc")]
    [InlineData("CrestFrame")]
    public void Documented_style_exists(string key)
    {
        Assert.Contains(key, Keys("Resources/Styles/Styles.xaml"));
    }

    [Theory]
    [MemberData(nameof(ViewFiles))]
    public void Views_do_not_use_inline_colours_fonts_or_sizes(string file)
    {
        var xaml = File.ReadAllText(Path.Combine(AppDir(), file));

        // Hex colours, explicit font settings and corner radii belong in Styles.xaml.
        var offenders = Regex.Matches(
                xaml,
                @"\b(TextColor|BackgroundColor|Color|Stroke|BorderColor)=""#|\b(FontSize|FontFamily|FontAttributes|CornerRadius|StrokeShape)=""")
            .Select(m => m.Value)
            .ToList();

        Assert.True(offenders.Count == 0, $"{file} has inline styling: {string.Join(", ", offenders)}");
    }

    [Theory]
    [MemberData(nameof(ViewFiles))]
    public void Views_only_reference_resources_that_exist(string file)
    {
        var available = Keys("Resources/Styles/Styles.xaml")
            .Concat(Keys("Resources/Styles/Colors.xaml"))
            .Concat(Keys("App.xaml"))
            .Concat(Keys(file))
            .ToHashSet();

        var used = Regex.Matches(File.ReadAllText(Path.Combine(AppDir(), file)), @"StaticResource ([\w.]+)")
            .Select(m => m.Groups[1].Value)
            .Distinct();

        Assert.All(used, key => Assert.Contains(key, available));
    }
}
