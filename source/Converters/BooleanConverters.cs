using System.Globalization;
using System.Windows.Data;

namespace BIMPlugins.ExtStorage.Converters
{
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string radioValue)
            {
                return radioValue.Equals(value?.ToString());
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value && parameter is string radioValue)
            {
                return radioValue;
            }
            return Binding.DoNothing;
        }
    }

    public class NullableToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
