using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class GridSplitter : TemplatedView
    {
        const string ElementGridSplitter = "PART_GridSplitter";

        Grid _gridSplitter;

        double _previousPositionX;
        double _previousPositionY;

        public static readonly BindableProperty ElementProperty =
            BindableProperty.Create(nameof(Element), typeof(View), typeof(GridSplitter), null);

        public View Element
        {
            get => (View)GetValue(ElementProperty);
            set => SetValue(ElementProperty, value);
        }

        public static readonly BindableProperty ResizeDirectionProperty =
            BindableProperty.Create(nameof(ResizeDirection), typeof(GridResizeDirection), typeof(GridSplitter), GridResizeDirection.Auto);

        public GridResizeDirection ResizeDirection
        {
            get => (GridResizeDirection)GetValue(ResizeDirectionProperty);
            set => SetValue(ResizeDirectionProperty, value);
        }

        public static new readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(GridSplitter), Color.LightGray);

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _gridSplitter = GetTemplateChild(ElementGridSplitter) as Grid;

            UpdateIsEnabled();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ResizeDirectionProperty.PropertyName)
                UpdateLayout();
            else if (propertyName == IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
        }

        void UpdateIsEnabled()
        {
            if (_gridSplitter == null)
                return;

            if (IsEnabled)
            {
                var panGestureRecognizer = new PanGestureRecognizer();
                panGestureRecognizer.PanUpdated += OnPanUpdated;
                _gridSplitter.GestureRecognizers.Add(panGestureRecognizer);
            }
            else
            {
                // TODO: Remove only specific gestures.
                _gridSplitter.GestureRecognizers.Clear();
            }
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS)
                    {
                        var totalX = e.TotalX - _previousPositionX;
                        var totalY = e.TotalY - _previousPositionY;

                        UpdateLayout(totalX, totalY);

                        _previousPositionX = e.TotalX;
                        _previousPositionY = e.TotalY;
                    }
                    else
                        UpdateLayout(e.TotalX, e.TotalY);
                    break;
                case GestureStatus.Started:
                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    break;
            }
        }

        void UpdateLayout(double offsetX = 0, double offsetY = 0)
        {
            if (!(Parent is Grid))
                // TODO: Throw Exception?
                return;

            if (ResizeDirection == GridResizeDirection.Columns)
                UpdateColumns(offsetX);
            else
                UpdateRows(offsetY);
        }

        void UpdateColumns(double offsetX)
        {
            if (offsetX == 0)
                return;

            var grid = Parent as Grid;
            int column = Grid.GetColumn(this);

            int columnCount = grid.ColumnDefinitions.Count();

            if (columnCount <= 1 || column == 0 || column == columnCount - 1)
                return;
            
            ColumnDefinition previousColumn = grid.ColumnDefinitions[column - 1];

            double previousRowWidth;

            if (previousColumn.Width.IsAbsolute)
                previousRowWidth = previousColumn.Width.Value;
            else
                previousRowWidth = (double)previousColumn.GetType().GetRuntimeProperties().First((p) => p.Name == "ActualWidth").GetValue(previousColumn);

            double actualWidth = previousRowWidth + offsetX;

            if (actualWidth < 0)
                actualWidth = 0;

            previousColumn.Width = new GridLength(actualWidth);
        }

        void UpdateRows(double offsetY)
        {
            if (offsetY == 0)
                return;

            var grid = Parent as Grid;
            var row = Grid.GetRow(this);
            int rowCount = grid.RowDefinitions.Count();

            if (rowCount <= 1 || row == 0 || row == rowCount - 1)
                return;

            RowDefinition previousRow = grid.RowDefinitions[row - 1];

            double previousRowHeight;

            if (previousRow.Height.IsAbsolute)
                previousRowHeight = previousRow.Height.Value;
            else
                previousRowHeight = (double)previousRow.GetType().GetRuntimeProperties().First((p) => p.Name == "ActualHeight").GetValue(previousRow);

            var actualHeight = previousRowHeight + offsetY;

            if (actualHeight < 0)
                actualHeight = 0;

            previousRow.Height = new GridLength(actualHeight);
        }
    }
}