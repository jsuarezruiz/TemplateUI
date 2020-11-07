using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using TemplateUI.Controls;
using TemplateUI.Gallery.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(OpacityGradientLayout), typeof(OpacityGradientLayoutRenderer))]

// @author: https://stackoverflow.com/users/9654227/na2axl
namespace TemplateUI.Gallery.Droid.Renderers
{
    public class OpacityGradientLayoutRenderer : VisualElementRenderer<AbsoluteLayout>
    {
        private OpacityGradientLayout opacityGradientLayout;

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            Invalidate();
        }

        public OpacityGradientLayoutRenderer(Context context) : base(context)
        {
            this.SetWillNotDraw(false);
        }

        protected override void DispatchDraw(global::Android.Graphics.Canvas canvas)
        {
            // horizontal gradient
            Xamarin.Forms.Color selectedColor = Xamarin.Forms.Color.Red;
            if (opacityGradientLayout != null)
            {
                selectedColor = this.opacityGradientLayout.SelectedColor;
            }
            var horizontalGradient = new LinearGradient(0, 0, Width, 0, Android.Graphics.Color.White, selectedColor.ToAndroid(), Shader.TileMode.Clamp);

            // vertical gradient
            Xamarin.Forms.Color alpha100 = Xamarin.Forms.Color.FromHsla(Xamarin.Forms.Color.Black.Hue, 1.0d, 0.5d, 0.0d);
            var verticalGradient = new LinearGradient(0, 0, 0, Height, alpha100.ToAndroid(), Android.Graphics.Color.Black, Shader.TileMode.Clamp);

            // draw horizontal gradient
            var horizontalPaint = new Android.Graphics.Paint()
            {
                Dither = true,
            };
            horizontalPaint.SetShader(horizontalGradient);
            canvas.DrawRect(0, 0, Width, Height, horizontalPaint);

            // draw vertical gradient
            var verticalPaint = new Android.Graphics.Paint()
            {
                Dither = true,
            };
            verticalPaint.SetShader(verticalGradient);
            canvas.DrawRect(0, 0, Width, Height, verticalPaint);

            base.DispatchDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AbsoluteLayout> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }
            try
            {
                this.opacityGradientLayout = e.NewElement as OpacityGradientLayout;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"ERROR:", ex.Message);
            }
        }
    }
}