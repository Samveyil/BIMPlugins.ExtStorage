using BIMPlugins.ExtStorage.Extensions;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Binding = System.Windows.Data.Binding;

namespace BIMPlugins.ExtStorage.Converters
{
    public class ImageOnLoadConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imagePath = value as string;

            if (imagePath.IsNullOrEmpty())
            {
                return new BitmapImage();
            }

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(imagePath, UriKind.Absolute);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            return bitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}