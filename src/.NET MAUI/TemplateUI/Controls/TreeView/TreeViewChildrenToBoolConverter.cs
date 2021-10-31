using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace TemplateUI.Controls
{
    public class TreeViewChildrenToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int count = (int)value;

            return count > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
