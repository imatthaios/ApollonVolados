using ApollonVolados.Mobile.Models;
using ApollonVolados.Mobile.Tests.Fakes;
using ApollonVolados.Mobile.ViewModels;

namespace ApollonVolados.Mobile.Tests.ViewModels;

public class NewsItemViewModelTests
{
    [Fact]
    public void Constructor_rejects_null()
    {
        Assert.Throws<ArgumentNullException>(() => new NewsItemViewModel(null!));
    }

    [Fact]
    public void Exposes_post_fields()
    {
        var post = TestPosts.Create(12, "Τίτλος &amp; υπότιτλος", "https://x/p/", "https://x/i.jpg", new DateTime(2025, 3, 5));

        var sut = new NewsItemViewModel(post);

        Assert.Equal(12, sut.Id);
        Assert.Equal("Τίτλος & υπότιτλος", sut.TitleText);
        Assert.Equal("https://x/p/", sut.Link);
        Assert.Equal("https://x/i.jpg", sut.ImageUrl);
        Assert.Equal(new DateTime(2025, 3, 5), sut.PublishedDate);
    }

    [Fact]
    public void Is_not_featured_by_default()
    {
        Assert.False(new NewsItemViewModel(TestPosts.Create(1)).IsFeatured);
    }

    [Fact]
    public void Link_is_empty_string_when_missing()
    {
        var sut = new NewsItemViewModel(new WpPost { Link = null });

        Assert.Equal(string.Empty, sut.Link);
    }

    [Fact]
    public void ImageUrl_falls_back_to_placeholder()
    {
        var sut = new NewsItemViewModel(TestPosts.Create(1, image: null));

        Assert.Equal(WpPost.PlaceholderImage, sut.ImageUrl);
    }

    [Theory]
    [InlineData(2025, 1, 9, "09 Ιαν 2025")]
    [InlineData(2025, 3, 5, "05 Μαρ 2025")]
    [InlineData(2024, 12, 31, "31 Δεκ 2024")]
    public void PublishedDateText_is_formatted_in_greek(int y, int m, int d, string expected)
    {
        var sut = new NewsItemViewModel(TestPosts.Create(1, date: new DateTime(y, m, d)));

        Assert.Equal(expected, sut.PublishedDateText);
    }

    [Fact]
    public void PublishedDateText_does_not_depend_on_current_culture()
    {
        var sut = new NewsItemViewModel(TestPosts.Create(1, date: new DateTime(2025, 3, 5)));
        var previous = System.Globalization.CultureInfo.CurrentCulture;
        try
        {
            System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Assert.Equal("05 Μαρ 2025", sut.PublishedDateText);
        }
        finally
        {
            System.Globalization.CultureInfo.CurrentCulture = previous;
        }
    }
}
