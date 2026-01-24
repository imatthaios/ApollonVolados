namespace ApollonVolados.Mobile.Views;

public partial class SplashPage : ContentPage
{
    public SplashPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Περίμενε λίγο για να φορτώσει το UI
        await Task.Delay(200);

        // --- ΕΦΕ ΠΕΡΙΣΤΡΟΦΗΣ (SPINNER) ---

        // Κάνε μια πλήρη περιστροφή 360 μοιρών σε 800ms
        await SplashLogo.RotateTo(360, 800, Easing.CubicInOut);

        // (Προαιρετικά) Κάνε και δεύτερη περιστροφή για πιο ωραίο εφέ
        SplashLogo.Rotation = 0; // Μηδενισμός
        await SplashLogo.RotateTo(360, 800, Easing.CubicInOut);

        // Μικρή παύση στο τέλος
        await Task.Delay(300);

        // --- ΑΛΛΑΓΗ ΣΕΛΙΔΑΣ ---
        // Αντικαθιστούμε την τρέχουσα σελίδα (Splash) με το πραγματικό AppShell
        Application.Current.MainPage = new AppShell();
    }
}