using System.ComponentModel;
using CoreAnimation;
using CoreGraphics;
using TemplateUI.Controls;
using TemplateUI.Gallery.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RadialPicker), typeof(RadialGradientLayerRenderer))]

namespace TemplateUI.Gallery.iOS.Renderers
{
    public class RadialGradientLayerRenderer : VisualElementRenderer<AbsoluteLayout>
    {
        public RadialGradientLayerRenderer()
        {
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            RadialPicker radialPicker = (RadialPicker)Element;

            // Conic Hue
            var gradientLayer = new CAGradientLayer();
            gradientLayer.LayerType = CAGradientLayerType.Conic;
            gradientLayer.Colors = new CGColor[] { UIColor.Red.CGColor, UIColor.Yellow.CGColor, UIColor.Green.CGColor, UIColor.Cyan.CGColor, UIColor.Blue.CGColor, UIColor.Magenta.CGColor, UIColor.Red.CGColor };
            gradientLayer.StartPoint = new CGPoint(x: 0.5, y: 0.5);
            gradientLayer.EndPoint = new CGPoint(x: 0.5, y: 0);
            gradientLayer.Frame = rect;

            // Radial Saturation
            var radialSaturation = new CAGradientLayer();
            radialSaturation.LayerType = CAGradientLayerType.Radial;
            var fromColor = Xamarin.Forms.Color.FromRgba(1.0d, 1.0d, 1.0d, 1.0d);
            var fromColor1 = Xamarin.Forms.Color.FromRgba(1.0d, 1.0d, 1.0d, 0.9d);
            var fromColor2 = Xamarin.Forms.Color.FromRgba(1.0d, 1.0d, 1.0d, 0.8d);
            var fromColor3 = Xamarin.Forms.Color.FromRgba(1.0d, 1.0d, 1.0d, 0.7d);
            var fromColor4 = Xamarin.Forms.Color.FromRgba(1.0d, 1.0d, 1.0d, 0.6d);
            var fromColor5 = Xamarin.Forms.Color.FromRgba(1.0d, 1.0d, 1.0d, 0.5d);
            var fromColor6 = Xamarin.Forms.Color.FromRgba(1.0d, 1.0d, 1.0d, 0.4d);
            var fromColor7 = Xamarin.Forms.Color.FromRgba(1.0d, 1.0d, 1.0d, 0.3d);
            var fromColor8 = Xamarin.Forms.Color.FromRgba(1.0d, 1.0d, 1.0d, 0.2d);
            var fromColor9 = Xamarin.Forms.Color.FromRgba(1.0d, 1.0d, 1.0d, 0.1d);
            var toColor = Xamarin.Forms.Color.FromRgba(1.0d, 1.0d, 1.0d, 0.0d);
            //radialSaturation.Colors = new CGColor[] { fromColor.ToCGColor(), fromColor1.ToCGColor(), fromColor2.ToCGColor(), fromColor3.ToCGColor(), fromColor4.ToCGColor(), fromColor5.ToCGColor(), fromColor6.ToCGColor(), fromColor7.ToCGColor(), fromColor8.ToCGColor(), fromColor9.ToCGColor(), toColor.ToCGColor() };
            radialSaturation.Colors = new CGColor[] { fromColor.ToCGColor(), toColor.ToCGColor() };
            CGPoint center = new CGPoint(x: 0.5, y: 0.5);
            radialSaturation.StartPoint = center;
            double radius = 1.0d;
            radialSaturation.EndPoint = new CGPoint(x: radius, y: radius);
            radialSaturation.Frame = rect;

            NativeView.Layer.InsertSublayer(radialSaturation, 0);
            NativeView.Layer.InsertSublayer(gradientLayer, 0);
        }
    }
}