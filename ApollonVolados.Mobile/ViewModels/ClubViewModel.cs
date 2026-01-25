using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ApollonVolados.Mobile.ViewModels;

public partial class ClubViewModel : ObservableObject
{
    public string HistoryText =>
        "Ο Αθλητικός Σύλλογος Απόλλων Βωλάδος ιδρύθηκε το 1924 και αποτελεί " +
        "ένα από τα ιστορικότερα σωματεία της περιοχής. Με διαχρονική παρουσία " +
        "στα αθλητικά δρώμενα και έντονη κοινωνική δράση, συνεχίζει να προάγει " +
        "τις αξίες του αθλητισμού και της ομαδικότητας.";

    [RelayCommand]
    async Task Call()
    {
        try
        {
            // Checks if the device can actually make calls (prevents crash on iPad/Simulator)
            if (PhoneDialer.Default.IsSupported)
                PhoneDialer.Default.Open("+306974902831");
            else
                await Launcher.OpenAsync("tel:+306974902831");
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("Info", "Cannot make calls on this device (Simulator).", "OK");
        }
    }

    [RelayCommand]
    async Task Email()
    {
        try
        {
            // Tries to open the native mail app
            await Launcher.OpenAsync("mailto:info@apollonvolados.gr");
        }
        catch (Exception)
        {
            // Catches the crash on the Simulator (Error -10814)
            await Shell.Current.DisplayAlert("Info", "Email tapped! (No Mail app on Simulator)", "OK");
        }
    }

    [RelayCommand]
    async Task Website()
    {
        try
        {
            await Launcher.OpenAsync("https://apollonvolados.gr");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "Could not open website", "OK");
        }
    }

    [RelayCommand]
    async Task Map()
    {
        try
        {
            // Fixed URL: Uses the standard Google Maps query format
            // This works on both Android (opens Maps app) and iOS (opens Browser/Maps)
            string mapUrl = "https://www.google.com/maps/search/?api=1&query=Γήπεδο+Βωλάδος";
            await Launcher.OpenAsync(mapUrl);
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("Error", "Could not open maps", "OK");
        }
    }
}