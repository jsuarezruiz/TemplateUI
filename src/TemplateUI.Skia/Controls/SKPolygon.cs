using SkiaSharp;
using Xamarin.Forms.Shapes;
using TemplateUI.Skia.Extensions;
using Xamarin.Forms;

namespace TemplateUI.Skia.Controls
{
    public class SKPolygon : SKShape
    {
        PointCollection _points;
        bool _fillMode;

        public SKPolygon() : base()
        {
        }

        public static readonly BindableProperty PointsProperty =
            BindableProperty.Create(nameof(Points), typeof(PointCollection), typeof(SKPolygon), new PointCollection(), BindingMode.OneWay, null,
                propertyChanged: OnPointsChanged);

        static void OnPointsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SKPolygon)?.UpdatePoints((PointCollection)newValue);
        }

        public PointCollection Points
        {
            get => (PointCollection)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }

        public static readonly BindableProperty FillRuleProperty =
           BindableProperty.Create(nameof(FillRule), typeof(PointCollection), typeof(SKPolygon), FillRule.EvenOdd, BindingMode.OneWay, null,
               propertyChanged: OnFillModeChanged);

        static void OnFillModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SKPolygon)?.UpdateFillMode((FillRule)newValue == FillRule.EvenOdd);
        }

        public FillRule FillRule
        {
            get => (FillRule)GetValue(FillRuleProperty);
            set => SetValue(FillRuleProperty, value);
        }

        void UpdateShape()
        {
            if (_points != null && _points.Count > 1)
            {
                var path = new SkiaSharp.SKPath
                {
                    FillType = _fillMode ? SKPathFillType.Winding : SKPathFillType.EvenOdd
                };

                path.MoveTo(_points[0].X.ToScaledPixel(), _points[0].Y.ToScaledPixel());
                for (int index = 1; index < _points.Count; index++)
                {
                    path.LineTo(_points[index].X.ToScaledPixel(), _points[index].Y.ToScaledPixel());
                }
                path.Close();

                UpdateShape(path);
            }
        }

        void UpdatePoints(PointCollection points)
        {
            _points = points;
            UpdateShape();
        }

        void UpdateFillMode(bool fillMode)
        {
            _fillMode = fillMode;
            UpdateShape();
        }
    }
}
