using System.Globalization;

namespace ToDoList.Converters;

public class GridVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime dueDate)
        {
            return dueDate.Date < DateTime.Now.Date;
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
