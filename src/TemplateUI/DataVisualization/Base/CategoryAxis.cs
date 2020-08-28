using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace TemplateUI.DataVisualization
{
    public class CategoryAxis : TemplatedView
    {
        const string ElementValues = "PART_ValuesPanel";
        const string ElementTicks = "PART_TickPanel";
        const string ElementValueAxisColumn = "PART_ValueAxisColumn";

        AbsoluteLayout _valuesPanel;
        AbsoluteLayout _tickPanel;
        ColumnDefinition _gridValueAxis;

        List<string> _values;
        List<double> _locations;

        readonly List<Label> _valueLabels;
        readonly List<Line> _valueTicks;

        public CategoryAxis()
        {
            _values = new List<string>();
            _locations = new List<double>();

            _valueLabels = new List<Label>();
            _valueTicks = new List<Line>();

            HorizontalOptions = LayoutOptions.Fill;
        }

        public static readonly BindableProperty AxisColorProperty =
           BindableProperty.Create(nameof(AxisColor), typeof(Color), typeof(CategoryAxis), Color.Black);

        public Color AxisColor
        {
            get { return (Color)GetValue(AxisColorProperty); }
            set { SetValue(AxisColorProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            _valuesPanel = GetTemplateChild(ElementValues) as AbsoluteLayout;
            _tickPanel = GetTemplateChild(ElementTicks) as AbsoluteLayout;
            _gridValueAxis = GetTemplateChild(ElementValueAxisColumn) as ColumnDefinition;

            if (_gridValueAxis != null)
                _gridValueAxis.Width = ValueAxis.ContentWidth;
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            Size desiredSize = new Size(0, 0);

            if (!double.IsInfinity(widthConstraint))
            {
                desiredSize.Width = widthConstraint;
            }

            double maxHeight = 0;

            foreach (Label label in _valueLabels)
            {
                label.Measure(widthConstraint, heightConstraint);
                maxHeight = Math.Max(maxHeight, label.Height);
            }

            desiredSize.Height = maxHeight;

            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        public void UpdateValues(IEnumerable<string> values)
        {
            _values = new List<string>(values);
            CreateValueItems();
        }

        public void UpdateLocations(IEnumerable<double> locations)
        {
            _locations = new List<double>(locations);
            UpdateLocation();
            InvalidateMeasure();
        }

        void CreateValueItems()
        {
            int count = Math.Min(_values.Count, _valueLabels.Count);

            for (int i = 0; i < count; i++)
                UpdateText(i);

            if (_values.Count != _valueLabels.Count)
                AddRemoveValueItems(count);
        }

        void AddRemoveValueItems(int count)
        {
            if (_values.Count > _valueLabels.Count)
                AddValueItems(count);
            else if (_values.Count < _valueLabels.Count)
                RemoveValueItems(count);
        }

        void AddValueItems(int count)
        {
            for (int i = count; i < _values.Count; i++)
            {
                _valueLabels.Add(new Label
                {
                    HorizontalOptions = LayoutOptions.End,
                    FontSize = (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS) ? 8.0 : Device.GetNamedSize(NamedSize.Micro, typeof(Label))
                });

                UpdateText(i);

                _valueTicks.Add(
                    new Line()
                    {
                        Stroke = Color.Black,
                        StrokeThickness = 1,
                        Y1 = 0,
                        Y2 = 4
                    });

                _valuesPanel.Children.Add(_valueLabels[i]);
                _tickPanel.Children.Add(_valueTicks[i]);
            }
        }

        void RemoveValueItems(int count)
        {
            for (int i = _valueLabels.Count - 1; i >= count; i--)
            {
                _valuesPanel.Children.Remove(_valueLabels[i]);
                _tickPanel.Children.Remove(_valueTicks[i]);

                _valueLabels.RemoveAt(i);
                _valueTicks.RemoveAt(i);
            }
        }

        void UpdateText(int index)
        {
            _valueLabels[index].Text = _values[index];
        }

        void UpdateLocation()
        {
            for (int i = 0; i < _valueLabels.Count; i++)
            {
                AbsoluteLayout.SetLayoutBounds(_valueLabels[i],
                    new Xamarin.Forms.Rectangle(_locations[i] - _valueLabels[i].Width / 2, _valueLabels[i].Y - _valueTicks[i].Height, _valueLabels[i].Width, _valueLabels[i].Height));
                AbsoluteLayout.SetLayoutBounds(_valueTicks[i],
                    new Xamarin.Forms.Rectangle(_locations[i], _valueTicks[i].Y - _valueTicks[i].Height, _valueTicks[i].Width, _valueTicks[i].Height));
            }
        }
    }
}