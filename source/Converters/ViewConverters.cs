using Autodesk.Revit.DB;
using System;
using System.Globalization;
using System.Windows.Data;

namespace BIMPlugins.ExtStorage.Converters
{
    public class ViewDuplicateOptionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ViewDuplicateOption viewDuplicateOption)
            {
                switch (viewDuplicateOption)
                {
                    case ViewDuplicateOption.Duplicate:
                        return "Копировать";
                    
                    case ViewDuplicateOption.WithDetailing:
                        return "Копировать с детализацией";
                    
                    default:
                        return "Создать зависимый вид";
                }
            }
            return "Копировать с детализацией";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string viewDuplicateOptionString)
            {
                switch (viewDuplicateOptionString)
                {
                    case "Копировать":
                        return ViewDuplicateOption.Duplicate;
                    
                    case "Копировать с детализацией":
                        return ViewDuplicateOption.WithDetailing;
                    
                    default:
                        return ViewDuplicateOption.AsDependent;
                }
            }
            return ViewDuplicateOption.WithDetailing;
        }
    }
}
