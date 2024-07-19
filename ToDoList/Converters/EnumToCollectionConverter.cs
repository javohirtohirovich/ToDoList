using System.Globalization;
using System.Reflection;

namespace ToDoList.Converters
{
    public class EnumToCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumType = parameter as Type;
            if (enumType != null && enumType.IsEnum)
            {
                return Enum.GetValues(enumType).Cast<object>().ToList();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is Type enumType && value is string stringValue)
            {
                foreach (var field in enumType.GetFields(BindingFlags.Static | BindingFlags.Public))
                {
                    if (field.Name.Equals(stringValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return field.GetValue(null);
                    }
                }
            }
            return null;
        }
    }
}
