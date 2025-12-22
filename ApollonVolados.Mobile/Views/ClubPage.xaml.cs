using ApollonVolados.Mobile.ViewModels;

namespace ApollonVolados.Mobile.Views;

public partial class ClubPage : ContentPage
{
    public ClubPage(ClubViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}