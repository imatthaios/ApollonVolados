using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using ApollonVolados.Mobile.Models;
using ApollonVolados.Mobile.Views;

namespace ApollonVolados.Mobile.ViewModels;

public partial class TeamsViewModel
{
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

    [RelayCommand]
    public async Task OpenTeamAsync(TeamCategory team)
    {
        if (team == null) return;

        await Shell.Current.GoToAsync(
            nameof(TeamWebPage),
            new Dictionary<string, object>
            {
                ["Title"] = team.Title,
                ["Url"] = team.Url
            });
    }
}
