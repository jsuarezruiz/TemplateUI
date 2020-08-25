using Xamarin.Forms;

namespace TemplateUI.Gallery.Models
{
    public class GalleryItem
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Icon { get; set; }
        public Color Color { get; set; }
        public GalleryItemStatus Status { get; set; }
    }
}