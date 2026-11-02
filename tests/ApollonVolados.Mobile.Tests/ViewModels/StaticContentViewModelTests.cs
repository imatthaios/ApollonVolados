using ApollonVolados.Mobile.ViewModels;

namespace ApollonVolados.Mobile.Tests.ViewModels;

public class StaticContentViewModelTests
{
    [Fact]
    public void Timeline_starts_with_founding_in_1924()
    {
        var first = new TimelineViewModel().Events.First();

        Assert.Equal("1924", first.Year);
    }

    [Fact]
    public void Timeline_is_in_chronological_order()
    {
        var years = new TimelineViewModel().Events.Select(e => int.Parse(e.Year)).ToList();

        Assert.Equal(years.Order(), years);
    }

    [Fact]
    public void Timeline_entries_are_complete()
    {
        Assert.All(new TimelineViewModel().Events, e =>
        {
            Assert.Matches(@"^\d{4}$", e.Year);
            Assert.False(string.IsNullOrWhiteSpace(e.Title));
            Assert.False(string.IsNullOrWhiteSpace(e.Description));
        });
    }

    [Fact]
    public void Milestones_start_with_founding_in_1924()
    {
        var first = new MilestonesViewModel().Items.First();

        Assert.Equal(1924, first.Year);
        Assert.True(first.IsMajor);
    }

    [Fact]
    public void Milestones_are_in_chronological_order()
    {
        var years = new MilestonesViewModel().Items.Select(m => m.Year).ToList();

        Assert.Equal(years.Order(), years);
    }

    [Fact]
    public void Milestones_entries_are_complete()
    {
        Assert.All(new MilestonesViewModel().Items, m =>
        {
            Assert.InRange(m.Year, 1924, DateTime.Now.Year);
            Assert.False(string.IsNullOrWhiteSpace(m.Title));
            Assert.False(string.IsNullOrWhiteSpace(m.Description));
            Assert.False(string.IsNullOrWhiteSpace(m.Icon));
        });
    }

    [Fact]
    public void Centenary_is_a_major_milestone()
    {
        var centenary = new MilestonesViewModel().Items.Single(m => m.Year == 2024);

        Assert.True(centenary.IsMajor);
    }
}
