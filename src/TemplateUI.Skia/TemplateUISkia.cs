using TemplateUI.Skia.Themes;
using Xamarin.Forms;

namespace TemplateUI.Skia
{
    public class TemplateUISkia
    {
        // TODO: Accept parameters to allow loading only resources to be used.
        public static void Init()
        {
            var templateUISkiaDictionary = new Generic();

            if (!Application.Current.Resources.MergedDictionaries.Contains(templateUISkiaDictionary))
                Application.Current.Resources.Add(templateUISkiaDictionary);
        }
    }
}