using System.Collections.ObjectModel;
using ApollonVolados.Mobile.Models;

namespace ApollonVolados.Mobile.ViewModels;

public class MilestonesViewModel
{
    public ObservableCollection<Milestone> Items { get; } = new()
    {
        new Milestone
        {
            Year = 1924,
            Title = "Ίδρυση Συλλόγου",
            Description = "Ίδρυση του Αθλητικού Συλλόγου Απόλλων Βωλάδος",
            Icon = "🟢",
            IsMajor = true
        },
        new Milestone
        {
            Year = 1985,
            Title = "Άνοδος Κατηγορίας",
            Description = "Ιστορική άνοδος σε ανώτερη κατηγορία",
            Icon = "⬆️"
        },
        new Milestone
        {
            Year = 2004,
            Title = "Πρωτάθλημα",
            Description = "Κατάκτηση τοπικού πρωταθλήματος",
            Icon = "🏆",
            IsMajor = true
        },
        new Milestone
        {
            Year = 2024,
            Title = "100 Χρόνια Απόλλων",
            Description = "Εκατό χρόνια ιστορίας",
            Icon = "🎉",
            IsMajor = true
        }
    };
}