using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class Slider : TemplatedView
    {
        const string ElementTrackBackground = "PART_TrackBackground";
        const string ElementProgress = "PART_Progress";
        const string ElementThumb = "PART_Thumb";

        View _trackBackground;
        BoxView _progress;
        ContentView _thumb;
        double _previousPosition;
        double? _previousVal = null;


        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        double ThumbHalfWidth => (_thumb?.Width ?? 0) / 2;

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(double), typeof(Slider), 0.0d, propertyChanged: OnValueChanged);

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Slider slider)
                slider.UpdateValue();
        }

        public static readonly BindableProperty ThumbProperty =
            BindableProperty.Create(nameof(Thumb), typeof(View), typeof(Slider), null, propertyChanged: ThumbChanged);

        public View Thumb
        {
            get => (View)GetValue(ThumbProperty);
            set => SetValue(ThumbProperty, value);
        }
        private static void ThumbChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Slider slider)
                slider._thumb.Content = newValue as View;
        }

        public static readonly BindableProperty MinimumProperty =
            BindableProperty.Create(nameof(Minimum), typeof(double), typeof(Slider), 0.0d);

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public static readonly BindableProperty MaximumProperty =
            BindableProperty.Create(nameof(Maximum), typeof(double), typeof(Slider), 10.0d);

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public static readonly BindableProperty TrackSizeProperty =
           BindableProperty.Create(nameof(TrackSize), typeof(double), typeof(Slider), 1.0d);

        public double TrackSize
        {
            get => (double)GetValue(TrackSizeProperty);
            set => SetValue(TrackSizeProperty, value);
        }

        public static readonly BindableProperty MinimumTrackColorProperty =
            BindableProperty.Create(nameof(MinimumTrackColor), typeof(Color), typeof(Slider), Color.White);

        public Color MinimumTrackColor
        {
            get => (Color)GetValue(MinimumTrackColorProperty);
            set => SetValue(MinimumTrackColorProperty, value);
        }

        public static readonly BindableProperty MaximumTrackColorProperty =
            BindableProperty.Create(nameof(MaximumTrackColor), typeof(Color), typeof(Slider), Color.Default);

        public Color MaximumTrackColor
        {
            get => (Color)GetValue(MaximumTrackColorProperty);
            set => SetValue(MaximumTrackColorProperty, value);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _trackBackground = GetTemplateChild(ElementTrackBackground) as View;
            _progress = GetTemplateChild(ElementProgress) as BoxView;
            _thumb = GetTemplateChild(ElementThumb) as ContentView;
            UpdateIsEnabled();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            UpdateValue(true);
        }

        void UpdateIsEnabled()
        {
            if (IsEnabled)
            {
                var panGestureRecognizer = new PanGestureRecognizer();
                panGestureRecognizer.PanUpdated += OnThumbPanUpdated;
                _thumb.GestureRecognizers.Add(panGestureRecognizer);
            }
            else
            {
                _trackBackground.GestureRecognizers.Clear();
                _thumb.GestureRecognizers.Clear();
            }
        }

        void UpdateValue(bool isNecessary = false)
        {
            var min = Minimum;
            var max = Maximum;
            var val = Value;
            var valChecked = CheckValueByRange(val, min, max);
            if (valChecked != val)
            {
                Value = valChecked;
                return;
            }
            ValueChanged?.Invoke(this, new ValueChangedEventArgs(valChecked));

            if (!isNecessary && _previousVal == val)
                return;

            var half = ThumbHalfWidth;
            var thumbCenterpostion = ConvertRangeValue(val, min, max, half, _trackBackground.Width -half);
            SetThumbPosition(thumbCenterpostion - half, thumbCenterpostion, val);
        }

        void OnThumbPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _previousPosition = e.TotalX;

                    if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS)
                        _previousPosition += _thumb.TranslationX;
                    break;
                case GestureStatus.Running:
                    var totalX = _previousPosition + e.TotalX;
                    var half = ThumbHalfWidth;
                    var maxPosition = _trackBackground.Width - half;

                    if (Device.RuntimePlatform == Device.Android)
                        totalX += _thumb.TranslationX;

                    var thumbCenterPostion = CheckValueByRange(totalX + half, half, maxPosition);
                    var val = ConvertRangeValue(thumbCenterPostion, half, maxPosition, Minimum, Maximum);
                    SetThumbPosition(thumbCenterPostion - half, thumbCenterPostion, val);
                    Value = val;
                    break;
                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    break;
            }
        }

        double ConvertRangeValue(double oldVal, double oldMin, double oldMax, double min, double max)
        {
            var relativeValue = (oldVal - oldMin) / (oldMax - oldMin);
            return min + (max - min) * relativeValue;
        }

        double CheckValueByRange(double val, double min, double max)
            => val <= min ? min : val >= max ? max : val;

        void SetThumbPosition(double thumbPosition, double progrssWidth, double val)
        {
            _previousVal = val;
            _thumb.TranslationX = thumbPosition;
            _progress.WidthRequest = progrssWidth;
        }
    }
}