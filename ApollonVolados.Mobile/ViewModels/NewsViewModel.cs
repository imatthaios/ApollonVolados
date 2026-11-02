using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ApollonVolados.Mobile.Services;

namespace ApollonVolados.Mobile.ViewModels;

/// <summary>Backs the News tab: loads posts, exposes busy/error state and opens articles.</summary>
public partial class NewsViewModel : ObservableObject
{
    /// <summary>Message shown when posts cannot be loaded and nothing is cached.</summary>
    public const string LoadErrorMessage =
        "Δεν ήταν δυνατή η φόρτωση των νέων. Ελέγξτε τη σύνδεσή σας και δοκιμάστε ξανά.";

    private readonly IWordPressService _service;
    private readonly INavigationService _navigation;

    public ObservableCollection<NewsItemViewModel> Posts { get; } = [];

    [ObservableProperty]
    private bool isBusy;

    /// <summary>Bound two-way to the RefreshView; reset when any load finishes.</summary>
    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string? errorMessage;

    /// <summary>True when the last load failed; the page shows a retry message.</summary>
    public bool HasError => ErrorMessage != null;

    public NewsViewModel(IWordPressService service, INavigationService navigation)
    {
        _service = service;
        _navigation = navigation;
    }

    /// <summary>Loads posts (cache first). Ignored while a load is already running. Never throws.</summary>
    [RelayCommand]
    public Task LoadAsync() => LoadCoreAsync(forceRefresh: false);

    /// <summary>Reloads posts from the network, bypassing a fresh cache.</summary>
    [RelayCommand]
    public Task RefreshAsync() => LoadCoreAsync(forceRefresh: true);

    /// <summary>Opens the article in <c>NewsDetailPage</c>. Does nothing for items without a link.</summary>
    [RelayCommand]
    public async Task OpenPostAsync(NewsItemViewModel? item)
    {
        if (item is null || string.IsNullOrWhiteSpace(item.Link))
            return;

        await _navigation.GoToAsync(
            Routes.NewsDetail,
            new Dictionary<string, object> { [Routes.UrlParameter] = item.Link });
    }

    private async Task LoadCoreAsync(bool forceRefresh)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var posts = await _service.GetLatestPostsAsync(forceRefresh);

            // Replace the list only after a successful load so a failed refresh keeps what's on screen.
            Posts.Clear();
            for (var i = 0; i < posts.Count; i++)
                Posts.Add(new NewsItemViewModel(posts[i]) { IsFeatured = i == 0 });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[News] Load failed: {ex}");
            ErrorMessage = LoadErrorMessage;
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }
}
