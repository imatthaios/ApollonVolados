namespace ApollonVolados.Mobile;

public partial class AppShell : Shell
{
    string? _lastTabId;

    public AppShell()
    {
        InitializeComponent();
        Navigated += OnShellNavigated;
    }

    private async void OnShellNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        var currentTabId = CurrentItem?.Route ?? CurrentItem?.Title ?? "tab";

        if (_lastTabId == currentTabId)
            return;

        _lastTabId = currentTabId;

        // 👇 ΠΡΟΣΕΧΕ: cast σε ContentPage
        if (CurrentPage is ContentPage contentPage &&
            contentPage.Content is VisualElement root)
        {
            root.Opacity = 0;
            root.TranslationX = 10;

            await Task.WhenAll(
                root.FadeTo(1, 160, Easing.CubicOut),
                root.TranslateTo(0, 0, 220, Easing.CubicOut)
            );
        }
    }
}