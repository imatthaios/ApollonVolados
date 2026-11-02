using ApollonVolados.Mobile.ViewModels;

namespace ApollonVolados.Mobile.Views;

/// <summary>Uses the large lead-story card for the featured post and the compact row for the rest.</summary>
public class NewsTemplateSelector : DataTemplateSelector
{
    public DataTemplate? LeadTemplate { get; set; }
    public DataTemplate? ItemTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container) =>
        (item is NewsItemViewModel { IsFeatured: true } ? LeadTemplate : ItemTemplate)
        ?? throw new InvalidOperationException("NewsTemplateSelector needs both LeadTemplate and ItemTemplate.");
}
