using System.Text.Json;
using ApollonVolados.Mobile.Models;
using ApollonVolados.Mobile.Tests.Fakes;

namespace ApollonVolados.Mobile.Tests.Models;

public class WpPostTests
{
    // Trimmed real-world shape of /wp-json/wp/v2/posts?_embed
    private const string SampleJson = """
        [
          {
            "id": 1234,
            "date": "2025-03-05T18:30:00",
            "link": "https://apollonvolados.gr/nikh-sto-ntermpi/",
            "title": { "rendered": "Νίκη στο ντέρμπι &#8211; 2&#8211;1" },
            "excerpt": { "rendered": "<p>Μεγάλη νίκη &amp; τρεις βαθμοί [&hellip;]</p>\n" },
            "_embedded": {
              "wp:featuredmedia": [ { "source_url": "https://apollonvolados.gr/img.jpg" } ]
            }
          }
        ]
        """;

    [Fact]
    public void Deserializes_WordPress_response_with_web_defaults()
    {
        var posts = JsonSerializer.Deserialize<List<WpPost>>(SampleJson, JsonSerializerOptions.Web)!;

        var post = Assert.Single(posts);
        Assert.Equal(1234, post.Id);
        Assert.Equal(new DateTime(2025, 3, 5, 18, 30, 0), post.Date);
        Assert.Equal("https://apollonvolados.gr/nikh-sto-ntermpi/", post.Link);
        Assert.Equal("https://apollonvolados.gr/img.jpg", post.ImageUrl);
    }

    [Fact]
    public void Deserializes_id_with_default_case_sensitive_options()
    {
        // The cache uses default (case-sensitive) options, so "id" must be mapped explicitly.
        var posts = JsonSerializer.Deserialize<List<WpPost>>(SampleJson)!;

        Assert.Equal(1234, posts[0].Id);
    }

    [Fact]
    public void TitleText_decodes_entities()
    {
        var post = JsonSerializer.Deserialize<List<WpPost>>(SampleJson)![0];

        Assert.Equal("Νίκη στο ντέρμπι – 2–1", post.TitleText);
    }

    [Fact]
    public void ExcerptText_strips_tags_decodes_entities_and_trims()
    {
        var post = JsonSerializer.Deserialize<List<WpPost>>(SampleJson)![0];

        Assert.Equal("Μεγάλη νίκη & τρεις βαθμοί […]", post.ExcerptText);
    }

    [Fact]
    public void TitleText_is_empty_when_title_missing()
    {
        Assert.Equal(string.Empty, new WpPost().TitleText);
        Assert.Equal(string.Empty, new WpPost { Title = new RenderedText() }.TitleText);
    }

    [Fact]
    public void ExcerptText_is_empty_when_excerpt_missing()
    {
        Assert.Equal(string.Empty, new WpPost().ExcerptText);
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("   ", "")]
    [InlineData("plain", "plain")]
    [InlineData("<b>bold</b> text", "bold text")]
    [InlineData("<a href=\"x\">link</a>", "link")]
    [InlineData("<p>multi\nline</p>", "multi\nline")]
    [InlineData("<span\nclass=\"x\">split tag</span>", "split tag")]
    [InlineData("&lt;not a tag&gt;", "<not a tag>")]
    [InlineData("  <p> padded </p>  ", "padded")]
    public void ToPlainText_handles_html(string? input, string expected)
    {
        Assert.Equal(expected, WpPost.ToPlainText(input));
    }

    [Fact]
    public void ImageUrl_uses_first_featured_media()
    {
        var post = new WpPost
        {
            Embedded = new Embedded
            {
                FeaturedMedia =
                [
                    new FeaturedMedia { SourceUrl = "https://a/1.jpg" },
                    new FeaturedMedia { SourceUrl = "https://a/2.jpg" }
                ]
            }
        };

        Assert.Equal("https://a/1.jpg", post.ImageUrl);
    }

    public static TheoryData<Embedded?> MissingImages => new()
    {
        null,
        new Embedded(),
        new Embedded { FeaturedMedia = [] },
        new Embedded { FeaturedMedia = [new FeaturedMedia()] },
        new Embedded { FeaturedMedia = [new FeaturedMedia { SourceUrl = " " }] },
    };

    [Theory]
    [MemberData(nameof(MissingImages))]
    public void ImageUrl_falls_back_to_bundled_placeholder(Embedded? embedded)
    {
        var post = new WpPost { Embedded = embedded };

        Assert.Equal(WpPost.PlaceholderImage, post.ImageUrl);
    }

    [Fact]
    public void Serialization_skips_computed_properties_and_round_trips()
    {
        var original = TestPosts.Create(7, "Τίτλος &amp; κάτι", image: "https://a/x.jpg");

        var json = JsonSerializer.Serialize(original);
        var copy = JsonSerializer.Deserialize<WpPost>(json)!;

        Assert.DoesNotContain("TitleText", json);
        Assert.DoesNotContain("ExcerptText", json);
        Assert.DoesNotContain("ImageUrl", json);
        Assert.Equal(original.Id, copy.Id);
        Assert.Equal(original.TitleText, copy.TitleText);
        Assert.Equal(original.Link, copy.Link);
        Assert.Equal(original.Date, copy.Date);
        Assert.Equal(original.ImageUrl, copy.ImageUrl);
    }
}
