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
        _vm.LoadCommand.Execute(null);
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
        if (e.CurrentSelection.FirstOrDefault() is not NewsItemViewModel item)
            return;

        await Launcher.Default.OpenAsync(item.Link);

        ((CollectionView)sender).SelectedItem = null;
    }
}