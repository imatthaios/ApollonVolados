using System.Globalization;

namespace ApollonVolados.Mobile.Converters;

/// <summary>Binding converter that negates a <see cref="bool"/>. Non-bool values convert to <c>false</c>.</summary>
public class InverseBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b && !b;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b && !b;
}
