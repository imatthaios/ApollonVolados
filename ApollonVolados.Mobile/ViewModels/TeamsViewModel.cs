using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using ApollonVolados.Mobile.Models;
using ApollonVolados.Mobile.Services;

namespace ApollonVolados.Mobile.ViewModels;

/// <summary>Backs the Teams tab: the list of club departments and navigation to each one's web page.</summary>
public partial class TeamsViewModel
{
    private readonly INavigationService _navigation;

    public TeamsViewModel(INavigationService navigation)
    {
        _navigation = navigation;
    }

    public ObservableCollection<TeamCategory> Categories { get; } =
        new()
        {
            new()
            {
                Title = "Ανδρική Ομάδα",
                Subtitle = "Η κύρια ποδοσφαιρική ομάδα",
                Icon = "⚽",
                Url = "https://apollonvolados.gr/antriki-omoada/"
            },
            new()
            {
                Title = "Γυναικεία Ομάδα",
                Subtitle = "Γυναικείο ποδόσφαιρο",
                Icon = "⚽♀️",
                Url = "https://apollonvolados.gr/gynaikeia-omada/"
            },
            new()
            {
                Title = "Παιδική Ομάδα",
                Subtitle = "Τμήματα υποδομής",
                Icon = "👦",
                Url = "https://apollonvolados.gr/akadimies/"
            },
            new()
            {
                Title = "Ομάδα Μπάσκετ",
                Subtitle = "Τμήμα καλαθοσφαίρισης",
                Icon = "🏀",
                Url = "https://apollonvolados.gr/mpasket/"
            },
            new()
            {
                Title = "Απόλλων Βολάδος Αττικής",
                Subtitle = "Παράρτημα Αττικής",
                Icon = "🏙",
                Url = "https://apollonvolados.gr/apollon-attikis/"
            },
            new()
            {
                Title = "Απόλλων Βωλάδος USA",
                Subtitle = "Ομογένεια ΗΠΑ",
                Icon = "🇺🇸",
                Url = "https://apollonvolados.gr/apollon-usa/"
            }
        };

    /// <summary>Opens the team's website page in <c>TeamWebPage</c>. Ignores null or link-less items.</summary>
    [RelayCommand]
    public async Task OpenTeamAsync(TeamCategory? team)
    {
        if (team is null || string.IsNullOrWhiteSpace(team.Url))
            return;

        await _navigation.GoToAsync(
            Routes.TeamWeb,
            new Dictionary<string, object> { [Routes.UrlParameter] = team.Url });
    }
}
