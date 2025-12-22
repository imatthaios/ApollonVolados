using ApollonVolados.Mobile.ViewModels;

namespace ApollonVolados.Mobile.Views.Controls;

public partial class MilestonesSection : ContentView
{
    public MilestonesSection()
    {
        InitializeComponent();
        BindingContext = new MilestonesViewModel();
    }
}