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

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync(); // never throws; failures surface as ErrorMessage
    }

    private async void OnItemSelected(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not NewsItemViewModel post)
            return;

        if (sender is CollectionView list)
            list.SelectedItem = null;

        UiFeedback.Selection();
        await _vm.OpenPostAsync(post);
    }
}
