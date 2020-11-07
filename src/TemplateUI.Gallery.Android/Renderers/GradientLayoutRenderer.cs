using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using TemplateUI.Controls;
using TemplateUI.Gallery.Droid.Renderers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GradientLayout), typeof(GradientLayoutRenderer))]

// @author: https://stackoverflow.com/users/9654227/na2axl
namespace TemplateUI.Gallery.Droid.Renderers
{
    public class GradientLayoutRenderer : VisualElementRenderer<AbsoluteLayout>
    {
        List<int> droidColorSpectrumArgb = new List<int>();
        public GradientLayoutRenderer(Context context) : base(context)
        {
            List<Xamarin.Forms.Color> colorSpectrum = new List<Xamarin.Forms.Color>
            {
                Xamarin.Forms.Color.FromHsla(0.1d, 1.0d, 0.5d),
                Xamarin.Forms.Color.FromHsla(0.2d, 1.0d, 0.5d),
                Xamarin.Forms.Color.FromHsla(0.3d, 1.0d, 0.5d),
                Xamarin.Forms.Color.FromHsla(0.4d, 1.0d, 0.5d),
                Xamarin.Forms.Color.FromHsla(0.5d, 1.0d, 0.5d),
                Xamarin.Forms.Color.FromHsla(0.6d, 1.0d, 0.5d),
                Xamarin.Forms.Color.FromHsla(0.7d, 1.0d, 0.5d),
                Xamarin.Forms.Color.FromHsla(0.8d, 1.0d, 0.5d),
                Xamarin.Forms.Color.FromHsla(0.9d, 1.0d, 0.5d),
                Xamarin.Forms.Color.FromHsla(1.0d, 1.0d, 0.5d),
            };

            List<Android.Graphics.Color> droidColorSpectrum = new List<Android.Graphics.Color>();
            foreach (Xamarin.Forms.Color col in colorSpectrum)
            {
                droidColorSpectrum.Add(col.ToAndroid());
            }

            foreach (Android.Graphics.Color droidColor in droidColorSpectrum)
            {
                this.droidColorSpectrumArgb.Add(droidColor.ToArgb());
            }
        }

        protected override void DispatchDraw(global::Android.Graphics.Canvas canvas)
        {
            
            var gradient = new LinearGradient(0, 0, 0, Height,
            colors: this.droidColorSpectrumArgb.ToArray(), positions: null, tile: Shader.TileMode.Clamp);

            var paint = new Android.Graphics.Paint()
            {
                Dither = true,
            };
            paint.SetShader(gradient);
            canvas.DrawRect(0, 0, Width, Height, paint);
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
                var gradientLayout = e.NewElement as GradientLayout;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"ERROR:", ex.Message);
            }
        }

        private static int convertDpToPixel(float dp)
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var pixels = dp * mainDisplayInfo.Density;
            return (int)pixels;
        }
    }
}