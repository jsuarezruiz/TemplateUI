using Xamarin.Forms;

namespace TemplateUI.Skia.Controls
{
    public class SKEllipse : SKShape
    {
        public SKEllipse() : base()
        {
            Aspect = Stretch.Fill;
            UpdateShape();
        }

        void UpdateShape()
        {
            var path = new SkiaSharp.SKPath();
            path.AddCircle(0, 0, 1);
            UpdateShape(path);
        }
    }
}