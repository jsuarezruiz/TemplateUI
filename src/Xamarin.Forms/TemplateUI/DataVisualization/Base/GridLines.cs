using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace TemplateUI.DataVisualization
{
    public class GridLines : TemplatedView
    {
        const string ElementGridLayout = "PART_GridLayout";

        AbsoluteLayout _gridLayout;
        List<double> _locations;
        readonly List<Line> _gridLines;

        public GridLines()
        {
            _locations = new List<double>();
            _gridLines = new List<Line>();

            VerticalOptions = LayoutOptions.Fill;
            HorizontalOptions = LayoutOptions.Fill;

            // TODO: Unsubscribe 
            LayoutChanged += OnGridLinesLayoutUpdated;
        }

        void OnGridLinesLayoutUpdated(object sender, object e)
        {
            UpdateGridLines();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _gridLayout = GetTemplateChild(ElementGridLayout) as AbsoluteLayout;
        }

        public void UpdateLocations(IEnumerable<double> locations)
        {
            _locations = new List<double>(locations);
            UpdateGridLines();
        }

        void UpdateGridLines()
        {
            int count = Math.Min(_locations.Count, _gridLines.Count);

            UpdateLineLocations(count);

            if (_locations.Count != _gridLines.Count)
            {
                if (_locations.Count > _gridLines.Count)
                    AddGridLines(count);
                else if (_locations.Count < _gridLines.Count)
                    RemoveGridLines(count);
            }
        }

        void UpdateLineLocations(int count)
        {
            for (int i = 0; i < count; i++)
            {
                UpdateLineX(i);
                UpdateLineY(i);
            }
        }

        void UpdateLineX(int i)
        {
            if (_gridLayout != null && _gridLines.Count > i)
            {
                if (_gridLines[i].X2 != _gridLayout.Width)
                    _gridLines[i].X2 = _gridLayout.Width;
            }
        }

        void UpdateLineY(int i)
        {      
            if (_gridLines.Count > i && _gridLines[i].Y1 != _locations[i])
            {
                var max = _locations.Max();
                var height = _gridLayout.Height;
                var y = 2 * (height - (_locations[i] * height / max)) + 1;
                _gridLines[i].Y1 = _gridLines[i].Y2 = y;
            }
        }

        void AddGridLines(int count)
        {
            for (int i = count; i < _locations.Count; i++)
            {
                Line line = new Line
                {
                    Aspect = Stretch.Fill,
                    Opacity = 0.25,
                    Stroke = new SolidColorBrush(Color.Gray),
                    StrokeThickness = 1,
                    X1 = 0
                };
                _gridLines.Add(line);

                UpdateLineX(i);
                UpdateLineY(i);

                _gridLayout.Children.Add(_gridLines[i]);
            }
        }

        void RemoveGridLines(int count)
        {
            for (int i = _gridLines.Count - 1; i >= count; i--)
            {
                _gridLayout.Children.Remove(_gridLines[i]);
                _gridLines.RemoveAt(i);
            }
        }
    }
}