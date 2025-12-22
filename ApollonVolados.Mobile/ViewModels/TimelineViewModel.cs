using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ApollonVolados.Mobile.Models;

namespace ApollonVolados.Mobile.ViewModels;

public partial class TimelineViewModel : ObservableObject
{
    public ObservableCollection<TimelineEvent> Events { get; } = new()
    {
        new TimelineEvent
        {
            Year = "1924",
            Title = "Ίδρυση Συλλόγου",
            Description = "Ιδρύεται ο Αθλητικός Σύλλογος Απόλλων Βωλάδος."
        },
        new TimelineEvent
        {
            Year = "1950",
            Title = "Πρώτες Αγωνιστικές Συμμετοχές",
            Description = "Ο σύλλογος αποκτά σταθερή παρουσία στα τοπικά πρωταθλήματα."
        },
        new TimelineEvent
        {
            Year = "1985",
            Title = "Ανάπτυξη Υποδομών",
            Description = "Δημιουργούνται ακαδημίες και νέες αθλητικές εγκαταστάσεις."
        },
        new TimelineEvent
        {
            Year = "2024",
            Title = "100 Χρόνια Απόλλων Βωλάδος",
            Description = "Εορτασμός ενός αιώνα ιστορίας, αθλητισμού και προσφοράς."
        }
    };
}