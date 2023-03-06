using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ToDo.Views;

public class IsNotNullOrWhiteSpace : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string strValue = value as string;
        {
            return (!string.IsNullOrWhiteSpace(strValue));
        }

    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
