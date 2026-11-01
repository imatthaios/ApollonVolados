using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ApollonVolados.Mobile.Services;

namespace ApollonVolados.Mobile.ViewModels;

/// <summary>Backs the Club tab's contact buttons (phone, email, website, map).</summary>
public partial class ClubViewModel : ObservableObject
{
    public const string PhoneNumber = "+306974902831";
    public const string EmailAddress = "info@apollonvolados.gr";
    public const string WebsiteUrl = "https://apollonvolados.gr";
    public const string StadiumQuery = "Γήπεδο Βωλάδος";

    /// <summary>Google Maps search URL for the stadium; opens the Maps app on Android and Maps/Safari on iOS.</summary>
    public static readonly string MapUrl =
        "https://www.google.com/maps/search/?api=1&query=" + Uri.EscapeDataString(StadiumQuery);

    private readonly ILauncher _launcher;
    private readonly IPhoneDialer _phoneDialer;
    private readonly IDialogService _dialogs;

    public ClubViewModel(ILauncher launcher, IPhoneDialer phoneDialer, IDialogService dialogs)
    {
        _launcher = launcher;
        _phoneDialer = phoneDialer;
        _dialogs = dialogs;
    }

    public string HistoryText =>
        "Ο Αθλητικός Σύλλογος Απόλλων Βωλάδος ιδρύθηκε το 1924 και αποτελεί " +
        "ένα από τα ιστορικότερα σωματεία της περιοχής. Με διαχρονική παρουσία " +
        "στα αθλητικά δρώμενα και έντονη κοινωνική δράση, συνεχίζει να προάγει " +
        "τις αξίες του αθλητισμού και της ομαδικότητας.";

    /// <summary>Dials the club. Falls back to a <c>tel:</c> link on devices without a dialer (iPad, simulator).</summary>
    [RelayCommand]
    public async Task CallAsync()
    {
        try
        {
            if (_phoneDialer.IsSupported)
            {
                _phoneDialer.Open(PhoneNumber);
                return;
            }

            if (!await _launcher.OpenAsync(new Uri("tel:" + PhoneNumber)))
                await _dialogs.ShowAlertAsync("Info", "Cannot make calls on this device.");
        }
        catch (Exception)
        {
            await _dialogs.ShowAlertAsync("Info", "Cannot make calls on this device.");
        }
    }

    /// <summary>Opens the mail app addressed to the club.</summary>
    [RelayCommand]
    public Task EmailAsync() =>
        OpenOrAlertAsync("mailto:" + EmailAddress, "Info", "No mail app is available on this device.");

    /// <summary>Opens the club website in the browser.</summary>
    [RelayCommand]
    public Task WebsiteAsync() =>
        OpenOrAlertAsync(WebsiteUrl, "Error", "Could not open website");

    /// <summary>Opens the stadium location in maps.</summary>
    [RelayCommand]
    public Task MapAsync() =>
        OpenOrAlertAsync(MapUrl, "Error", "Could not open maps");

    // Launcher throws on the simulator (e.g. error -10814 when no mail app) and returns false
    // when nothing can handle the URI; both cases end in the same friendly alert.
    private async Task OpenOrAlertAsync(string uri, string title, string message)
    {
        bool opened;
        try
        {
            opened = await _launcher.OpenAsync(new Uri(uri));
        }
        catch (Exception)
        {
            opened = false;
        }

        if (!opened)
            await _dialogs.ShowAlertAsync(title, message);
    }
}
