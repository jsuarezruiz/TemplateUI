using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using TemplateUI.Skia.Extensions;
using Xamarin.Forms;

namespace TemplateUI.Skia.Controls
{
    public class SKFrame : ContentView, IDisposable
    {
        readonly Grid _root;
        readonly SKCanvasView _canvas;
        readonly float _scalingFactor;

        public SKFrame()
        {
            _scalingFactor = Device.info.ScalingFactor == 0 ? 1 : (float)Device.info.ScalingFactor;

            _root = new Grid()
            {
                Margin = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            _canvas = new SKCanvasView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            _canvas.PaintSurface += OnPaintSurface;
            _root.Children.Add(_canvas);

            base.Content = _root;
        }

        new public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(SKFrame), null,
                propertyChanged: OnContentChanged);

        static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SKFrame sKFrame)
            {
                sKFrame.UpdateContent();
                sKFrame.UpdateCanvas();
            }
        }

        public new View Content
        {
            set => SetValue(ContentProperty, value);
            get => (View)GetValue(ContentProperty);
        }

        public static new readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(SKFrame), Color.White,
                propertyChanged: OnFrameChanged);

        static void OnFrameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SKFrame)?.UpdateCanvas();
        }

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(SKFrame), Color.Default,
                propertyChanged: OnFrameChanged);

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(SKFrame), default(CornerRadius),
                propertyChanged: OnFrameChanged);

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly BindableProperty HasShadowProperty =
            BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(SKFrame), default(bool),
                propertyChanged: OnFrameChanged);

        public bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }

        protected void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            int width = info.Width;
            int height = info.Height;

            float x = 0;
            float y = 0;

            if (HasShadow)
            {
                DrawShadow(canvas, width, height);

                var drawPadding = 2;

                width -= drawPadding * 2;
                height -= drawPadding * 2;

                x = drawPadding;
                y = drawPadding;
            }

            var skRect = new SKRect
            {
                Left = x + _scalingFactor,
                Top = y + _scalingFactor,
                Right = width - _scalingFactor,
                Bottom = height - _scalingFactor
            };

            SKRoundRect roundRect = new SKRoundRect(skRect);

            //Set Corner Radius for each corner                       
            SKPoint[] radii = new SKPoint[]
            {
                new SKPoint((float)CornerRadius.TopLeft * _scalingFactor, (float)CornerRadius.TopLeft * _scalingFactor),
                new SKPoint((float)CornerRadius.TopRight * _scalingFactor, (float)CornerRadius.TopRight * _scalingFactor),
                new SKPoint((float)CornerRadius.BottomRight * _scalingFactor, (float)CornerRadius.BottomRight * _scalingFactor),
                new SKPoint((float)CornerRadius.BottomLeft * _scalingFactor, (float)CornerRadius.BottomLeft * _scalingFactor)
            };

            roundRect.SetRectRadii(skRect, radii);

            if (BorderColor != Color.Default)
            {
                DrawBorder(canvas, roundRect);
            }

            DrawBackground(canvas, roundRect);
        }

        void UpdateContent()
        {
            if (Content == null)
                return;

            _root.Children.Add(Content);
        }

        void UpdateCanvas()
        {
            _canvas?.InvalidateSurface();
        }

        // TODO: Improve the Shadow drawing
        void DrawShadow(SKCanvas canvas, int width, int height)
        {
            const int ShadowDistance = 5;
            Color ShadowColor = Color.Black;
            const int ShadowBlur = 10;
            const float ShadowSigma = -6f;

            using (var shadowPaint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            })
            {
                var shadowDistance = Convert.ToSingle(ShadowDistance);
                var padding = Convert.ToSingle(ShadowBlur * 2);

                shadowPaint.MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, Convert.ToSingle(ShadowBlur));

                var diameter = padding * 2;
                var rectangleWidth = width - diameter;
                var rectangleHeight = height - diameter;

                using (var path = CreatePath(rectangleWidth, rectangleHeight, padding, _scalingFactor))
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    var imageFilter = SKImageFilter.CreateDropShadow(
                        shadowDistance,
                        shadowDistance,
                        ShadowSigma,
                        ShadowSigma,
                        ShadowColor.ToSKColor(), 
                        SKDropShadowImageFilterShadowMode.DrawShadowOnly);
#pragma warning restore CS0618 // Type or member is obsolete

                    shadowPaint.ImageFilter = imageFilter;
                    canvas.DrawPath(path, shadowPaint);
                }
            }
        }

        void DrawBackground(SKCanvas canvas, SKRoundRect roundRect)
        {
            using (var backgroundPaint = new SKPaint()
            {
                Color = (BackgroundColor == Color.Default) ? SKColors.White : BackgroundColor.ToSKColor(),
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            })
            {
                SkiaSharp.SKPath path = CreateRoundedRectPath(roundRect);
                canvas.DrawPath(path, backgroundPaint);
                canvas.ClipPath(path);
            }
        }

        void DrawBorder(SKCanvas canvas, SKRoundRect roundRect)
        {
            const float StrokeWidth = 6;

            using (SKPaint borderPaint = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = StrokeWidth,
                Color = BorderColor.ToSKColor(),
                IsAntialias = true
            })
            {
                SkiaSharp.SKPath path = CreateRoundedRectPath(roundRect);
                canvas.DrawPath(path, borderPaint);
            }
        }

        SkiaSharp.SKPath CreatePath(float width, float height, float padding, float scalingFactor)
        {
            SkiaSharp.SKPath path = new SkiaSharp.SKPath();

            var topLeftRadius = Convert.ToSingle(CornerRadius.TopLeft * scalingFactor);
            var topRightRadius = Convert.ToSingle(CornerRadius.TopRight * scalingFactor);
            var bottomLeftRadius = Convert.ToSingle(CornerRadius.BottomLeft * scalingFactor);
            var bottomRightRadius = Convert.ToSingle(CornerRadius.BottomRight * scalingFactor);

            var x = topLeftRadius + padding;
            var y = padding;

            path.MoveTo(x, y);

            path.LineTo(width - topRightRadius + padding, y);
            path.ArcTo(topRightRadius, new SKPoint(width + padding, topRightRadius + padding));

            path.LineTo(width + padding, height - bottomRightRadius + padding);
            path.ArcTo(bottomRightRadius,
                 new SKPoint(width - bottomRightRadius + padding, height + padding));

            path.LineTo(bottomLeftRadius + padding, height + padding);
            path.ArcTo(bottomLeftRadius,
                new SKPoint(padding, height - bottomLeftRadius + padding));

            path.LineTo(padding, topLeftRadius + padding);
            path.ArcTo(topLeftRadius, new SKPoint(x, y));

            path.Close();

            return path;
        }

        SkiaSharp.SKPath CreateRoundedRectPath(SKRoundRect roundRect)
        {
            SkiaSharp.SKPath skPath = new SkiaSharp.SKPath();
            skPath.AddRoundRect(roundRect, SKPathDirection.Clockwise);
            skPath.Close();

            return skPath;
        }

        public void Dispose()
        {
            _canvas.PaintSurface -= OnPaintSurface;
        }
    }
}