using ApollonVolados.Mobile.Models;
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
    
    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not NewsItemViewModel item)
            return;

        await Launcher.Default.OpenAsync(item.Link);

        ((CollectionView)sender).SelectedItem = null;
    }
}