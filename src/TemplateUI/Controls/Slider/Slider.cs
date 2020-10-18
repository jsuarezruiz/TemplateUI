using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class Slider : TemplatedView
    {
        const string ElementTrackBackground = "PART_TrackBackground";
        const string ElementTrack = "PART_Track";
        const string ElementProgress = "PART_Progress";
        const string ElementThumb = "PART_Thumb";

        View _trackBackground;
        BoxView _progress;
        ContentView _thumb;
        double ThumbHalfWidth => (_thumb?.Width ?? 0) / 2 ;

        double _previousPosition;

        public static readonly BindableProperty ThumbProperty = BindableProperty.Create(nameof(Thumb), typeof(View), typeof(Slider), null, propertyChanged: ThumbChanged);

        public View Thumb
        {
            get => (View)GetValue(ThumbProperty);
            set{ SetValue(ThumbProperty, value); }
        }
        private static void ThumbChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var slider = bindable as Slider;
            slider._thumb.Content = newValue as View;
        }

        public static readonly BindableProperty MinimumProperty =
            BindableProperty.Create(nameof(Minimum), typeof(double), typeof(Slider), 0.0d);

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly BindableProperty MaximumProperty =
            BindableProperty.Create(nameof(Maximum), typeof(double), typeof(Slider), 10.0d);

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(double), typeof(Slider), 0.0d,
                propertyChanged: OnValueChanged);

        static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Slider slider)
            {
                slider.ValueChanged?.Invoke(bindable, new ValueChangedEventArgs((double)newValue));
                slider.UpdateValue();
            }
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set { SetValue(ValueProperty, value); }
        }

        public static readonly BindableProperty TrackSizeProperty =
           BindableProperty.Create(nameof(TrackSize), typeof(double), typeof(Slider), 1.0d);

        public double TrackSize
        {
            get => (double)GetValue(TrackSizeProperty);
            set { SetValue(TrackSizeProperty, value); }
        }

        public static readonly BindableProperty MinimumTrackColorProperty =
            BindableProperty.Create(nameof(MinimumTrackColor), typeof(Color), typeof(Slider), Color.White);

        public Color MinimumTrackColor
        {
            get => (Color)GetValue(MinimumTrackColorProperty);
            set { SetValue(MinimumTrackColorProperty, value); }
        }

        public static readonly BindableProperty MaximumTrackColorProperty =
            BindableProperty.Create(nameof(MaximumTrackColor), typeof(Color), typeof(Slider), Color.Default);

        public Color MaximumTrackColor
        {
            get => (Color)GetValue(MaximumTrackColorProperty);
            set { SetValue(MaximumTrackColorProperty, value); }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

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
            UpdateValue();
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

        void UpdateValue()
        {
            var half = ThumbHalfWidth;
            var position = ConvertRangeValue(Value, Minimum, Maximum, half, _trackBackground.Width - half) - half;
            if (position <= 0)
                return;

            _progress.WidthRequest = position;
            _thumb.TranslationX = position;
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
                    double totalX = _previousPosition + e.TotalX;

                    if (Device.RuntimePlatform == Device.Android)
                        totalX += _thumb.TranslationX;
                    
                    var half = ThumbHalfWidth;
                    var position = totalX <= half ? half : totalX >= _trackBackground.Width - half ? _trackBackground.Width - half : totalX;
                    Value = ConvertRangeValue(position, half, _trackBackground.Width - half, Minimum, Maximum);
                    break;
                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    break;
            }
        }
        double ConvertRangeValue(double oldValue, double oldMin, double oldMax, double Min, double Max)
        {
            var relativeValue = (oldValue - oldMin) / (oldMax - oldMin);
            return Min + (Max - Min) * relativeValue;
        }
    }
}