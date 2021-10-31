using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using TemplateUI.Gallery.Models;
using System;
using System.Globalization;

namespace TemplateUI.Gallery.Converters
{
    public class GalleryItemStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var galleryItemStatus = (GalleryItemStatus)value;

            if (galleryItemStatus == GalleryItemStatus.Preview)
                return Colors.Orange;

            if (galleryItemStatus == GalleryItemStatus.InProgress)
                return Colors.Red;

            if (galleryItemStatus == GalleryItemStatus.New)
                return Colors.Green;

            return Colors.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}