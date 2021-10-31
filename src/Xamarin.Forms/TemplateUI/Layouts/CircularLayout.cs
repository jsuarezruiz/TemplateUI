using System;
using TemplateUI.Helpers;
using Xamarin.Forms;

namespace TemplateUI.Layouts
{
    public enum CircularOrientation
    {
        Clockwise,
        Counterclockwise
    }

    /// <summary>
    /// The CircularLayout is a simple Layout derivative that lays out its children in a circular arrangement. 
    /// It has some useful properties to allow some customization like the Orientation (Clockwise or Counterclockwise).
    /// </summary>
    public class CircularLayout : Layout<View>
    {
        public static readonly BindableProperty OrientationProperty =
            BindableProperty.Create(nameof(Orientation), typeof(CircularOrientation), typeof(CircularLayout), CircularOrientation.Clockwise,
                BindingMode.TwoWay, propertyChanged: (bindable, oldvalue, newvalue) => ((CircularLayout)bindable).InvalidateMeasure());

        public CircularOrientation Orientation
        {
            get { return (CircularOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly BindableProperty AngleProperty =
         BindableProperty.Create(nameof(Angle), typeof(double), typeof(CircularLayout), 0.0d,
             BindingMode.TwoWay, null);

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        public static double GetAngle(View obj)
        {
            return (double)obj.GetValue(AngleProperty);
        }

        public static void SetAngle(View obj, double value)
        {
            obj.SetValue(AngleProperty, value);
        }

        public static readonly BindableProperty RadiusProperty =
            BindableProperty.Create(nameof(Radius), typeof(double), typeof(CircularLayout), 0.0d,
                BindingMode.TwoWay, null);

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static double GetRadius(View obj)
        {
            return (double)obj.GetValue(RadiusProperty);
        }

        public static void SetRadius(View obj, double value)
        {
            obj.SetValue(RadiusProperty, value);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (WidthRequest > 0)
                widthConstraint = Math.Min(widthConstraint, WidthRequest);

            if (HeightRequest > 0)
                heightConstraint = Math.Min(heightConstraint, HeightRequest);

            double internalWidth = double.IsPositiveInfinity(widthConstraint) ? double.PositiveInfinity : Math.Max(0, widthConstraint);
            double internalHeight = double.IsPositiveInfinity(heightConstraint) ? double.PositiveInfinity : Math.Max(0, heightConstraint);

            if (double.IsPositiveInfinity(widthConstraint) && double.IsPositiveInfinity(heightConstraint))
            {
                return new SizeRequest(Size.Zero, Size.Zero);
            }

            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            CreateLayout(x, y, width, height);
        }

        private void CreateLayout(double x, double y, double width, double height)
        {
            Point panelCenter = new Point(width / 2, height / 2);

            foreach (View element in Children)
            {
                Point location = CreatePoint(GetAngle(element), GetRadius(element));
                location.X += panelCenter.X;
                location.Y += panelCenter.Y;

                SizeRequest childSizeRequest = element.Measure(double.PositiveInfinity, double.PositiveInfinity);
                Rectangle elementBounds = CreateCenteralizeRectangle(location, new Size(childSizeRequest.Request.Width, childSizeRequest.Request.Height));

                LayoutChildIntoBoundingRegion(element, elementBounds);
            }
        }

        private Rectangle CreateCenteralizeRectangle(Point point, Size size)
        {
            Rectangle result = new Rectangle(
                point.X - size.Width / 2,
                point.Y - size.Height / 2,
                size.Width,
                size.Height);

            return result;
        }

        private Point CreatePoint(double angle, double radius)
        {
            const int ClockwiseAngle = 90;
            const int CounterclockwiseAngle = 270;

            angle = Orientation == CircularOrientation.Clockwise
                ? RadianHelper.DegreeToRadian(angle - ClockwiseAngle)
                : -RadianHelper.DegreeToRadian(angle - CounterclockwiseAngle);

            Point result = new Point(
                radius * Math.Cos(angle),
                radius * Math.Sin(angle));

            return result;
        }
    }
}