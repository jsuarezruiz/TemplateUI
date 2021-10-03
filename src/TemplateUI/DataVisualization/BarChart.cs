using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace TemplateUI.DataVisualization
{
    // TODO: Move code to SerialChart class
    public class BarChart : SerialChart
    {
        const int GridLinesCount = 5;

        Size _chartSize;
        Grid _chartPanel;
        CategoryAxis _categoryAxis;
        ValueAxis _valueAxis;
        GridLines _gridLines;
        Path _barChart;
        PathGeometry _barChartGeometry;

        double _minimumValue;
        double _maximumValue;

        double _gridLinesStep;

        readonly List<double> _valueCollection;
        readonly List<string> _categoryCollection;

        readonly PointCollection _points;

        readonly List<double> _valueAxisValueCollection;
        readonly List<double> _valueAxisCategoryCollection;

        readonly List<double> _categoryAxisValueCollection;
        readonly List<string> _categoryAxisCategoryCollection;

        public BarChart()
        {
            _valueCollection = new List<double>();
            _categoryCollection = new List<string>();

            _points = new PointCollection();

            _valueAxisValueCollection = new List<double>();
            _valueAxisCategoryCollection = new List<double>();

            _categoryAxisValueCollection = new List<double>();
            _categoryAxisCategoryCollection = new List<string>();
        }

        protected double XStep
        {
            get
            {
                if (_points.Count > 1)
                    return _points[1].X - _points[0].X;
                else if (_points.Count == 1)
                    return _points[0].X * 2;
                else
                    return 0;
            }
        }

        protected override void OnApplyTemplate()
        {
            InitializeTemplate();
            InitializeCategoryAxis();
            InitializeValueAxis();
            InitializeGridLines();
            InitializeBarChart();
        }

        void InitializeTemplate()
        {
            _chartPanel = GetTemplateChild("PART_Chart") as Grid;
            _chartPanel.Padding = new Thickness(0, 0, ValueAxis.ContentWidth, 0);
            _chartPanel.SizeChanged += OnChartPanelSizeChanged;
        }

        void InitializeCategoryAxis()
        {
            _categoryAxis = GetTemplateChild("PART_CategoryAxis") as CategoryAxis;
        }

        void InitializeValueAxis()
        {
            _valueAxis = GetTemplateChild("PART_ValueAxis") as ValueAxis;
        }

        void InitializeGridLines()
        {
            _gridLines = GetTemplateChild("PART_GridLines") as GridLines;
        }

        void OnChartPanelSizeChanged(object sender, EventArgs e)
        {
            _chartSize = new Size(_chartPanel.Width - _chartPanel.Padding.Right, _chartPanel.Height);

            if (_chartSize.Height > 0 && _chartSize.Width > 0)
            {
                AddBarChartToLayout();
                UpdatePoints();
                UpdateAxisData();
                UpdateChartSize();
            }
        }

        void InitializeBarChart()
        {
            _barChart = new Path
            {
                Aspect = Stretch.Fill,
                Stroke = new SolidColorBrush(Color),
                Fill = new SolidColorBrush(Color),
                StrokeThickness = 1,
                Margin = new Thickness(24, 0)
            };

            _barChartGeometry = new PathGeometry();
        }

        void AddBarChartToLayout()
        {
            _chartPanel.Children.Add(_barChart);
        }

        public override void UpdateDataSource()
        {
            if (DataSource == null)
                return;

            UpdateValueData();
            UpdateCategoryData();
        }

        public override void UpdateColor()
        {
            _barChart.Stroke = new SolidColorBrush(Color);
            _barChart.Fill = new SolidColorBrush(Color);
        }

        void UpdateValueData()
        {
            _valueCollection.Clear();

            if (DataSource != null && !string.IsNullOrEmpty(ValueMemberPath))
            {
                BindingValue eval = new BindingValue(ValueMemberPath);

                foreach (object dataItem in this.DataSource)
                {
                    _valueCollection.Add(Convert.ToDouble(eval.Eval(dataItem)));
                }
            }
        }

        void UpdateCategoryData()
        {
            _categoryCollection.Clear();

            if (DataSource != null && !string.IsNullOrEmpty(CategoryMemberPath))
            {
                BindingValue eval = new BindingValue(CategoryMemberPath);

                foreach (object dataItem in this.DataSource)
                {
                    _categoryCollection.Add(eval.Eval(dataItem).ToString());
                }
            }
        }

        void UpdatePoints()
        {
            if (_valueCollection.Count == 0)
                return;

            var minimumValue = _valueCollection.Min();
            var maximumValue = _valueCollection.Max();

            AdjustMinMax(minimumValue, maximumValue);
            UpdatePointData();
        }

        void UpdateAxisData()
        {
            if (_valueAxis != null && _categoryAxis != null)
            {
                UpdateCategoryAxisData();

                _categoryAxis.UpdateValues(_categoryAxisCategoryCollection);
                _categoryAxis.UpdateLocations(_categoryAxisValueCollection);

                UpdateValueAxisData();

                _valueAxis.UpdateValues(_valueAxisValueCollection);
                _valueAxis.UpdateLocations(_valueAxisValueCollection);

                _gridLines.UpdateLocations(_valueAxisValueCollection);
            }
        }

        void UpdateCategoryAxisData()
        {
            int gridCount = GetCategoryGridCount(GridLinesCount);

            if (gridCount != 0)
            {
                int gridStep = _categoryCollection.Count / gridCount;

                _categoryAxisCategoryCollection.Clear();
                _categoryAxisValueCollection.Clear();

                if (gridStep > 0)
                {
                    for (int i = 0; i < _categoryCollection.Count; i += gridStep)
                    {
                        _categoryAxisCategoryCollection.Add(_categoryCollection[i]);
                        _categoryAxisValueCollection.Add(_points[i].X);
                    }
                }
            }
        }

        void UpdateValueAxisData()
        {
            _valueAxisCategoryCollection.Clear();
            _valueAxisValueCollection.Clear();

            foreach (double value in _valueCollection)
            {
                _valueAxisCategoryCollection.Add(value);
            }

            if (_valueAxis != null && _gridLinesStep > 0)
            {
                _valueAxisValueCollection.Clear();

                for (double d = _minimumValue + _gridLinesStep; d <= _maximumValue; d += _gridLinesStep)
                    _valueAxisValueCollection.Add(d);
            }
        }

        void UpdateChartSize()
        {
            var pathMargin = GetPathMargin();

            var horizontalMargin = pathMargin / 2;

            var maxLocation = _maximumValue;
            var maxValue = _valueCollection.Max();

            var verticalMargin = Height - (Height * maxValue / maxLocation);

            _barChart.Margin = new Thickness(horizontalMargin, verticalMargin, horizontalMargin, 0);
        }

        int GetCategoryGridCount(int gridCountHint)
        {
            int gridCount = gridCountHint;

            if (gridCountHint >= _categoryCollection.Count)
            {
                gridCount = _categoryCollection.Count;
            }
            else
            {
                int hint = gridCountHint;
                while ((_categoryCollection.Count - 1) % hint != 0 && hint > 1)
                    hint--;

                if (hint == 1)
                {
                    hint = gridCountHint;
                    while ((_categoryCollection.Count - 1) % hint != 0 && hint < Math.Min(_categoryCollection.Count, gridCountHint * 2))
                        hint++;
                }

                if (hint < gridCountHint * 2)
                    gridCount = hint;
            }

            return gridCount;
        }

        void AdjustMinMax(double minimumValue, double maximumValue)
        {
            double min = minimumValue;
            double max = maximumValue;

            if (min == 0 && max == 0)
                max = 9;
    
            if (min > max)
                min = max - 1;

            double initial_min = min;
            double initial_max = max;

            double dif = max - min;
            double dif_e;

            if (dif == 0)
                dif_e = Math.Pow(10, Math.Floor(Math.Log(Math.Abs(max)) * Math.Log10(Math.E))) / 10;
            else
                dif_e = Math.Pow(10, Math.Floor(Math.Log(Math.Abs(dif)) * Math.Log10(Math.E))) / 10;

            max = Math.Ceiling(max / dif_e) * dif_e + dif_e;
            min = Math.Floor(min / dif_e) * dif_e - dif_e;

            dif = max - min;
            dif_e = Math.Pow(10, Math.Floor(Math.Log(Math.Abs(dif)) * Math.Log10(Math.E))) / 10;

            double step = Math.Ceiling((dif / 5) / dif_e) * dif_e;
            double step_e = Math.Pow(10, Math.Floor(Math.Log(Math.Abs(step)) * Math.Log10(Math.E)));

            double temp = Math.Ceiling(step / step_e);	//number from 1 to 10

            if (temp > 5)
                temp = 10;

            if (temp <= 5 && temp > 2)
                temp = 5;

            step = Math.Ceiling(step / (step_e * temp)) * step_e * temp;

            min = step * Math.Floor(min / step);
            max = step * Math.Ceiling(max / step);

            if (min < 0 && initial_min >= 0)
                min = 0;

            if (max > 0 && initial_max <= 0)
                max = 0;

            _gridLinesStep = step;
            _minimumValue = min;
            _maximumValue = max;
        }

        double GetPathMargin()
        {
            int count = _valueCollection.Count;
            double step = _chartSize.Width / count;

            return step / 2;
        }

        double GetXCoordinate(int index)
        {
            int count = _valueCollection.Count;
            double step = _chartSize.Width / count;

            return (step * index) + (step / 2);
        }

        double GetYCoordinate(double value)
        {
            return _chartSize.Height - _chartSize.Height * ((value - _minimumValue) / (_maximumValue - _minimumValue));
        }

        Point GetPointCoordinates(int index, double value)
        {
            return new Point(GetXCoordinate(index), GetYCoordinate(value));
        }

        void UpdatePointData()
        {
            _points.Clear();

            if (_valueCollection.Count > 0)
            {
                for (int i = 0; i < _valueCollection.Count; i++)
                {
                    _points.Add(GetPointCoordinates(i, _valueCollection[i]));
                }
            }

            RenderChart();
        }

        void RenderChart()
        {
            if (_points != null)
            {
                int changeCount = Math.Min(_points.Count, _barChartGeometry.Figures.Count);
                ChangeColumns(changeCount, _barChartGeometry);
                int diff = _points.Count - _barChartGeometry.Figures.Count;    

                if (diff > 0)
                    AddColumns(changeCount, _barChartGeometry);
                else if (diff < 0)
                    RemoveColumns(changeCount);

                _barChart.Data = _barChartGeometry;
            }
        }

        void AddColumns(int changeCount, PathGeometry barChartGeometry)
        {
            for (int i = changeCount; i < _points.Count; i++)
            {
                PathFigure column = new PathFigure();
                barChartGeometry.Figures.Add(column);

                for (int si = 0; si < 4; si++)
                    column.Segments.Add(new LineSegment());

                SetColumnSegments(i, barChartGeometry);
            }
        }

        void RemoveColumns(int changeCount)
        {
            for (int i = _barChartGeometry.Figures.Count - 1; i >= changeCount; i--)
                _barChartGeometry.Figures.RemoveAt(i);
        }

        void ChangeColumns(int changeCount, PathGeometry barChartGeometry)
        {
            for (int i = 0; i < changeCount; i++)
                SetColumnSegments(i, barChartGeometry);
        }
                
        void SetColumnSegments(int index, PathGeometry barChartGeometry)
        {
            double width = XStep * 0.5;
            double left = _points[index].X - width / 2;
            double right = left + width;
            double max = _points.Max(p => p.Y);
            double y1 = max;
            double y2 = _points[index].Y;

            barChartGeometry.Figures[index].StartPoint = new Point(left, y1);
            (barChartGeometry.Figures[index].Segments[0] as LineSegment).Point = new Point(right, y1);
            (barChartGeometry.Figures[index].Segments[1] as LineSegment).Point = new Point(right, y2);
            (barChartGeometry.Figures[index].Segments[2] as LineSegment).Point = new Point(left, y2);
            (barChartGeometry.Figures[index].Segments[3] as LineSegment).Point = new Point(left, y1);
        }
    }
}