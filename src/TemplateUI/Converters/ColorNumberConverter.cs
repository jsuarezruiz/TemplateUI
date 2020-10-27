using System;
using System.Globalization;
using Xamarin.Forms;

namespace TemplateUI.Converters
{
    public class ColorNumberConverter : IValueConverter
    {
        readonly int maxRGBNumber = 255;
        readonly int maxHue = 359;
        readonly int maxSaturation = 100;
        readonly int maxLightness = 100;

        public object Convert(object value, Type targetType, object parameter, CultureInfo ci)
        {
            if (value != null && value is double)
            {
                double doubleValue = (double)value;
                switch (parameter)
                {
                    case "R":
                    case "G":
                    case "B":
                        return Math.Round(doubleValue * maxRGBNumber, 0);
                    case "H":
                        return Math.Round(doubleValue * maxHue, 0);
                    case "S":
                        return Math.Round(doubleValue * maxSaturation, 0);
                    case "L":
                        return Math.Round(doubleValue * maxLightness, 0);
                    default:
                        return 0.0;
                }
            }
            return 0.0;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo ci)
        {
            Console.WriteLine(value);
            return value;
        }
    }
}
