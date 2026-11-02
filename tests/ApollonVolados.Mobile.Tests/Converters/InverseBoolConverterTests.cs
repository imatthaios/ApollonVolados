using System.Globalization;
using ApollonVolados.Mobile.Converters;

namespace ApollonVolados.Mobile.Tests.Converters;

public class InverseBoolConverterTests
{
    private readonly InverseBoolConverter _sut = new();

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void Convert_negates_bool(bool input, bool expected)
    {
        Assert.Equal(expected, _sut.Convert(input, typeof(bool), null, CultureInfo.InvariantCulture));
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void ConvertBack_negates_bool(bool input, bool expected)
    {
        Assert.Equal(expected, _sut.ConvertBack(input, typeof(bool), null, CultureInfo.InvariantCulture));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("true")]
    [InlineData(1)]
    public void Non_bool_values_convert_to_false(object? input)
    {
        Assert.Equal(false, _sut.Convert(input, typeof(bool), null, CultureInfo.InvariantCulture));
        Assert.Equal(false, _sut.ConvertBack(input, typeof(bool), null, CultureInfo.InvariantCulture));
    }
}
