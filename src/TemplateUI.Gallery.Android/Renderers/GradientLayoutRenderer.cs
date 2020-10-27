using System;
using Android.Content;
using TemplateUI.Controls;
using TemplateUI.Gallery.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(OpacityGradientLayout), typeof(GradientLayoutRenderer))]

// @author: https://stackoverflow.com/users/9654227/na2axl
namespace TemplateUI.Gallery.Droid.Renderers
{
    public class GradientLayoutRenderer : VisualElementRenderer<AbsoluteLayout>
    {
        private Color SelectedColor { get; set; }

        private GradientColorStackMode Mode { get; set; }

        public GradientLayoutRenderer(Context ctx) : base(ctx)
        { }

        protected override void DispatchDraw(global::Android.Graphics.Canvas canvas)
        {
            Android.Graphics.LinearGradient gradient;

            int[] androidColors = new int[] { Color.White.ToAndroid().ToArgb(), SelectedColor.ToAndroid().ToArgb() };

            switch (Mode)
            {
                default:
                case GradientColorStackMode.ToRight:
                    gradient = new Android.Graphics.LinearGradient(0, 0, Width, 0, androidColors, null, Android.Graphics.Shader.TileMode.Mirror);
                    break;
                case GradientColorStackMode.ToLeft:
                    gradient = new Android.Graphics.LinearGradient(Width, 0, 0, 0, androidColors, null, Android.Graphics.Shader.TileMode.Mirror);
                    break;
                case GradientColorStackMode.ToTop:
                    gradient = new Android.Graphics.LinearGradient(0, Height, 0, 0, androidColors, null, Android.Graphics.Shader.TileMode.Mirror);
                    break;
                case GradientColorStackMode.ToBottom:
                    gradient = new Android.Graphics.LinearGradient(0, 0, 0, Height, androidColors, null, Android.Graphics.Shader.TileMode.Mirror);
                    break;
                case GradientColorStackMode.ToTopLeft:
                    gradient = new Android.Graphics.LinearGradient(Width, Height, 0, 0, androidColors, null, Android.Graphics.Shader.TileMode.Mirror);
                    break;
                case GradientColorStackMode.ToTopRight:
                    gradient = new Android.Graphics.LinearGradient(0, Height, Width, 0, androidColors, null, Android.Graphics.Shader.TileMode.Mirror);
                    break;
                case GradientColorStackMode.ToBottomLeft:
                    gradient = new Android.Graphics.LinearGradient(Width, 0, 0, Height, androidColors, null, Android.Graphics.Shader.TileMode.Mirror);
                    break;
                case GradientColorStackMode.ToBottomRight:
                    gradient = new Android.Graphics.LinearGradient(0, 0, Width, Height, androidColors, null, Android.Graphics.Shader.TileMode.Mirror);
                    break;
            }

            var paint = new Android.Graphics.Paint()
            {
                Dither = true,
            };

            paint.SetShader(gradient);
            canvas.DrawPaint(paint);

            base.DispatchDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AbsoluteLayout> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
                return;

            try
            {
                if (e.NewElement is OpacityGradientLayout layout)
                {
                    SelectedColor = layout.SelectedColor;
                    Mode = layout.Mode;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"ERROR:", ex.Message);
            }
        }
    }
}