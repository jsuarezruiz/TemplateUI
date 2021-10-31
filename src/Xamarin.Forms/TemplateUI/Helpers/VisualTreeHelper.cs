using Xamarin.Forms;

namespace TemplateUI.Helpers
{
    // TODO: Complete adding methods to get childrens, etc.
    public static class VisualTreeHelper
    {
        public static T GetParent<T>(this Element element) where T : Element
        {
            if (element is T)
                return element as T;
            else
            {
                if (element.Parent != null)
                    return element.Parent.GetParent<T>();

                return default;
            }
        }
    }
}