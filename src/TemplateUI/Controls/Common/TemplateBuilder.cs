using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public static class TemplateBuilder
    {
        public static string GetControlTemplate(string controlName, RenderMode renderMode, VisualType? visualType = null)
        {
            const string SkiaRenderMode = "SK";

            var renderModeString = renderMode == RenderMode.Skia ? SkiaRenderMode : string.Empty;

            if(visualType != null && visualType == VisualType.None)
            {
                if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS)
                    visualType = VisualType.Cupertino;

                if (Device.RuntimePlatform == Device.Android)
                    visualType = VisualType.Material;

                if (Device.RuntimePlatform == Device.UWP || Device.RuntimePlatform == Device.WPF)
                    visualType = VisualType.Fluent;
            }

            return $"{renderModeString}{visualType}{controlName}ControlTemplate";
        }
    }
}