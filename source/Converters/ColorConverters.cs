using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BIMPlugins.ExtStorage.Converters
{
    public class RevitColorToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Autodesk.Revit.DB.Color revitColor)
            {
                var wpfColor = Color.FromRgb(
                    revitColor.Red,
                    revitColor.Green,
                    revitColor.Blue
                );

                return new SolidColorBrush(wpfColor);
            }

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Обратное преобразование не требуется");
        }
    }
}
