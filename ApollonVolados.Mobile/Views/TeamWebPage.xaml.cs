namespace ApollonVolados.Mobile.Views;

[QueryProperty(nameof(Url), "url")]
[QueryProperty(nameof(PageTitle), "title")]
public partial class TeamWebPage : ContentPage
{
    public string Url
    {
        set
        {
            field = Uri.UnescapeDataString(value);
            Browser.Source = field;
        }
    }

    public string PageTitle
    {
        set => Title = Uri.UnescapeDataString(value);
    }

    public TeamWebPage()
    {
        InitializeComponent();
    }
}