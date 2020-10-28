using System;
using System.Globalization;
using TemplateUI.Helpers;
using Xamarin.Forms;

namespace TemplateUI.Converters
{
    public class ColorNumberConverter : IValueConverter
    {
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
                        return Math.Round(ColorNumberHelper.FromSourceToTargetRGB(doubleValue), 0);
                    case "H":
                        return Math.Round(ColorNumberHelper.FromSourceToTargetHue(doubleValue), 0);
                    case "S":
                        return Math.Round(ColorNumberHelper.FromSourceToTargetSaturation(doubleValue), 0);
                    case "L":
                        return Math.Round(ColorNumberHelper.FromSourceToTargetLuminosity(doubleValue), 0);
                    default:
                        return 0.0;
                }
            }
            return 0.0;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo ci)
        {
            Console.WriteLine($"Convert Back: {value}");
            return value;
        }
    }
}
