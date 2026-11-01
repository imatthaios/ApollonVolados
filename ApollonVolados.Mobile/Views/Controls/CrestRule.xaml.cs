namespace ApollonVolados.Mobile.Views.Controls;

/// <summary>
/// Signature divider under page and section titles. Left-aligned by default
/// (bar, then a hairline to the edge); set <see cref="IsCentered"/> for centred
/// layouts (hairline, bar, hairline).
/// </summary>
public partial class CrestRule : ContentView
{
    public static readonly BindableProperty IsCenteredProperty = BindableProperty.Create(
        nameof(IsCentered), typeof(bool), typeof(CrestRule), false,
        propertyChanged: (b, _, _) => ((CrestRule)b).UpdateLayout());

    public CrestRule()
    {
        InitializeComponent();
    }

    public bool IsCentered
    {
        get => (bool)GetValue(IsCenteredProperty);
        set => SetValue(IsCenteredProperty, value);
    }

    private void UpdateLayout()
    {
        Root.ColumnDefinitions = IsCentered
            ? new ColumnDefinitionCollection(new(GridLength.Star), new(GridLength.Auto), new(GridLength.Star))
            : new ColumnDefinitionCollection(new(GridLength.Auto), new(GridLength.Star));

        LeadingLine.IsVisible = IsCentered;
        Grid.SetColumn(LeadingLine, 0);
        Grid.SetColumn(Bar, IsCentered ? 1 : 0);
        Grid.SetColumn(TrailingLine, IsCentered ? 2 : 1);
    }
}
