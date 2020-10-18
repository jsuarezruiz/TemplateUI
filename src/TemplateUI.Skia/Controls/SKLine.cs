using TemplateUI.Skia.Extensions;
using Xamarin.Forms;

namespace TemplateUI.Skia.Controls
{
    public class SKLine : SKShape
    {
        float _x1, _y1, _x2, _y2;

        public SKLine() : base()
        {
        }

        public static readonly BindableProperty X1Property =
           BindableProperty.Create(nameof(X1), typeof(double), typeof(SKLine), 0.0d, BindingMode.OneWay, null,
               propertyChanged: OnX1Changed);

        static void OnX1Changed(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SKLine)?.UpdateX1((float)newValue);
        }

        public double X1
        {
            get => (double)GetValue(X1Property);
            set => SetValue(X1Property, value);
        }

        public static readonly BindableProperty Y1Property =
           BindableProperty.Create(nameof(Y1), typeof(double), typeof(SKLine), 0.0d, BindingMode.OneWay, null,
               propertyChanged: OnY1Changed);

        static void OnY1Changed(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SKLine)?.UpdateY1((float)newValue);
        }

        public double Y1
        {
            get => (double)GetValue(Y1Property);
            set => SetValue(Y1Property, value);
        }

        public static readonly BindableProperty X2Property =
           BindableProperty.Create(nameof(X2), typeof(double), typeof(SKLine), 0.0d, BindingMode.OneWay, null,
               propertyChanged: OnX2Changed);

        static void OnX2Changed(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SKLine)?.UpdateX2((float)newValue);
        }

        public double X2
        {
            get => (double)GetValue(X2Property);
            set => SetValue(X2Property, value);
        }

        public static readonly BindableProperty Y2Property =
           BindableProperty.Create(nameof(Y2), typeof(double), typeof(SKLine), 0.0d, BindingMode.OneWay, null,
               propertyChanged: OnY2Changed);

        static void OnY2Changed(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SKLine)?.UpdateY2((float)newValue);
        }

        public double Y2
        {
            get => (double)GetValue(Y2Property);
            set => SetValue(Y2Property, value);
        }

        void UpdateShape()
        {
            var path = new SkiaSharp.SKPath();
            path.MoveTo(_x1, _y1);
            path.LineTo(_x2, _y2);
            UpdateShape(path);
        }

        void UpdateX1(float x1)
        {
            _x1 = x1.ToScaledPixel();
            UpdateShape();
        }

        void UpdateY1(float y1)
        {
            _y1 = y1.ToScaledPixel();
            UpdateShape();
        }

        void UpdateX2(float x2)
        {
            _x2 = x2.ToScaledPixel();
            UpdateShape();
        }

        void UpdateY2(float y2)
        {
            _y2 = y2.ToScaledPixel();
            UpdateShape();
        }
    }
}
