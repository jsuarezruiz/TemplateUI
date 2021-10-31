using TemplateUI.Themes;
using Xamarin.Forms;

namespace TemplateUI
{
    public class TemplateUI
    {
        // TODO: Accept parameters to allow loading only resources to be used.
        public static void Init()
        { 
            var templateUIDictionary = new Generic();

            if (!Application.Current.Resources.MergedDictionaries.Contains(templateUIDictionary))
                Application.Current.Resources.Add(templateUIDictionary);
        }
    }
}