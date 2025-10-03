using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace BIMPlugins.ExtStorage.Converters
{
    public class EscapeUnderscoreConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
                return str.Replace("_", "__");
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
                return str.Replace("__", "_");
            return value;
        }
    }

    public class NumberToPercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "0%";

            if (value is IConvertible numericValue)
            {
                double number = System.Convert.ToDouble(numericValue);
                return $"{number}%";
            }

            if (value is string strValue && double.TryParse(strValue, out double parsedNumber))
            {
                return $"{parsedNumber}%";
            }

            return "0%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class FilePathToFileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Path.GetFileName(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
