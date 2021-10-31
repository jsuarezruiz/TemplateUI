using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace TemplateUI.Controls
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CircleProgressBar : TemplatedView
    {
        const string ElementArc = "PART_Arc";
        const string ElementText = "PART_Text";

        Path _arcSegment;
        Label _arcText;

        public static readonly BindableProperty MinimumProperty =
            BindableProperty.Create(nameof(Minimum), typeof(double), typeof(CircleProgressBar), 0.0d,
                propertyChanged: OnAngleChanged);

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly BindableProperty MaximumProperty =
            BindableProperty.Create(nameof(Maximum), typeof(double), typeof(CircleProgressBar), 10.0d,
                propertyChanged: OnAngleChanged);

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(double), typeof(CircleProgressBar), 0.0d,
                propertyChanged: OnAngleChanged);

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set { SetValue(ValueProperty, value); }
        }

        static void OnAngleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CircleProgressBar)?.UpdateAngle();
        }

        public static readonly BindableProperty ProgressColorProperty =
            BindableProperty.Create(nameof(ProgressColor), typeof(Color), typeof(CircleProgressBar), Color.Black);

        public Color ProgressColor
        {
            get => (Color)GetValue(ProgressColorProperty);
            set { SetValue(ProgressColorProperty, value); }
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(CircleProgressBar), string.Empty);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CircleProgressBar), Color.Black);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(CircleProgressBar), 24.0d);

        [Xamarin.Forms.TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(CircleProgressBar), FontAttributes.None);

        public FontAttributes FontAttributes
        {
            get => (FontAttributes)GetValue(FontAttributesProperty);
            set { SetValue(FontAttributesProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _arcSegment = GetTemplateChild(ElementArc) as Path;
            _arcText = GetTemplateChild(ElementText) as Label;

            UpdateAngle();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            UpdateAngle();
        }

        // TODO: Review and complete the logic to calculate the angle
        void UpdateAngle()
        {
            if (_arcSegment == null)
                return;

            double percentage = Value * 100 / Maximum;
            double angle = percentage * 360 / 100;
            double radius = Width / 2;

            Point startPoint = new Point(radius, 0);
            Point endPoint = GetEndPoint(angle, radius);

            endPoint.X += radius;
            endPoint.Y += radius;

            if (startPoint.X == Math.Round(endPoint.X) && startPoint.Y == Math.Round(endPoint.Y))
                endPoint.X -= 0.01;

            _arcSegment.HorizontalOptions = LayoutOptions.Start;
            _arcSegment.Aspect = Stretch.Fill;
            _arcSegment.HeightRequest = _arcSegment.WidthRequest = radius * 2;

            _arcSegment.Data = new PathGeometry
            {
                Figures = new PathFigureCollection
                {
                    new PathFigure
                    {
                        StartPoint = startPoint,
                        Segments = new PathSegmentCollection
                        {
                            new ArcSegment
                            {
                                RotationAngle = angle,
                                SweepDirection = SweepDirection.Clockwise,
                                Size = new Size(radius, radius),
                                Point = endPoint,
                                IsLargeArc = angle > 180
                            }
                        }
                    }
                }
            };

            if (string.IsNullOrEmpty(Text))
                _arcText.Text = percentage.ToString();
        }


        Point GetEndPoint(double angle, double radius)
        {
            double angleRad = Math.PI / 180.0 * (angle - 90);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);

            return new Point(x, y);
        }
    }
}