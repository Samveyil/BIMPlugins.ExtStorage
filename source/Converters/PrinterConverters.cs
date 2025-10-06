using Autodesk.Revit.DB;
using System;
using System.Globalization;
using System.Windows.Data;

namespace BIMPlugins.ExtStorage.Converters
{
    public class RasterQualityTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RasterQualityType qualityType)
            {
                switch (qualityType)
                {
                    case RasterQualityType.Low:
                        return "Низкое";
                    
                    case RasterQualityType.Medium:
                        return "Среднее";
                    
                    case RasterQualityType.High:
                        return "Высокое";
                    
                    case RasterQualityType.Presentation:
                        return "Презентационное";
                    
                    default:
                        return "Высокое";
                }
            }
            return "Высокое";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string qualityString)
            {
                switch (qualityString)
                {
                    case "Низкое":
                        return RasterQualityType.Low;
                    
                    case "Среднее":
                        return RasterQualityType.Medium;
                    
                    case "Высокое":
                        return RasterQualityType.High;
                    
                    case "Презентационное":
                        return RasterQualityType.Presentation;
                    
                    default:
                        return RasterQualityType.High;
                }
            }
            return RasterQualityType.High;
        }
    }

    public class ColorDepthTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ColorDepthType colorDepth)
            {
                switch (colorDepth)
                {
                    case ColorDepthType.BlackLine:
                        return "Черные линии";
                    
                    case ColorDepthType.GrayScale:
                        return "Оттенки серого";
                    
                    case ColorDepthType.Color:
                        return "Цвет";
                    
                    default:
                        return "Цвет";
                }
            }
            return "Цвет";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string colorString)
            {
                switch (colorString)
                {
                    case "Черные линии":
                        return ColorDepthType.BlackLine;
                    
                    case "Оттенки серого":
                        return ColorDepthType.GrayScale;
                    
                    default:
                        return ColorDepthType.Color;
                }
            }
            return ColorDepthType.Color;
        }
    }
}
