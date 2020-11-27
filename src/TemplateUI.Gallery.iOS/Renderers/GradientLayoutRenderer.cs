using CoreAnimation;
using CoreGraphics;
using TemplateUI.Controls;
using TemplateUI.Gallery.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GradientLayout), typeof(GradientLayoutRenderer))]

namespace TemplateUI.Gallery.iOS.Renderers
{
    public class GradientLayoutRenderer : VisualElementRenderer<AbsoluteLayout>
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            GradientLayout layout = (GradientLayout)Element;

            CGColor[] colors = new CGColor[layout.Colors.Length];

            for (int i = 0, l = colors.Length; i < l; i++)
            {
                colors[i] = layout.Colors[i].ToCGColor();
            }

            var gradientLayer = new CAGradientLayer();

            switch (layout.Mode)
            {
                default:
                case GradientColorStackMode.ToRight:
                    gradientLayer.StartPoint = new CGPoint(0, 0.5);
                    gradientLayer.EndPoint = new CGPoint(1, 0.5);
                    break;
                case GradientColorStackMode.ToLeft:
                    gradientLayer.StartPoint = new CGPoint(1, 0.5);
                    gradientLayer.EndPoint = new CGPoint(0, 0.5);
                    break;
                case GradientColorStackMode.ToTop:
                    gradientLayer.StartPoint = new CGPoint(0.5, 0);
                    gradientLayer.EndPoint = new CGPoint(0.5, 1);
                    break;
                case GradientColorStackMode.ToBottom:
                    gradientLayer.StartPoint = new CGPoint(0.5, 1);
                    gradientLayer.EndPoint = new CGPoint(0.5, 0);
                    break;
                case GradientColorStackMode.ToTopLeft:
                    gradientLayer.StartPoint = new CGPoint(1, 0);
                    gradientLayer.EndPoint = new CGPoint(0, 1);
                    break;
                case GradientColorStackMode.ToTopRight:
                    gradientLayer.StartPoint = new CGPoint(0, 1);
                    gradientLayer.EndPoint = new CGPoint(1, 0);
                    break;
                case GradientColorStackMode.ToBottomLeft:
                    gradientLayer.StartPoint = new CGPoint(1, 1);
                    gradientLayer.EndPoint = new CGPoint(0, 0);
                    break;
                case GradientColorStackMode.ToBottomRight:
                    gradientLayer.StartPoint = new CGPoint(0, 0);
                    gradientLayer.EndPoint = new CGPoint(1, 1);
                    break;
            }

            gradientLayer.Frame = rect;
            gradientLayer.Colors = colors;

            NativeView.Layer.InsertSublayer(gradientLayer, 0);
        }
    }
}
