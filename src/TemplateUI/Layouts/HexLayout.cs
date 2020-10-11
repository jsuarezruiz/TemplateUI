using System;
using Xamarin.Forms;

namespace TemplateUI.Layouts
{
    /// <summary>
    /// A Layout that arranges the elements in a honeycomb pattern.
    /// Based on https://github.com/AlexanderSharykin/HexGrid. Created by Alexander Sharykin.
    /// </summary>
    public class HexLayout : Layout<View>
    {
        public static readonly BindableProperty OrientationProperty =
         BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(HexLayout), StackOrientation.Vertical,
             BindingMode.TwoWay, propertyChanged: (bindable, oldvalue, newvalue) => ((HexLayout)bindable).InvalidateMeasure());

        public StackOrientation Orientation
        {
            get { return (StackOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly BindableProperty ColumnCountProperty =
            BindableProperty.Create(nameof(ColumnCount), typeof(int), typeof(HexLayout), 1,
                BindingMode.TwoWay, propertyChanged: (bindable, oldvalue, newvalue) => ((HexLayout)bindable).InvalidateMeasure());

        public int ColumnCount
        {
            get { return (int)GetValue(ColumnCountProperty); }
            set { SetValue(ColumnCountProperty, value); }
        }

        public static readonly BindableProperty RowCountProperty =
            BindableProperty.Create(nameof(RowCount), typeof(int), typeof(HexLayout), 1,
                BindingMode.TwoWay, propertyChanged: (bindable, oldvalue, newvalue) => ((HexLayout)bindable).InvalidateMeasure());

        public int RowCount
        {
            get { return (int)GetValue(RowCountProperty); }
            set { SetValue(RowCountProperty, value); }
        }

        public static readonly BindableProperty ColumnProperty =
            BindableProperty.Create(nameof(Column), typeof(int), typeof(HexLayout), 1,
                BindingMode.TwoWay, null);

        public int Column
        {
            get { return (int)GetValue(ColumnProperty); }
            set { SetValue(ColumnProperty, value); }
        }

        private int GetColumn(VisualElement e)
        {
            int column = (int)e.GetValue(ColumnProperty);
            if (column >= ColumnCount)
                column = ColumnCount - 1;
            return column;
        }

        public static readonly BindableProperty RowProperty =
            BindableProperty.Create(nameof(Row), typeof(int), typeof(HexLayout), 1,
                BindingMode.TwoWay, null);

        public int Row
        {
            get { return (int)GetValue(RowProperty); }
            set { SetValue(RowProperty, value); }
        }

        private int GetRow(VisualElement e)
        {
            int row = (int)e.GetValue(RowProperty);
            if (row >= RowCount)
                row = RowCount - 1;
            return row;
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            double w = widthConstraint;
            double h = heightConstraint;

            if (double.IsInfinity(w) || double.IsInfinity(h))
            {
                h = 0;
                w = 0;

                foreach (View child in Children)
                {
                    var childSize = child.Measure(widthConstraint, heightConstraint);

                    if (childSize.Request.Height > h)
                        h = childSize.Request.Height;

                    if (childSize.Request.Width > w)
                        w = childSize.Request.Width;
                }

                if (Orientation == StackOrientation.Horizontal)
                    return new SizeRequest(new Size(w * (ColumnCount * 3 + 1) / 4, h * (RowCount * 2 + 1) / 2));

                return new SizeRequest(new Size(w * (ColumnCount * 2 + 1) / 2, h * (RowCount * 3 + 1) / 4));
            }

            return Measure(widthConstraint, heightConstraint);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            HasShift(out bool first, out bool last);

            Size hexSize = GetHexSize(new Size(width, height));

            double columnWidth, rowHeight;

            if (Orientation == StackOrientation.Horizontal)
            {
                rowHeight = 0.5 * hexSize.Height;
                columnWidth = 0.25 * hexSize.Width;
            }
            else
            {
                rowHeight = 0.25 * hexSize.Height;
                columnWidth = 0.5 * hexSize.Width;
            }

            var elements = Children;
            for (int i = 0; i < elements.Count; i++)
            {
                if (!elements[i].IsVisible)
                    continue;

                LayoutChild(elements[i], hexSize, columnWidth, rowHeight, first);
            }
        }

        private void LayoutChild(VisualElement element, Size hexSize, double columnWidth, double rowHeight, bool shift)
        {
            int row = GetRow(element);
            int column = GetColumn(element);

            double x;
            double y;

            if (Orientation == StackOrientation.Horizontal)
            {
                x = 3 * columnWidth * column;
                y = rowHeight * (2 * row + (column % 2 == 1 ? 1 : 0) + (shift ? -1 : 0));
            }
            else
            {
                x = columnWidth * (2 * column + (row % 2 == 1 ? 1 : 0) + (shift ? -1 : 0));
                y = 3 * rowHeight * row;
            }

            LayoutChildIntoBoundingRegion(element, new Rectangle(x, y, hexSize.Width, hexSize.Height));
        }

        private void HasShift(out bool first, out bool last)
        {
            if (Orientation == StackOrientation.Horizontal)
                HasRowShift(out first, out last);
            else
                HasColumnShift(out first, out last);
        }

        private void HasRowShift(out bool firstRow, out bool lastRow)
        {
            firstRow = lastRow = true;

            var elements = Children;
            for (int i = 0; i < elements.Count && (firstRow || lastRow); i++)
            {
                var e = elements[i];

                if (!e.IsVisible)
                    continue;

                int row = GetRow(e);
                int column = GetColumn(e);

                int mod = column % 2;

                if (row == 0 && mod == 0)
                    firstRow = false;

                if (row == RowCount - 1 && mod == 1)
                    lastRow = false;
            }
        }

        private void HasColumnShift(out bool firstColumn, out bool lastColumn)
        {
            firstColumn = lastColumn = true;

            var elements = Children;
            for (int i = 0; i < elements.Count && (firstColumn || lastColumn); i++)
            {
                var e = elements[i];

                if (!e.IsVisible)
                    continue;

                int row = GetRow(e);
                int column = GetColumn(e);

                int mod = row % 2;

                if (column == 0 && mod == 0)
                    firstColumn = false;

                if (column == ColumnCount - 1 && mod == 1)
                    lastColumn = false;
            }
        }

        private Size GetHexSize(Size gridSize)
        {
            double minH = 0;
            double minW = 0;

            foreach (var e in Children)
            {
                if (e is VisualElement f)
                {
                    if (f.MinimumHeightRequest > minH)
                        minH = f.MinimumHeightRequest;

                    if (f.MinimumWidthRequest > minW)
                        minW = f.MinimumWidthRequest;
                }
            }

            HasShift(out bool first, out bool last);

            var possibleSize = GetPossibleSize(gridSize);
            double possibleW = possibleSize.Width;
            double possibleH = possibleSize.Height;

            var w = Math.Max(minW, possibleW);
            var h = Math.Max(minH, possibleH);

            return new Size(w, h);
        }

        private Size GetPossibleSize(Size gridSize)
        {
            HasShift(out bool first, out bool last);

            if (Orientation == StackOrientation.Horizontal)
                return GetPossibleSizeHorizontal(gridSize, first, last);

            return GetPossibleSizeVertical(gridSize, first, last);
        }

        private Size GetPossibleSizeVertical(Size gridSize, bool first, bool last)
        {
            int columns = (first ? 0 : 1) + 2 * ColumnCount - (last ? 1 : 0);
            double w = 2 * (gridSize.Width / columns);

            int rows = 1 + 3 * RowCount;
            double h = 4 * (gridSize.Height / rows);

            return new Size(w, h);
        }

        private Size GetPossibleSizeHorizontal(Size gridSize, bool first, bool last)
        {
            int columns = 1 + 3 * ColumnCount;
            double w = 4 * (gridSize.Width / columns);

            int rows = (first ? 0 : 1) + 2 * RowCount - (last ? 1 : 0);
            double h = 2 * (gridSize.Height / rows);

            return new Size(w, h);
        }
    }
}