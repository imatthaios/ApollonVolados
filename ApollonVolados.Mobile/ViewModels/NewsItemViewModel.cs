using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using ApollonVolados.Mobile.Models;

namespace ApollonVolados.Mobile.ViewModels;

/// <summary>Display wrapper around a <see cref="WpPost"/> for one card in the news list.</summary>
public partial class NewsItemViewModel : ObservableObject
{
    private static readonly CultureInfo Greek = CultureInfo.GetCultureInfo("el-GR");

    private readonly WpPost _post;

    public NewsItemViewModel(WpPost post)
    {
        ArgumentNullException.ThrowIfNull(post);
        _post = post;
    }

    public int Id => _post.Id;

    public string TitleText => _post.TitleText;

    public string ImageUrl => _post.ImageUrl;

    /// <summary>Article URL on the website, or an empty string if WordPress omitted it.</summary>
    public string Link => _post.Link ?? string.Empty;

    public DateTime PublishedDate => _post.Date;

    /// <summary>The lead story, shown as the large card at the top of the list.</summary>
    public bool IsFeatured { get; init; }

    /// <summary>Publish date formatted in Greek, e.g. <c>05 Μαρ 2025</c>.</summary>
    public string PublishedDateText =>
        PublishedDate.ToString("dd MMM yyyy", Greek);
}
