using ApollonVolados.Mobile.ViewModels;

namespace ApollonVolados.Mobile.Views;

public partial class ContactPage : ContentPage
{
    public ContactPage()
    {
        InitializeComponent();
        BindingContext = new ContactViewModel();
    }
}