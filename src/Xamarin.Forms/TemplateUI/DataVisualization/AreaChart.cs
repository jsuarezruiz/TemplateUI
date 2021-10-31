using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace TemplateUI.DataVisualization
{
    // TODO: Move code to SerialChart class
    public class AreaChart : SerialChart
    {
        const int GridLinesCount = 5;

        Size _chartSize;
        Grid _chartPanel;
        CategoryAxis _categoryAxis;
        ValueAxis _valueAxis;
        GridLines _gridLines;
        Polygon _areaChart;

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

        public AreaChart()
        {
            _valueCollection = new List<double>();
            _categoryCollection = new List<string>();

            _points = new PointCollection();

            _valueAxisValueCollection = new List<double>();
            _valueAxisCategoryCollection = new List<double>();

            _categoryAxisValueCollection = new List<double>();
            _categoryAxisCategoryCollection = new List<string>();
        }

        protected override void OnApplyTemplate()
        {
            InitializeTemplate();
            InitializeCategoryAxis();
            InitializeValueAxis();
            InitializeGridLines();
            InitializeAreaChart();
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
                AddAreaChartToLayout();
                UpdatePoints();
                UpdateAxisData();
                UpdateChartSize();
            }
        }

        void InitializeAreaChart()
        {
            _areaChart = new Polygon
            {
                Aspect = Stretch.Fill,
                Fill = new SolidColorBrush(Color),
                Stroke = new SolidColorBrush(Color),
                StrokeThickness = 2
            };
        }

        void AddAreaChartToLayout()
        {
            _chartPanel.Children.Add(_areaChart);
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
            _areaChart.Stroke = new SolidColorBrush(Color);
            _areaChart.Fill = new SolidColorBrush(Color);
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
            var polygonMargin = GetPolygonMargin();

            var horizontalMargin = polygonMargin / 2;

            var maxLocation = _maximumValue;
            var maxValue = _valueCollection.Max();

            var verticalMargin = Height - (Height * maxValue / maxLocation);

            _areaChart.Margin = new Thickness(horizontalMargin, verticalMargin, horizontalMargin, 0);
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

        double GetPolygonMargin()
        {
            int count = _valueCollection.Count;
            double step = _chartSize.Width / count;

            return step;
        }

        double GetXCoordinate(int index)
        {
            int count = _valueCollection.Count;
            double step = _chartSize.Width / count;

            return step * index + step / 2;
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
            if (_points == null)
                return;

            PointCollection newPoints = GetAreaPoints();

            if (_areaChart.Points.Count != newPoints.Count)
                _areaChart.Points = newPoints;
            else
            {
                for (int i = 0; i < newPoints.Count; i++)
                {
                    if (!_areaChart.Points[i].Equals(newPoints[i]))
                    {
                        _areaChart.Points = newPoints;
                        break;
                    }
                }
            }
        }

        PointCollection GetAreaPoints()
        {
            if (_points == null)
                return new PointCollection();

            var max = _points.Max(p => p.Y);

            if (_points.Count > 0)
            {
                _points.Add(new Point(_points[_points.Count - 1].X, max));
                _points.Add(new Point(_points[0].X, max));
            }

            return _points;
        }
    }
}