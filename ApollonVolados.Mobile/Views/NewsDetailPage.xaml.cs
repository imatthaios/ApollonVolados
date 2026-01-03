namespace ApollonVolados.Mobile.Views;

[QueryProperty(nameof(Url), "url")]
public partial class NewsDetailPage : ContentPage
{
    private string _url;

    public string Url
    {
        get => _url;
        set
        {
            _url = value;

            if (!string.IsNullOrWhiteSpace(_url))
                Browser.Source = _url;
        }
    }

    public NewsDetailPage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}