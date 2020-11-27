using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using TemplateUI.Controls;
using TemplateUI.Gallery.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GradientLayout), typeof(GradientLayoutRenderer))]

// @author: https://stackoverflow.com/users/9654227/na2axl
namespace TemplateUI.Gallery.Droid.Renderers
{
    public class GradientLayoutRenderer : VisualElementRenderer<AbsoluteLayout>
    {
        private GradientLayout gradientLayout;
        List<int> droidColorSpectrumArgb = new List<int>();

        public GradientLayoutRenderer(Context context) : base(context)
        {
        }

        protected override void DispatchDraw(global::Android.Graphics.Canvas canvas)
        {
            int startX;
            int startY;
            int endX;
            int endY;

            switch (this.gradientLayout.Mode)
            {
                case GradientColorStackMode.ToLeft:
                    startX = 0;
                    startY = 0;
                    endX = Width;
                    endY = 0;
                    break;
                case GradientColorStackMode.ToRight:
                    startX = Width;
                    startY = 0;
                    endX = 0;
                    endY = 0;
                    break;
                case GradientColorStackMode.ToBottom:
                    startX = 0;
                    startY = Height;
                    endX = 0;
                    endY = 0;
                    break;
                case GradientColorStackMode.ToTop:
                    startX = 0;
                    startY = 0;
                    endX = 0;
                    endY = Height;
                    break;
                case GradientColorStackMode.ToTopLeft:
                    startX = 0;
                    startY = 0;
                    endX = Width;
                    endY = Height;
                    break;
                case GradientColorStackMode.ToTopRight:
                    startX = Width;
                    startY = 0;
                    endX = 0;
                    endY = Height;
                    break;
                case GradientColorStackMode.ToBottomLeft:
                    startX = 0;
                    startY = Height;
                    endX = Width;
                    endY = 0;
                    break;
                case GradientColorStackMode.ToBottomRight:
                    startX = Width;
                    startY = Height;
                    endX = 0;
                    endY = 0;
                    break;
                default:
                    startX = 0;
                    startY = 0;
                    endX = 0;
                    endY = Height;
                    break;
            }

            LinearGradient gradient = new LinearGradient(startX, startY, endX, endY, colors: this.droidColorSpectrumArgb.ToArray(), positions: null, tile: Shader.TileMode.Clamp);

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
                this.gradientLayout = e.NewElement as GradientLayout;
                List<Xamarin.Forms.Color> colorSpectrum = new List<Xamarin.Forms.Color>();
                colorSpectrum.AddRange(gradientLayout.Colors);

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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"ERROR:", ex.Message);
            }
        }
    }
}