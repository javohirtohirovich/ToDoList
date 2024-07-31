using System.Globalization;

namespace ToDoList.Converters;

public class DueDateToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime dueDate)
        {
            return dueDate.Date < DateTime.Now.Date ? Color.FromArgb("#EE4E4E") : Colors.Grey;
        }
        return Colors.Grey;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
