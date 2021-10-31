using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace TemplateUI.Controls
{
    public class ElementToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
