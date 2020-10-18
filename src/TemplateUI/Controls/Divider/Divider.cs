using System;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    // TODO: Replace the BoxView with a Line and add more properties (StrokeDashArray, etc.).
    public class Divider : TemplatedView
    {
        const string ElementContainer = "PART_Container";
        const string ElementLine = "PART_Line";

        Grid _container;
        View _line;

        public static readonly BindableProperty OrientationProperty =
            BindableProperty.Create(nameof(Orientation), typeof(DividerOrientation), typeof(Divider), DividerOrientation.Horizontal,
                propertyChanged: OnOrientationChanged);

        static void OnOrientationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as Divider)?.UpdateSeparator();
        }

        public DividerOrientation Orientation
        {
            get => (DividerOrientation)GetValue(OrientationProperty);
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly BindableProperty LineStrokeProperty =
            BindableProperty.Create(nameof(LineStroke), typeof(Color), typeof(Divider), Color.Black);

        public Color LineStroke
        {
            get => (Color)GetValue(LineStrokeProperty);
            set { SetValue(LineStrokeProperty, value); }
        }

        public static readonly BindableProperty LineStrokeThicknessProperty =
            BindableProperty.Create(nameof(LineStrokeThickness), typeof(double), typeof(Divider), 1.0d,
                propertyChanged: OnLineStrokeThicknessChanged);

        static void OnLineStrokeThicknessChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as Divider)?.UpdateSeparator();
        }

        public double LineStrokeThickness
        {
            get => (double)GetValue(LineStrokeThicknessProperty);
            set { SetValue(LineStrokeThicknessProperty, value); }
        }

        public static readonly BindableProperty RenderModeProperty =
            BindableProperty.Create(nameof(RenderMode), typeof(RenderMode), typeof(Divider), RenderMode.Native,
                propertyChanged: OnRenderModeChanged);

        static void OnRenderModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Divider divider)
            {
                divider.UpdateControlTemplate();
                divider.UpdateSeparator();
            }
        }

        public RenderMode RenderMode
        {
            get => (RenderMode)GetValue(RenderModeProperty);
            set { SetValue(RenderModeProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _container = GetTemplateChild(ElementContainer) as Grid;
            _line = GetTemplateChild(ElementLine) as View;
        }

        void UpdateSeparator()
        {
            if (Orientation == DividerOrientation.Horizontal)
                _container.HeightRequest = _line.HeightRequest = LineStrokeThickness;
            else
                _container.WidthRequest = _line.WidthRequest = LineStrokeThickness;
        }

        void UpdateControlTemplate()
        {
            var template = TemplateBuilder.GetControlTemplate(GetType().Name, RenderMode, null);
            Application.Current.Resources.TryGetValue(template, out object controlTemplate);

            if (controlTemplate == null)
                throw new ArgumentNullException("To use Skia RenderMode you must use TemplateUISkia.Init(); in your Xamarin.Forms Application class.");

            ControlTemplate = controlTemplate as ControlTemplate;
        }
    }
}