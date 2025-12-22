using ApollonVolados.Mobile.ViewModels;

namespace ApollonVolados.Mobile.Views.Controls;

public partial class TimelineSection : ContentView
{
    public TimelineSection()
    {
        InitializeComponent();
        BindingContext = new TimelineViewModel();
    }
}