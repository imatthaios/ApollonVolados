using ApollonVolados.Mobile.ViewModels;

namespace ApollonVolados.Mobile.Views;

public partial class NewsPage : ContentPage
{
    private readonly NewsViewModel _vm;

    public NewsPage(NewsViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as NewsViewModel)?.LoadAsync();
    }
    
    private async void OnArticleTapped(object sender, EventArgs e)
    {
        if (sender is Border card)
        {
            await card.ScaleTo(0.97, 80, Easing.CubicOut);
            await card.ScaleTo(1, 120, Easing.CubicOut);
        }
    }
    
    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not NewsItemViewModel post)
            return;

        ((CollectionView)sender).SelectedItem = null;

        await Shell.Current.GoToAsync(
            nameof(NewsDetailPage),
            new Dictionary<string, object>
            {
                { "url", post.Link }
            });
    }

    
    // private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    // {
    //     if (e.CurrentSelection.FirstOrDefault() is not NewsItemViewModel item)
    //         return;
    //
    //     if (sender is CollectionView cv &&
    //         cv.SelectedItem is NewsItemViewModel)
    //     {
    //         var container = cv.Handler?.PlatformView;
    //     }
    //
    //     await Launcher.Default.OpenAsync(item.Link);
    //     ((CollectionView)sender).SelectedItem = null;
    // }
}