using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace TemplateUI.DataVisualization
{
    public class ValueAxis : TemplatedView
    {
        public static double ContentWidth = 36;

        const string ElementValues = "PART_ValuesPanel";
        const string ElementTicks = "PART_TickPanel";

        AbsoluteLayout _valuesPanel;
        AbsoluteLayout _tickPanel;

        List<double> _values;
        List<double> _locations;

        readonly List<Label> _valueBoxes;
        readonly List<Line> _valueTicks;

        public ValueAxis()
        {
            _values = new List<double>();
            _locations = new List<double>();

            _valueBoxes = new List<Label>();
            _valueTicks = new List<Line>();

            VerticalOptions = LayoutOptions.Fill;
        }

        public static readonly BindableProperty AxisColorProperty =
            BindableProperty.Create(nameof(AxisColor), typeof(Color), typeof(ValueAxis), Color.Black);

        public Color AxisColor
        {
            get { return (Color)GetValue(AxisColorProperty); }
            set { SetValue(AxisColorProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _valuesPanel = GetTemplateChild(ElementValues) as AbsoluteLayout;
            _tickPanel = GetTemplateChild(ElementTicks) as AbsoluteLayout;
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            Size desiredSize = new Size(0, 0);

            if (!double.IsInfinity(heightConstraint))
                desiredSize.Height = heightConstraint;

            double maxBoxWidth = 0;

            foreach (Label valueBox in _valueBoxes)
            { 
                valueBox.Measure(widthConstraint, heightConstraint);
                maxBoxWidth = Math.Max(maxBoxWidth, valueBox.Width);
            }

            desiredSize.Width = maxBoxWidth;

            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        public void UpdateValues(IEnumerable<double> values)
        {
            _values = new List<double>(values);
            CreateValueObjects();
        }

        public void UpdateLocations(IEnumerable<double> locations)
        {
            _locations = new List<double>(locations);
            UpdateLocation();
            InvalidateMeasure();
        }

        void CreateValueObjects()
        {
            int count = Math.Min(_values.Count, _valueBoxes.Count);

            for (int i = 0; i < count; i++)
                UpdateText(i);

            if (_values.Count != _valueBoxes.Count)
                AddRemoveValueItems(count);
        }

        void AddRemoveValueItems(int count)
        {
            if (_values.Count > _valueBoxes.Count)
                AddValueItems(count);
            else if (_values.Count < _valueBoxes.Count)
                RemoveValueItems(count);
        }
        
        void AddValueItems(int count)
        {
            for (int i = count; i < _values.Count; i++)
            {
                _valueBoxes.Add(new Label
                {
                    FontSize = (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS) ? 8.0 : Device.GetNamedSize(NamedSize.Micro, typeof(Label))
                });

                UpdateText(i);

                _valueTicks.Add(
                    new Line()
                    {
                        Stroke = Color.Black,
                        StrokeThickness = 1,
                        X1 = 0,
                        X2 = 4
                    });

                _valuesPanel.Children.Add(_valueBoxes[i]);
                _tickPanel.Children.Add(_valueTicks[i]);
            }
        }

        void RemoveValueItems(int count)
        {
            for (int i = _valueBoxes.Count - 1; i >= count; i--)
            {
                _valuesPanel.Children.Remove(_valueBoxes[i]);
                _tickPanel.Children.Remove(_valueTicks[i]);
                _valueBoxes.RemoveAt(i);
                _valueTicks.RemoveAt(i);
            }
        }

        void UpdateText(int index)
        {
            _valueBoxes[index].Text = _values[index].ToString();
        }

        void UpdateLocation()
        {
            var max = _locations.Max();
            var height = _valuesPanel.Height;

            for (int i = 0; i < _valueBoxes.Count; i++)
            {
                var y = height - (_locations[i] * height / max);

                AbsoluteLayout.SetLayoutBounds(_valueBoxes[i], new Xamarin.Forms.Rectangle(_valueBoxes[i].X + _valueTicks[i].Width, y - _valueBoxes[i].Height / 2, _valueBoxes[i].Width, _valueBoxes[i].Height));
                AbsoluteLayout.SetLayoutBounds(_valueTicks[i], new Xamarin.Forms.Rectangle(_valueTicks[i].X + _valueTicks[i].Width, y, _valueTicks[i].Width, _valueTicks[i].Height));
            }
        }
    }
}