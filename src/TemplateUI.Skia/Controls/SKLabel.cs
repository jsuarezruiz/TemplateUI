using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace TemplateUI.Skia.Controls
{
    public class SKLabel : SKCanvasView
    {
        SizeRequest? _lastSize;
        readonly SKPaint _paint;

        public SKLabel()
        {
            _paint = new SKPaint();
        }

        ~SKLabel()
        {
            _paint?.Dispose();
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(SKLabel), string.Empty, BindingMode.OneWay, null,
                propertyChanged: OnResizeChanged);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(SKLabel), Color.Black, BindingMode.OneWay, null,
                propertyChanged: OnInvalidateChanged);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(SKLabel), 14.0, BindingMode.OneWay, null,
                propertyChanged: OnResizeChanged);

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        static void OnInvalidateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SKLabel view = bindable as SKLabel;

            view?.RefreshPaint();
            view?.InvalidateSurface();
        }

        static void OnResizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SKLabel view = bindable as SKLabel;

            view?.RefreshPaint();
            view?.InvalidateMeasure();
            view?.InvalidateSurface();
        }

        protected override void InvalidateMeasure()
        {
            _lastSize = null;
            base.InvalidateMeasure();
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (_lastSize == null && ShouldDraw())
            {
                SKRect textBounds = new SKRect();
                Rectangle requestRect;

                _paint.MeasureText(Text, ref textBounds);
                requestRect = textBounds.ToFormsRect();

                requestRect.Height = _paint.FontMetrics.Descent - _paint.FontMetrics.Ascent;
                requestRect.Width = Math.Ceiling(Math.Min(requestRect.Width, widthConstraint));
                requestRect.Height = Math.Ceiling(Math.Min(requestRect.Height, heightConstraint));

                _lastSize = new SizeRequest(requestRect.Size);
            }

            return _lastSize.GetValueOrDefault(new SizeRequest());
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs args)
        {
            if (ShouldDraw())
            {
                SKCanvas canvas = args.Surface.Canvas;

                canvas.Scale(args.Info.Width / (float)Width);
                canvas.Clear();
                canvas.DrawText(Text, 0, (float)Height - _paint.FontMetrics.Descent * 0.5f, _paint);
            }
        }

        bool ShouldDraw()
        {
            return !string.IsNullOrEmpty(Text) && TextColor != Color.Transparent && FontSize > 0;
        }

        void RefreshPaint()
        {
            _paint.Color = TextColor.ToSKColor();
            _paint.TextSize = (float)FontSize;
            _paint.IsAntialias = true;
        }
    }
}