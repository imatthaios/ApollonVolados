using ApollonVolados.Mobile.Tests.Fakes;
using ApollonVolados.Mobile.ViewModels;
using ApollonVolados.Mobile.Views;
using Microsoft.Maui.Controls;

namespace ApollonVolados.Mobile.Tests.ViewModels;

public class NewsTemplateSelectorTests
{
    private readonly DataTemplate _lead = new(() => new Label());
    private readonly DataTemplate _item = new(() => new Label());

    private NewsTemplateSelector CreateSut() => new() { LeadTemplate = _lead, ItemTemplate = _item };

    [Fact]
    public void Featured_post_uses_lead_template()
    {
        var item = new NewsItemViewModel(TestPosts.Create(1)) { IsFeatured = true };

        Assert.Same(_lead, CreateSut().SelectTemplate(item, new CollectionView()));
    }

    [Fact]
    public void Other_posts_use_item_template()
    {
        var item = new NewsItemViewModel(TestPosts.Create(1));

        Assert.Same(_item, CreateSut().SelectTemplate(item, new CollectionView()));
    }

    [Fact]
    public void Unknown_items_use_item_template()
    {
        Assert.Same(_item, CreateSut().SelectTemplate("not a post", new CollectionView()));
    }

    [Fact]
    public void Missing_templates_fail_loudly()
    {
        var sut = new NewsTemplateSelector();

        Assert.Throws<InvalidOperationException>(() =>
            sut.SelectTemplate(new NewsItemViewModel(TestPosts.Create(1)), new CollectionView()));
    }
}
