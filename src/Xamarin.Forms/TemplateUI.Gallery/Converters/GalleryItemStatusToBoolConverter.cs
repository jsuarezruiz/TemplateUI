using System;
using System.Globalization;
using Xamarin.Forms;
using TemplateUI.Gallery.Models;

namespace TemplateUI.Gallery.Converters
{
    public class GalleryItemStatusToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var galleryItemStatus = (GalleryItemStatus)value;

            if (galleryItemStatus == GalleryItemStatus.Preview || galleryItemStatus == GalleryItemStatus.InProgress)
                return true;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}