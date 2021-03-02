using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using TemplateUI.Controls;
using TemplateUI.Gallery.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RadialPicker), typeof(RadialPickerRenderer))]

namespace TemplateUI.Gallery.Droid.Renderers
{
    public class RadialPickerRenderer : VisualElementRenderer<AbsoluteLayout>
    {
        private OpacityGradientLayout opacityGradientLayout;

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            Invalidate();
        }

        public RadialPickerRenderer(Context context) : base(context)
        {
            // REDRAW
            this.SetWillNotDraw(false);
        }

        protected override void DispatchDraw(global::Android.Graphics.Canvas canvas)
        {
            // radial gradient
            Xamarin.Forms.Color selectedColor = Xamarin.Forms.Color.Red;
            if (opacityGradientLayout != null)
            {
                selectedColor = this.opacityGradientLayout.SelectedColor;
            }

            var fromSaturation = Xamarin.Forms.Color.FromRgba(1.0d, 1.0d, 1.0d, 1.0d);
            var toSaturation = Xamarin.Forms.Color.FromRgba(1.0d, 1.0d, 1.0d, 0.0d);
            var radialGradient = new RadialGradient(300.0f, 300.0f, 300.0f, fromSaturation.ToAndroid(), toSaturation.ToAndroid(), Shader.TileMode.Clamp);

            // sweep gradient
            int[] colors = new int[] { Android.Graphics.Color.Red.ToArgb(), Android.Graphics.Color.Yellow.ToArgb(), Android.Graphics.Color.Green.ToArgb(), Android.Graphics.Color.Cyan.ToArgb(), Android.Graphics.Color.Blue.ToArgb(), Android.Graphics.Color.Magenta.ToArgb(), Android.Graphics.Color.Red.ToArgb() };
            float[] positions = new float[0];
            var sweepGradient = new SweepGradient(300.0f, 300.0f, colors, positions: null);

            // draw vertical gradient
            var verticalPaint = new Android.Graphics.Paint()
            {
                Dither = true,
            };
            verticalPaint.SetShader(sweepGradient);
            canvas.DrawRect(0, 0, Width, Height, verticalPaint);

            // draw horizontal gradient
            var horizontalPaint = new Android.Graphics.Paint()
            {
                Dither = true,
            };
            horizontalPaint.SetShader(radialGradient);
            canvas.DrawRect(0, 0, Width, Height, horizontalPaint);


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