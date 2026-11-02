using ApollonVolados.Mobile.Models;
using ApollonVolados.Mobile.ViewModels;

namespace ApollonVolados.Mobile.Views;

public partial class TeamsPage : ContentPage
{
    private readonly TeamsViewModel _vm;

    public TeamsPage(TeamsViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    private async void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not TeamCategory item)
            return;

        if (sender is CollectionView list)
            list.SelectedItem = null;

        UiFeedback.Selection();
        await _vm.OpenTeamAsync(item);
    }
}
