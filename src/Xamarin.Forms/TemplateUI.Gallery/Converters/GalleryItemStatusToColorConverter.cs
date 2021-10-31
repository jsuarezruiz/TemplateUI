using System;
using System.Globalization;
using TemplateUI.Gallery.Models;
using Xamarin.Forms;

namespace TemplateUI.Gallery.Converters
{
    public class GalleryItemStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var galleryItemStatus = (GalleryItemStatus)value;

            if (galleryItemStatus == GalleryItemStatus.Preview)
                return Color.Orange;

            if (galleryItemStatus == GalleryItemStatus.InProgress)
                return Color.Red;

            if (galleryItemStatus == GalleryItemStatus.New)
                return Color.Green;

            return Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}