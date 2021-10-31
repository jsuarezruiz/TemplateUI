using Microsoft.Maui.Controls;
using Microsoft.Maui.Hosting;
using TemplateUI.Themes;

namespace TemplateUI.Hosting
{
    public static class AppHostBuilderExtensions
    {
        public static MauiAppBuilder ConfigureTemplateUI(this MauiAppBuilder builder)
        {
            InitTemplateUI();

            return builder;
        }

        static void InitTemplateUI()
        {
            var templateUIDictionary = new Generic();

            if (!Application.Current.Resources.MergedDictionaries.Contains(templateUIDictionary))
                Application.Current.Resources.Add(templateUIDictionary);
        }
    }
}
