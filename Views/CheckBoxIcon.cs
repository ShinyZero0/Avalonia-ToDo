using System;
using Avalonia.Data.Converters;
using Avalonia.Data;
using System.Globalization;
using SkiaSharp;
// using Avalonia.Media.Imaging;

namespace ToDo.Views;
public class GetCheckBoxIcon : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string output;
        if ((bool)value  == false || value is null)
        {
            output = @"Assets/chevron-left.png";
        }
        else
        {
            // return "/Assets/chevron-bottom-512.webp";
            // output = @"Assets/chevron-bottom-512.webp";
            output = @"Assets/chevron-down.png";
        }
        return BitmapAssetValueConverter.Instance.Convert(output, typeof(SKBitmap), parameter, culture);

    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return "";
    }

}
