using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using ApollonVolados.Mobile.Models;

namespace ApollonVolados.Mobile.ViewModels;

public partial class NewsItemViewModel : ObservableObject
{
    private readonly WpPost _post;

    public NewsItemViewModel(WpPost post)
    {
        _post = post;
    }

    public int Id => _post.Id;

    public string TitleText => _post.TitleText;

    public string ImageUrl => _post.ImageUrl;

    public string Link => _post.Link;

    public DateTime PublishedDate => _post.Date;

    public string PublishedDateText =>
        PublishedDate.ToString("dd MMM yyyy", new CultureInfo("el-GR"));
}