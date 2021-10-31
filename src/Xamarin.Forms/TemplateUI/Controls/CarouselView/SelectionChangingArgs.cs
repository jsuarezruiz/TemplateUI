using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class SelectionChangingArgs
    {
        public View Parent { get; set; }
        public View CurrentView { get; set; }
        public View NextView { get; set; }
        public double Offset { get; set; }
        public SwipeDirection Direction { get; set; }
    }
}