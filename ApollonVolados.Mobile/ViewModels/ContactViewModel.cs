using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ApollonVolados.Mobile.ViewModels;

public partial class ContactViewModel : ObservableObject
{
    private const string PhoneNumber = "+3069XXXXXXXX";
    private const string EmailAddress = "info@apollonvolados.gr";
    private const string WebsiteUrl = "https://apollonvolados.gr";
    private const string MapsQuery = "Βωλάδα, Κάρπαθος, Ελλάδα";

    [RelayCommand]
    async Task Call()
    {
        await Launcher.Default.OpenAsync($"tel:{PhoneNumber}");
    }

    [RelayCommand]
    async Task SendEmail()
    {
        await Launcher.Default.OpenAsync($"mailto:{EmailAddress}");
    }

    [RelayCommand]
    async Task OpenWebsite()
    {
        await Launcher.Default.OpenAsync(WebsiteUrl);
    }

    [RelayCommand]
    async Task OpenMap()
    {
        await Launcher.Default.OpenAsync(
            $"https://www.google.com/maps/search/?api=1&query={Uri.EscapeDataString(MapsQuery)}");
    }
}