using System.Globalization;

namespace ToDoList.Converters;

public class DueDateToImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime dueDate)
        {
            return dueDate.Date < DateTime.Now.Date ? "calendar_red.png" : "calendar.png";
        }
        return "calendar.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
