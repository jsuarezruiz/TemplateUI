using Xamarin.Forms;

namespace TemplateUI.Controls
{
    // TODO: Replace the BoxView with a Line and add more properties (StrokeDashArray, etc.).
    public class Divider : TemplatedView
    {
        const string ElementContainer = "PART_Container";
        const string ElementLine = "PART_Line";

        Grid _container;
        BoxView _line;

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

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _container = GetTemplateChild(ElementContainer) as Grid;
            _line = GetTemplateChild(ElementLine) as BoxView;
        }

        void UpdateSeparator()
        {
            if (Orientation == DividerOrientation.Horizontal)
                _container.HeightRequest = _line.HeightRequest = LineStrokeThickness;
            else
                _container.WidthRequest = _line.WidthRequest = LineStrokeThickness;
        }
    }
}
