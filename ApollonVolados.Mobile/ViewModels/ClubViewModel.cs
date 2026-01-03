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
        => await Launcher.OpenAsync("tel:+306974902831");

    [RelayCommand]
    async Task Email()
        => await Launcher.OpenAsync("mailto:info@apollonvolados.gr");

    [RelayCommand]
    async Task Website()
        => await Launcher.OpenAsync("https://apollonvolados.gr");

    [RelayCommand]
    async Task Map()
        => await Launcher.OpenAsync(
            "https://maps.google.com/?q=Δημοτικό Γήπεδο Βωλάδος");
}