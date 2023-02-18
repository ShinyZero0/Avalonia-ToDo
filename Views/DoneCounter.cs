using System;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Data.Converters;
using Avalonia.Collections;
using ToDo.Models;
using System.Linq;
using Avalonia.Controls;

namespace ToDo.Views;
public class DoneCounter : IValueConverter
{
    public object? Convert(object? value,
                           Type targetType,
                           object? parameter,
                           CultureInfo culture)
    {
        var list = (IEnumerable<ToDoItem>)value;
        int cnt = 0;
        foreach (var item in list)
        {
            if (((ToDoItem)item).IsDone == true)
            {
                cnt++;
            }
        }

        string v = $"Выполнено {cnt} задач из {list.Count()}";
        return v;
    }
    public object ConvertBack( object? value, Type targetType, object? parameter, CultureInfo culture )
    {
        throw new NotSupportedException();
    }
}
