using System.Globalization;

namespace ToDoList.Converters;

public class NullToTransparentColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null ? Colors.Transparent : Color.FromArgb("#7C838A");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
