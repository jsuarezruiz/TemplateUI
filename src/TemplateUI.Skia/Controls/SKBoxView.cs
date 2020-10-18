using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace TemplateUI.Skia.Controls
{
    public class SKBoxView : SKCanvasView
    {
        Color _color;
        CornerRadius _cornerRadius;

        public SKBoxView() : base()
        {
     
        }

        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create(nameof(Color), typeof(Color), typeof(SKBoxView), Color.Default, BindingMode.OneWay, null,
                propertyChanged: OnColorChanged);

        static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SKBoxView)?.UpdateColor((Color)newValue);
        }

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static readonly BindableProperty CornerRadiusProperty =
           BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(SKBoxView), new CornerRadius(), BindingMode.OneWay, null,
               propertyChanged: OnCornerRadiusChanged);

        static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SKBoxView)?.UpdateCornerRadius((CornerRadius)newValue);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs args)
        {
            var surface = args.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            using (var paint = new SKPaint())
            {
                paint.Style = SKPaintStyle.Fill;
                paint.IsAntialias = true;

                SkiaSharp.SKPath path = CreateRoundedRectPath(0, 0, args.Info.Width, args.Info.Height, _cornerRadius);

                paint.Color = _color.ToSKColor();
                canvas.ClipPath(path, SKClipOperation.Intersect, true);
                canvas.DrawPath(path, paint);
            }
        }

        SkiaSharp.SKPath CreateRoundedRectPath(int left, int top, int width, int height, CornerRadius cornerRadius)
        {
            var path = new SkiaSharp.SKPath();
            var skRoundRect = new SKRoundRect(new SKRect(left, top, width, height));

            SKPoint[] radii = new SKPoint[4]
            {
                new SKPoint((float)cornerRadius.TopLeft, (float)cornerRadius.TopLeft),
                new SKPoint((float)cornerRadius.TopRight, (float)cornerRadius.TopRight),
                new SKPoint((float)cornerRadius.BottomRight, (float)cornerRadius.BottomRight),
                new SKPoint((float)cornerRadius.BottomLeft, (float)cornerRadius.BottomLeft)
            };

            skRoundRect.SetRectRadii(skRoundRect.Rect, radii);
            path.AddRoundRect(skRoundRect);
            path.Close();

            return path;
        }

        void UpdateColor(Color color)
        {
            _color = color;
            InvalidateSurface();
        }

        void UpdateCornerRadius(CornerRadius cornerRadius)
        {
            _cornerRadius = cornerRadius;
            InvalidateSurface();
        }
    }
}