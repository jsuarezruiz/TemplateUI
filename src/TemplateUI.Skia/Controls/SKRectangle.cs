using SkiaSharp;
using Xamarin.Forms;
using TemplateUI.Skia.Extensions;

namespace TemplateUI.Skia.Controls
{
    public class SKRectangle : SKShape
    {
        double _height, _width;
        float _radiusX, _radiusY;

        public SKRectangle() : base()
        {
            Aspect = Stretch.Fill;
            UpdateShape();
        }

        public static readonly BindableProperty RadiusXProperty =
            BindableProperty.Create(nameof(RadiusX), typeof(double), typeof(SKRectangle), 0.0d, BindingMode.OneWay, null,
                propertyChanged: OnRadiusXChanged);

        static void OnRadiusXChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SKRectangle)?.UpdateRadiusX((double)newValue);
        }

        public double RadiusX
        {
            get => (double)GetValue(RadiusXProperty);
            set => SetValue(RadiusXProperty, value);
        }

        public static readonly BindableProperty RadiusYProperty =
            BindableProperty.Create(nameof(RadiusY), typeof(double), typeof(SKRectangle), 0.0d, BindingMode.OneWay, null,
                propertyChanged: OnRadiusYChanged);

        static void OnRadiusYChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SKRectangle)?.UpdateRadiusY((double)newValue);
        }

        public double RadiusY
        {
            get => (double)GetValue(RadiusYProperty);
            set => SetValue(RadiusYProperty, value);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            _height = height;
            _width = width;

            UpdateRadiusX(RadiusX);
            UpdateRadiusY(RadiusY);

            base.OnSizeAllocated(width, height);
        }

        void UpdateShape()
        {
            var path = new SkiaSharp.SKPath();
            path.AddRoundRect(new SKRect(0, 0, 1, 1), _radiusX, _radiusY, SKPathDirection.Clockwise);
            UpdateShape(path);
        }

        void UpdateRadiusX(double radiusX)
        {
            if (_width <= 0)
                return;

            _radiusX = (float)(radiusX / _width);
            UpdateShape();
        }

        void UpdateRadiusY(double radiusY)
        {
            if (_height <= 0)
                return;

            _radiusY = (float)(radiusY / _height);
            UpdateShape();
        }
    }
}
