using ApollonVolados.Mobile.Models;
using ApollonVolados.Mobile.ViewModels;

namespace ApollonVolados.Mobile.Views;

public partial class TeamsPage : ContentPage
{
    public TeamsPage(TeamsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var item = e.CurrentSelection.FirstOrDefault() as TeamCategory;
        if (item == null)
            return;

        ((CollectionView)sender).SelectedItem = null;

        await Shell.Current.GoToAsync(
            nameof(TeamWebPage),
            new Dictionary<string, object>
            {
                { "url", item.Url },
                { "title", item.Title }
            });
    }
}