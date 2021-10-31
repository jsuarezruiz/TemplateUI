using Microsoft.Maui.Controls;
using TemplateUI.Themes;

namespace TemplateUI
{
    public static class TemplateUI
    {
        public static void Init()
        {
            var templateUIDictionary = new Generic();

            if (!Application.Current.Resources.MergedDictionaries.Contains(templateUIDictionary))
                Application.Current.Resources.Add(templateUIDictionary);
        }
    }
}
