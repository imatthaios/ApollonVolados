using ApollonVolados.Mobile.ViewModels;

namespace ApollonVolados.Mobile.Views;

public partial class ClubPage : ContentPage
{
    public ClubPage(ClubViewModel vm, AppearanceViewModel appearance)
    {
        InitializeComponent();
        BindingContext = vm;
        AppearanceSection.BindingContext = appearance;
    }
}
