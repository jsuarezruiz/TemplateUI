using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class ComparerView : TemplatedView
    {
        const double ComparerThumbSize = 40;

        const string ElementTarget = "PART_Target";
        const string ElementTargetContaner = "PART_TargetContainer";
        const string ElementSource = "PART_Source";
        const string ElementThumbContainer = "PART_ThumbContainer";
        const string ElementThumb = "PART_Thumb";

        View _targetView;
        View _sourceView;
        Grid _targetContainer;
        Grid _thumbContainer;
        ComparerThumb _thumb;

        double _previousPositionX;
        double _previousPositionY;

        public static readonly BindableProperty TargetViewProperty =
           BindableProperty.Create(nameof(TargetView), typeof(View), typeof(ComparerView), null);

        public View TargetView
        {
            get => (View)GetValue(TargetViewProperty);
            set { SetValue(TargetViewProperty, value); }
        }

        public static readonly BindableProperty SourceViewProperty =
           BindableProperty.Create(nameof(SourceView), typeof(View), typeof(ComparerView), null);

        public View SourceView
        {
            get => (View)GetValue(SourceViewProperty);
            set { SetValue(SourceViewProperty, value); }
        }

        public static readonly BindableProperty OrientationProperty =
            BindableProperty.Create(nameof(Orientation), typeof(ComparerOrientation), typeof(ComparerView), ComparerOrientation.Horizontal,
                propertyChanged: OnOrientationChanged);

        static void OnOrientationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ComparerView)?.UpdateOrientation();
        }

        public ComparerOrientation Orientation
        {
            get => (ComparerOrientation)GetValue(OrientationProperty);
            set { SetValue(OrientationProperty, value); }
        }

        public static new readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(TargetView), typeof(Color), typeof(ComparerView), Color.Default);

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create(nameof(Color), typeof(Color), typeof(ComparerView), Color.Gray,
                propertyChanged: OnColorChanged);

        static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ComparerView)?.UpdateColor();
        }

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set { SetValue(ColorProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _targetView = GetTemplateChild(ElementTarget) as View;
            _sourceView = GetTemplateChild(ElementSource) as View;
            _targetContainer = GetTemplateChild(ElementTargetContaner) as Grid;
            _thumbContainer = GetTemplateChild(ElementThumbContainer) as Grid;
            _thumb = GetTemplateChild(ElementThumb) as ComparerThumb;

            UpdateIsEnabled();
            UpdateOrientation();
            UpdateColor();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
        }

        void UpdateIsEnabled()
        {
            if(IsEnabled)
            {
                var panGestureRecognizer = new PanGestureRecognizer();
                panGestureRecognizer.PanUpdated += OnPanUpdated;
                _thumbContainer.GestureRecognizers.Add(panGestureRecognizer);
            }
            else
                _thumbContainer.GestureRecognizers.Clear();
        }

        void UpdateOrientation()
        {
            if (_thumb == null)
                return;

            // TODO: Calculate Thumb size

            if (Orientation == ComparerOrientation.Horizontal)
            {
                _thumbContainer.HorizontalOptions = LayoutOptions.Start;
                _thumbContainer.VerticalOptions = LayoutOptions.Fill;
                _thumbContainer.Margin = new Thickness(-ComparerThumbSize / 2, 0, 0, 0);
            }
            else
            {
                _thumbContainer.HorizontalOptions = LayoutOptions.Fill;
                _thumbContainer.VerticalOptions = LayoutOptions.Start;
                _thumbContainer.Margin = new Thickness(0, -ComparerThumbSize / 2, 0, 0);
            }

            _thumb.Orientation = Orientation;
        }

        void UpdateColor()
        {
            if (_thumb == null)
                return;

            _thumb.Color = Color;
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _previousPositionX = e.TotalX;
                    _previousPositionY = e.TotalY;

                    if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS)
                    {
                        _previousPositionX += _thumbContainer.TranslationX;
                        _previousPositionY += _thumbContainer.TranslationY;
                    }
                    break;
                case GestureStatus.Running:

                    if (Orientation == ComparerOrientation.Horizontal)
                    {
                        var delta = _previousPositionX+ e.TotalX;

                        if (Device.RuntimePlatform == Device.Android)
                            delta += _thumbContainer.TranslationX;

                        if (IsValidDelta(delta))
                            UpdateThumbPosition(delta);
                    }
                    else
                    {
                        var delta = _previousPositionY + e.TotalY;

                        if (Device.RuntimePlatform == Device.Android)
                            delta += _thumbContainer.TranslationY;

                        if (IsValidDelta(delta))
                            UpdateThumbPosition(delta);
                    }
                    break;
                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    break;
            }
        }

        bool IsValidDelta(double delta)
        {
            if (Orientation == ComparerOrientation.Horizontal)
            {
                if (delta <= 0 || delta > Width)
                    return false;
            }
            else
            {
                if (delta <= 0 || delta > Height)
                    return false;
            }

            return true;
        }

        void UpdateThumbPosition(double delta)
        {
            if (Math.Abs(delta) < 0.01)
                return;

            if (Orientation == ComparerOrientation.Horizontal)
            {
                _thumbContainer.TranslationX = delta;
                _targetContainer.TranslationX = delta;
                _targetView.TranslationX = -delta;
            }
            else
            {
                _thumbContainer.TranslationY = delta;
                _targetContainer.TranslationY = delta;
                _targetView.TranslationY = -delta;
            }
        }
    }
}