using System;
using System.Globalization;
using TemplateUI.Gallery.Models;
using Xamarin.Forms;

namespace TemplateUI.Gallery.Converters
{
    public class GalleryItemStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var galleryItemStatus = (GalleryItemStatus)value;

            if (galleryItemStatus == GalleryItemStatus.Preview)
                return "Preview";

            if (galleryItemStatus == GalleryItemStatus.InProgress)
                return "In Progress";

            if (galleryItemStatus == GalleryItemStatus.New)
                return "New";

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}