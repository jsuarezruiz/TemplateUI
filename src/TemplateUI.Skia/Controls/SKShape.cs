using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using TemplateUI.Skia.Extensions;
using Xamarin.Forms;
using XColor = Xamarin.Forms.Color;

namespace TemplateUI.Skia.Controls
{
    public class SKShape : SKCanvasView
    {
        SkiaSharp.SKPath _skPath;
        readonly SKPaint _paint;
        SKRect _drawableBounds;
        SKRect _pathFillBounds;
        SKRect _pathStrokeBounds;
        SKMatrix _transform;

        XColor _stroke;
        XColor _fill;
        Stretch _stretch;

        float _strokeWidth;
        float[] _strokeDash;
        float _strokeDashOffset;

        public SKShape()
        {
            _paint = new SKPaint
            {
                IsAntialias = true
            };

            _pathFillBounds = new SKRect();
            _pathStrokeBounds = new SKRect();

            _stretch = Stretch.None;
        }

        ~SKShape()
        {
            _paint?.Dispose();
        }

        public static readonly BindableProperty FillProperty =
            BindableProperty.Create(nameof(Fill), typeof(XColor), typeof(SKShape), XColor.Default, BindingMode.OneWay, null,
                propertyChanged: OnFillChanged);

        static void OnFillChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SKShape)?.UpdateFill((XColor)newValue);
        }

        public XColor Fill
        {
            get => (XColor)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create(nameof(Stroke), typeof(XColor), typeof(SKShape), XColor.Default, BindingMode.OneWay, null,
                propertyChanged: OnStrokeChanged);

        static void OnStrokeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SKShape)?.UpdateStroke((XColor)newValue);
        }

        public XColor Stroke
        {
            get => (XColor)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        public static readonly BindableProperty StrokeThicknessProperty =
            BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(SKShape), 1.0d, BindingMode.OneWay, null,
                propertyChanged: OnStrokeThicknessChanged);

        static void OnStrokeThicknessChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SKShape)?.UpdateStrokeThickness((double)newValue);
        }

        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public static readonly BindableProperty AspectProperty =
            BindableProperty.Create(nameof(Aspect), typeof(Stretch), typeof(SKShape), Stretch.Uniform, BindingMode.OneWay, null,
                propertyChanged: OnAspectChanged);

        static void OnAspectChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SKShape)?.UpdateAspect((Stretch)newValue);
        }

        public Stretch Aspect
        {
            get => (Stretch)GetValue(AspectProperty);
            set => SetValue(AspectProperty, value);
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs args)
        {
            SKCanvas canvas = args.Surface.Canvas;
            _drawableBounds = args.Info.Rect;

            canvas.Clear();

            if (_skPath == null)
                return;

            SKMatrix transformMatrix = CreateMatrix();
            SkiaSharp.SKPath transformedSkPath = new SkiaSharp.SKPath();
            _skPath.Transform(transformMatrix, transformedSkPath);
            transformMatrix.MapRect(_pathFillBounds);

            SKRect strokeBounds;
            using (SkiaSharp.SKPath strokePath = new SkiaSharp.SKPath())
            {
                _paint.GetFillPath(transformedSkPath, strokePath);
                strokeBounds = strokePath.Bounds;
            }

            if (_fill != null)
            {
                _paint.Style = SKPaintStyle.Fill;

                SKColor fillColor = XColor.Default.ToSKColor();

                if (_fill is XColor xColor && xColor != XColor.Default)
                    fillColor = xColor.ToSKColor();

                _paint.Color = fillColor;

                canvas.DrawPath(transformedSkPath, _paint);
                _paint.Shader = null;
            }

            if (_stroke != null)
            {
                _paint.Style = SKPaintStyle.Stroke;

                SKColor strokeColor = XColor.Default.ToSKColor();

                if (_stroke is XColor xColor && xColor != XColor.Default)
                    strokeColor = xColor.ToSKColor();

                _paint.Color = strokeColor;

                canvas.DrawPath(transformedSkPath, _paint);
                _paint.Shader = null;
            }
        }

        public void UpdateShape(SkiaSharp.SKPath sKPath)
        {
            _skPath = sKPath;
            UpdatePathShape();
        }

        public void UpdateShapeTransform(SKMatrix matrix)
        {
            _transform = matrix;
            _skPath.Transform(_transform);
            InvalidateSurface();
        }

        public void UpdateAspect(Stretch stretch)
        {
            _stretch = stretch;
            InvalidateSurface();
        }

        public void UpdateFill(XColor fill)
        {
            _fill = fill;
            InvalidateSurface();
        }

        public void UpdateStroke(XColor stroke)
        {
            _stroke = stroke;
            InvalidateSurface();
        }

        public void UpdateStrokeThickness(double strokeWidth)
        {
            _strokeWidth = strokeWidth.ToScaledPixel();
            _paint.StrokeWidth = _strokeWidth;
            UpdateStrokeDash();
        }

        public void UpdateStrokeDashArray(float[] dash)
        {
            _strokeDash = dash;
            UpdateStrokeDash();
        }

        public void UpdateStrokeDashOffset(float strokeDashOffset)
        {
            _strokeDashOffset = strokeDashOffset;
            UpdateStrokeDash();
        }

        public void UpdateStrokeDash()
        {
            if (_strokeDash != null && _strokeDash.Length > 1)
            {
                float[] strokeDash = new float[_strokeDash.Length];

                for (int i = 0; i < _strokeDash.Length; i++)
                    strokeDash[i] = _strokeDash[i] * _strokeWidth;
                _paint.PathEffect = SKPathEffect.CreateDash(strokeDash, _strokeDashOffset * _strokeWidth);
            }
            else
            {
                _paint.PathEffect = null;
            }

            UpdatePathStrokeBounds();
        }

        public void UpdateStrokeLineCap(SKStrokeCap strokeCap)
        {
            _paint.StrokeCap = strokeCap;
            UpdatePathStrokeBounds();
        }
        public void UpdateStrokeLineJoin(SKStrokeJoin strokeJoin)
        {
            _paint.StrokeJoin = strokeJoin;
            InvalidateSurface();
        }

        public void UpdateStrokeMiterLimit(float strokeMiterLimit)
        {
            _paint.StrokeMiter = strokeMiterLimit * 2;
            UpdatePathStrokeBounds();
        }

        protected void UpdatePathShape()
        {
            if (_skPath != null)
            {
                using (SkiaSharp.SKPath fillPath = new SkiaSharp.SKPath())
                {
                    _paint.StrokeWidth = 0.01f;
                    _paint.Style = SKPaintStyle.Stroke;
                    _paint.GetFillPath(_skPath, fillPath);
                    _pathFillBounds = fillPath.Bounds;
                    _paint.StrokeWidth = _strokeWidth;
                }
            }
            else
            {
                _pathFillBounds = SKRect.Empty;
            }

            UpdatePathStrokeBounds();
        }

        SKMatrix CreateMatrix()
        {
            SKMatrix matrix = SKMatrix.CreateIdentity();

            SKRect drawableBounds = _drawableBounds;
            float halfStrokeWidth = _paint.StrokeWidth / 2;

            drawableBounds.Left += halfStrokeWidth;
            drawableBounds.Top += halfStrokeWidth;
            drawableBounds.Right -= halfStrokeWidth;
            drawableBounds.Bottom -= halfStrokeWidth;

            float widthScale = drawableBounds.Width / _pathFillBounds.Width;
            float heightScale = drawableBounds.Height / _pathFillBounds.Height;

            switch (_stretch)
            {
                case Stretch.None:
                    drawableBounds = _drawableBounds;
                    float adjustX = Math.Min(0, _pathStrokeBounds.Left);
                    float adjustY = Math.Min(0, _pathStrokeBounds.Top);
                    if (adjustX < 0 || adjustY < 0)
                    {
                        matrix = SKMatrix.CreateTranslation(-adjustX, -adjustY);
                    }
                    break;
                case Stretch.Fill:
                    matrix = SKMatrix.CreateScale(widthScale, heightScale);
                    matrix = matrix.PostConcat(
                        SKMatrix.CreateTranslation(drawableBounds.Left - widthScale * _pathFillBounds.Left,
                        drawableBounds.Top - heightScale * _pathFillBounds.Top));
                    break;
                case Stretch.Uniform:
                    float minScale = Math.Min(widthScale, heightScale);
                    matrix = SKMatrix.CreateScale(minScale, minScale);
                    matrix = matrix.PostConcat(
                        SKMatrix.CreateTranslation(drawableBounds.Left - (minScale * _pathFillBounds.Left) + (drawableBounds.Width - (minScale * _pathFillBounds.Width)) / 2,
                        drawableBounds.Top - (minScale * _pathFillBounds.Top) + (drawableBounds.Height - (minScale * _pathFillBounds.Height)) / 2));
                    break;
                case Stretch.UniformToFill:
                    float maxScale = Math.Max(widthScale, heightScale);
                    matrix = SKMatrix.CreateScale(maxScale, maxScale);
                    matrix = matrix.PostConcat(
                        SKMatrix.CreateTranslation(drawableBounds.Left - (maxScale * _pathFillBounds.Left),
                        drawableBounds.Top - (maxScale * _pathFillBounds.Top)));
                    break;
            }

            return matrix;
        }

        void UpdatePathStrokeBounds()
        {
            if (_skPath != null)
            {
                using (SkiaSharp.SKPath strokePath = new SkiaSharp.SKPath())
                {
                    _paint.Style = SKPaintStyle.Stroke;
                    _paint.GetFillPath(_skPath, strokePath);
                    _pathStrokeBounds = strokePath.Bounds;
                }
            }
            else
            {
                _pathStrokeBounds = SKRect.Empty;
            }

            InvalidateSurface();
        }
    }
}