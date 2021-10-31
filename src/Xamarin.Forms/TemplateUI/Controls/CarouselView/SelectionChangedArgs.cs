using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public enum SelectionChanged
    {
        Completed,
        Reset
    }

    public class SelectionChangedArgs
    {
        public View Parent { get; set; }
        public View CurrentView { get; set; }
        public View NextView { get; set; }
        public SwipeDirection Direction { get; set; }
        public SelectionChanged Status { get; set; }
    }
}