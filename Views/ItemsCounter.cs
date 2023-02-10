using System;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Data.Converters;
using ToDo.Models;
using System.Linq;

namespace ToDo.Views;
public class ItemsCounter : IValueConverter
{
    public object? Convert(object? value,
                           Type targetType,
                           object? parameter,
                           CultureInfo culture)
    {
        var list = (ObservableCollection<ToDoItem>) value;
        int cnt = 0;
        foreach (var item in list)
        {
            if (item.IsDone == true)
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
