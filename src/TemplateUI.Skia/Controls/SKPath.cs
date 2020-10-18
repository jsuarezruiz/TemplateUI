using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using TemplateUI.Skia.Extensions;

namespace TemplateUI.Skia.Controls
{
    public class SKPath : SKShape
    {
        public SKPath() : base()
        {
        }

        public static readonly BindableProperty DataProperty =
            BindableProperty.Create(nameof(Data), typeof(Geometry), typeof(SKPath), null, BindingMode.OneWay, null,
                propertyChanged: OnDataChanged);

        static void OnDataChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SKPath)?.UpdateData(((Geometry)newValue).ToSKPath());
        }

        public Geometry Data
        {
            get => (Geometry)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly BindableProperty TransformProperty =
            BindableProperty.Create(nameof(Transform), typeof(Transform), typeof(SKPath), null, BindingMode.OneWay, null,
                propertyChanged: OnTransformChanged);

        static void OnTransformChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SKPath)?.UpdateTransform(((Transform)newValue).ToSKMatrix());
        }

        public Transform Transform
        {
            get => (Transform)GetValue(TransformProperty);
            set => SetValue(TransformProperty, value);
        }

        void UpdateData(SkiaSharp.SKPath path)
        {
            UpdateShape(path);
        }

        void UpdateTransform(SKMatrix transform)
        {
            UpdateShapeTransform(transform);
        }
    }
}
