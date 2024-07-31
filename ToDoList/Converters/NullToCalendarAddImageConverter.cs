using System.Globalization;

namespace ToDoList.Converters;

public class NullToCalendarAddImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null ? "calendar_add.png" : "calendar_white.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
