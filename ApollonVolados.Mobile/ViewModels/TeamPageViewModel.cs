using CommunityToolkit.Mvvm.ComponentModel;

namespace ApollonVolados.Mobile.ViewModels;

public partial class TeamViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isBusy;
}