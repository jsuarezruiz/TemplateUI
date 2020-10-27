using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace TemplateUI.Converters
{
    public class ColorConverter : IMultiValueConverter
    {
        public double PickerWidth { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
            {
                return 0;
            }

            List<object> valueList = new List<object>();
            valueList.AddRange(values);
            double hexCode = values.GetValue(0) != null ? (double)values.GetValue(0) : 0.0d;
            double red = values.GetValue(1) != null ? (double)values.GetValue(1) : 0.0d;
            double green = values.GetValue(2) != null ? (double)values.GetValue(2) : 0.0d;
            double blue = values.GetValue(3) != null ? (double)values.GetValue(3) : 0.0d;
            double hue = values.GetValue(4) != null ? (double)values.GetValue(4) : 0.0d;
            double saturation = values.GetValue(5) != null ? (double)values.GetValue(5) : 0.0d;
            double lightness = values.GetValue(6) != null ? (double)values.GetValue(6) : 0.0d;
            //double hslHeight = values.GetValue(7) != null ? (double)values.GetValue(7) : 0.0d;
            //double hslWidth = values.GetValue(8) != null ? (double)values.GetValue(8) : 0.0d;

            if (hue != 0)
            {
                double x = PickerWidth / 360 * hue;
                return x;
            }
            return 20;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
