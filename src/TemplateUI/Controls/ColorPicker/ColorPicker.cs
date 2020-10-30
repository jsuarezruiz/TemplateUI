using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TemplateUI.Helpers;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class ColorPicker : TemplatedView, INotifyPropertyChanged
    {
        const string ElementHSLThumb = "PART_Thumb";
        const string ElementHSLPicker = "PART_SaturationAndLight";
        const string ElementRGBPicker = "PART_HuePicker";
        const string ElementRGBThumb = "PART_HueThumb";
        const string ElementEntryRed = "PART_ENTRY_red";
        const string ElementEntryGreen = "PART_ENTRY_green";
        const string ElementEntryBlue = "PART_ENTRY_blue";
        const string ElementEntryHue = "PART_ENTRY_hue";
        const string ElementEntrySaturation = "PART_ENTRY_saturation";
        const string ElementEntryLuminosity = "PART_ENTRY_luminosity";

        Frame _rgbthumb;
        Frame _hueThumb;
        OpacityGradientLayout _hslPicker;
        GradientLayout _rgbPicker;
        Entry _entryRed;
        Entry _entryGreen;
        Entry _entryBlue;
        Entry _entryHue;
        Entry _entrySaturation;
        Entry _entryLuminosity;

        double _hslPreviousPositionX;
        double _hslPreviousPostionY;
        double _rgbThumbPreviousPostionY;

        /**************************************************
         * Bindable Properties
         **************************************************/
        public static readonly BindableProperty ValueXhslThumbProperty =
            BindableProperty.Create(nameof(ValueXhslThumb), typeof(double), typeof(ColorPicker), 0.0d,
                propertyChanged: OnHslValueChanged);

        public static readonly BindableProperty ValueYhslThumbProperty =
            BindableProperty.Create(nameof(ValueYhslThumb), typeof(double), typeof(ColorPicker), 0.0d,
                propertyChanged: OnHslValueChanged);

        static void OnHslValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ColorPicker colorPicker)
            {
                colorPicker.ValueChanged?.Invoke(bindable, new ValueChangedEventArgs((double)newValue));
                colorPicker.UpdateHSLThumb();
                colorPicker.CalculatePickedColorBasedOnThumbs();
            }
        }

        public double ValueXhslThumb
        {
            get => (double)GetValue(ValueXhslThumbProperty);
            set { SetValue(ValueXhslThumbProperty, value); }
        }

        public double ValueYhslThumb
        {
            get => (double)GetValue(ValueYhslThumbProperty);
            set { SetValue(ValueYhslThumbProperty, value); }
        }

        public static readonly BindableProperty ValueXrgbThumbProperty =
            BindableProperty.Create(nameof(ValueXrgbThumb), typeof(double), typeof(ColorPicker), 0.0d,
                propertyChanged: OnRgbValueChanged);

        public double ValueXrgbThumb
        {
            get => (double)GetValue(ValueXrgbThumbProperty);
            set { SetValue(ValueXrgbThumbProperty, value); }
        }

        public static readonly BindableProperty ValueYrgbThumbProperty =
            BindableProperty.Create(nameof(ValueYrgbThumb), typeof(double), typeof(ColorPicker), 0.0d,
                propertyChanged: OnRgbValueChanged);

        public double ValueYrgbThumb
        {
            get => (double)GetValue(ValueYrgbThumbProperty);
            set { SetValue(ValueYrgbThumbProperty, value); }
        }

        static void OnRgbValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ColorPicker colorPicker)
            {
                colorPicker.ValueChanged?.Invoke(bindable, new ValueChangedEventArgs((double)newValue));
                colorPicker.UpdateRGBThumb();
                colorPicker.CalculatePickedColorBasedOnThumbs();
            }
        }

        public static readonly BindableProperty MaximumXProperty =
            BindableProperty.Create(nameof(MaximumX), typeof(double), typeof(ColorPicker), 10.0d);

        public double MaximumX
        {
            get => (double)GetValue(MaximumXProperty);
            set { SetValue(MaximumXProperty, value); }
        }

        public static readonly BindableProperty MaximumYProperty =
            BindableProperty.Create(nameof(MaximumY), typeof(double), typeof(ColorPicker), 10.0d);

        public double MaximumY
        {
            get => (double)GetValue(MaximumYProperty);
            set { SetValue(MaximumYProperty, value); }
        }

        /**************************************************
         * Properties
         **************************************************/
        private Color pickedColor;
        public Color PickedColor
        {
            get
            {
                return this.pickedColor;
            }
            set
            {
                this.pickedColor = value;
                OnPropertyChanged();
            }
        }

        // This color has a fixed saturation and luminosity
        private Color pickedColorForHSL;
        public Color PickedColorForHSL
        {
            get
            {
                return this.pickedColorForHSL;
            }
            set
            {
                this.pickedColorForHSL = value;
                OnPropertyChanged();
            }
        }

        /**************************************************
         * Hooks
         **************************************************/
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _rgbthumb = (Frame)GetTemplateChild(ElementHSLThumb);
            _hslPicker = (OpacityGradientLayout)GetTemplateChild(ElementHSLPicker);
            _hueThumb = (Frame)GetTemplateChild(ElementRGBThumb);
            _rgbPicker = (GradientLayout)GetTemplateChild(ElementRGBPicker);
            _entryRed = (Entry)GetTemplateChild(ElementEntryRed);
            _entryGreen = (Entry)GetTemplateChild(ElementEntryGreen);
            _entryBlue = (Entry)GetTemplateChild(ElementEntryBlue);
            _entryHue = (Entry)GetTemplateChild(ElementEntryHue);
            _entrySaturation = (Entry)GetTemplateChild(ElementEntrySaturation);
            _entryLuminosity = (Entry)GetTemplateChild(ElementEntryLuminosity);

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

            UpdateHSLThumb();
            UpdateRGBThumb();
        }

        /**************************************************
         * Events
         **************************************************/
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        /**************************************************
         * Private Methods
         **************************************************/
        void UpdateIsEnabled()
        {
            if (IsEnabled)
            {
                var panGestureRecognizer = new PanGestureRecognizer();
                panGestureRecognizer.PanUpdated += PanGestureRecognizer_PanUpdated;
                _rgbthumb.GestureRecognizers.Add(panGestureRecognizer);

                var panGestureRecognizer2 = new PanGestureRecognizer();
                panGestureRecognizer2.PanUpdated += RgbPanGestureRecognizer_PanUpdated;
                _hueThumb.GestureRecognizers.Add(panGestureRecognizer2);

                _entryRed.Completed += _entryRed_Completed;
                _entryGreen.Completed += _entryGreen_Completed;
                _entryBlue.Completed += _entryBlue_Completed;
                _entryHue.Completed += _entryHue_Completed;
                _entrySaturation.Completed += _entrySaturation_Completed;
                _entryLuminosity.Completed += _entryLuminosity_Completed;
            }
            else
            {
                _rgbthumb.GestureRecognizers.Clear();
                _hueThumb.GestureRecognizers.Clear();
                _entryRed.Completed -= _entryRed_Completed;
                _entryGreen.Completed -= _entryGreen_Completed;
                _entryBlue.Completed -= _entryBlue_Completed;
                _entryHue.Completed -= _entryHue_Completed;
                _entrySaturation.Completed -= _entrySaturation_Completed;
                _entryLuminosity.Completed -= _entryLuminosity_Completed;
            }
        }

        private void _entryLuminosity_Completed(object sender, EventArgs e)
        {
            double newValue = 1.0d;
            bool parseSuccess = double.TryParse(_entryLuminosity.Text, out newValue);
            if (parseSuccess)
            {
                double maxLuminosity = ColorNumberHelper.MaxLuminosityFromSaturation(ColorNumberHelper.FromSourceToTargetSaturation(PickedColor.Saturation));
                double convertedNewValue = ColorNumberHelper.FromTargetToSourceLuminosity(newValue >= maxLuminosity ? maxLuminosity : newValue);
                Color newColor = Color.FromHsla(PickedColor.Hue, PickedColor.Saturation, convertedNewValue);
                PickedColor = newColor;
                PickedColorForHSL = Color.FromHsla(PickedColor.Hue, 1.0d, 0.5d);
                // move hsl thumb
                _rgbthumb.TranslationX = PickedColor.Saturation * _hslPicker.Width;
                _rgbthumb.TranslationY = _hslPicker.Height - (PickedColor.Luminosity * _hslPicker.Height + (_rgbthumb.TranslationX / _hslPicker.Width) * (_hslPicker.Height / 2));
            }
        }

        private void _entrySaturation_Completed(object sender, EventArgs e)
        {
            double newValue = 1.0d;
            bool parseSuccess = double.TryParse(_entrySaturation.Text, out newValue);
            if (parseSuccess)
            {
                double maxSaturation = ColorNumberHelper.MaxSaturationFromLuminosity(ColorNumberHelper.FromSourceToTargetLuminosity(PickedColor.Luminosity));
                double convertedNewValue = ColorNumberHelper.FromTargetToSourceSaturation(newValue >= maxSaturation ? maxSaturation : newValue);
                Color newColor = Color.FromHsla(PickedColor.Hue, convertedNewValue, PickedColor.Luminosity);
                PickedColor = newColor;
                PickedColorForHSL = Color.FromHsla(PickedColor.Hue, 1.0d, 0.5d);
                // move hsl thumb
                _rgbthumb.TranslationX = PickedColor.Saturation * _hslPicker.Width;
                _rgbthumb.TranslationY = _hslPicker.Height - (PickedColor.Luminosity * _hslPicker.Height + (_rgbthumb.TranslationX / _hslPicker.Width) * (_hslPicker.Height / 2));
            }
        }

        private void _entryHue_Completed(object sender, EventArgs e)
        {
            double newValue = 1.0d;
            bool parseSuccess = double.TryParse(_entryHue.Text, out newValue);
            if (parseSuccess)
            {
                double convertedNewValue = ColorNumberHelper.FromTargetToSourceHue(newValue);
                Color newColor = Color.FromHsla(convertedNewValue, PickedColor.Saturation, PickedColor.Luminosity);
                PickedColor = newColor;
                PickedColorForHSL = Color.FromHsla(PickedColor.Hue, 1.0d, 0.5d);
                // move rgb thumb
                _hueThumb.TranslationY = PickedColor.Hue * _rgbPicker.Height;
            }
        }

        private void _entryBlue_Completed(object sender, EventArgs e)
        {
            double newValue = 1.0d;
            bool parseSuccess = double.TryParse(_entryBlue.Text, out newValue);
            if (parseSuccess)
            {
                double convertedNewValue = ColorNumberHelper.FromTargetToSourceRGB(newValue);
                Color newColor = Color.FromRgb(PickedColor.R, PickedColor.G, convertedNewValue);
                PickedColor = newColor;
                PickedColorForHSL = Color.FromHsla(PickedColor.Hue, 1.0d, 0.5d);
                // move rgb & hsl thumb
                _hueThumb.TranslationY = PickedColor.Hue * _rgbPicker.Height;
                _rgbthumb.TranslationX = PickedColor.Saturation * _hslPicker.Width;
                _rgbthumb.TranslationY = _hslPicker.Height - (PickedColor.Luminosity * _hslPicker.Height + (_rgbthumb.TranslationX / _hslPicker.Width) * (_hslPicker.Height / 2));
            }
        }

        private void _entryGreen_Completed(object sender, EventArgs e)
        {
            double newValue = 1.0d;
            bool parseSuccess = double.TryParse(_entryGreen.Text, out newValue);
            if (parseSuccess)
            {
                double convertedNewValue = ColorNumberHelper.FromTargetToSourceRGB(newValue);
                Color newColor = Color.FromRgb(PickedColor.R, convertedNewValue, PickedColor.B);
                PickedColor = newColor;
                PickedColorForHSL = Color.FromHsla(PickedColor.Hue, 1.0d, 0.5d);
                // move rgb & hsl thumb
                _hueThumb.TranslationY = PickedColor.Hue * _rgbPicker.Height;
                _rgbthumb.TranslationX = PickedColor.Saturation * _hslPicker.Width;
                _rgbthumb.TranslationY = _hslPicker.Height - (PickedColor.Luminosity * _hslPicker.Height + (_rgbthumb.TranslationX / _hslPicker.Width) * (_hslPicker.Height / 2));
            }
        }

        private void _entryRed_Completed(object sender, EventArgs e)
        {
            double newValue = 1.0d;
            bool parseSuccess = double.TryParse(_entryRed.Text, out newValue);
            if (parseSuccess)
            {
                double convertedNewValue = ColorNumberHelper.FromTargetToSourceRGB(newValue);
                Color newColor = Color.FromRgb(convertedNewValue, PickedColor.G, PickedColor.B);
                PickedColor = newColor;
                PickedColorForHSL = Color.FromHsla(PickedColor.Hue, 1.0d, 0.5d);
                // move rgb thumb
                _hueThumb.TranslationY = PickedColor.Hue * _rgbPicker.Height;
                _rgbthumb.TranslationX = PickedColor.Saturation * _hslPicker.Width;
                _rgbthumb.TranslationY = _hslPicker.Height - (PickedColor.Luminosity * _hslPicker.Height + (_rgbthumb.TranslationX / _hslPicker.Width) * (_hslPicker.Height / 2));
            }
        }

        void PanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _hslPreviousPositionX = e.TotalX;
                    _hslPreviousPostionY = e.TotalY;

                    if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS)
                    {
                        _hslPreviousPositionX += _rgbthumb.TranslationX;
                        _hslPreviousPostionY += _rgbthumb.TranslationY;
                    }
                    break;
                case GestureStatus.Running:
                    double totalX = _hslPreviousPositionX + e.TotalX;
                    double totalY = _hslPreviousPostionY + e.TotalY;

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        totalX += _rgbthumb.TranslationX;
                        totalY += _rgbthumb.TranslationY;
                    }

                    // Keep the position of thumbs in check
                    var positionX = totalX < 0 ? 0 : totalX > _hslPicker.Width ? _hslPicker.Width : totalX;
                    ValueXhslThumb = positionX * MaximumX / _hslPicker.Width;
                    var positionY = totalY < 0 ? 0 : totalY > _hslPicker.Height ? _hslPicker.Height : totalY;
                    ValueYhslThumb = positionY * MaximumY / _hslPicker.Height;
                    break;
                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    break;
            }
        }

        void RgbPanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _rgbThumbPreviousPostionY = e.TotalY;

                    if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS)
                    {
                        _rgbThumbPreviousPostionY += _hueThumb.TranslationY;
                    }
                    break;
                case GestureStatus.Running:
                    double totalY = _rgbThumbPreviousPostionY + e.TotalY;

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        totalY += _hueThumb.TranslationY;
                    }

                    var positionY = totalY < 0 ? 0 : totalY > _rgbPicker.Height ? _rgbPicker.Height : totalY;
                    ValueYrgbThumb = positionY * MaximumY / _rgbPicker.Height;
                    break;
                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    break;
            }
        }

        private void CalculatePickedColorBasedOnThumbs()
        {
            // HUE (0.0d to 359.0d)
            double hue = 359.0d * (_hueThumb.TranslationY / _rgbPicker.Height);

            // LUMINOSITY (0.0d to 100.0d)
            double positionXhslInv = _hslPicker.Width - _rgbthumb.TranslationX;
            double positionYhslInv = _hslPicker.Height - _rgbthumb.TranslationY;
            double minimumLuminosity = 50.0d;
            double maximumLuminosity = minimumLuminosity + (positionXhslInv / _hslPicker.Width * 50.0d);
            double luminosity = positionYhslInv / _hslPicker.Height * maximumLuminosity;

            // SATURATION (0.0d to 100.0d)
            double saturation = _rgbthumb.TranslationX / _hslPicker.Width * 100;

            // SETTING PICKED COLOR
            double technicalHue = ColorNumberHelper.FromTargetToSourceHue(hue);
            double technicalSaturation = ColorNumberHelper.FromTargetToSourceSaturation(saturation);
            double technicalLuminosity = ColorNumberHelper.FromTargetToSourceLuminosity(luminosity);
            this.PickedColor = Color.FromHsla(technicalHue, technicalSaturation, technicalLuminosity);
            this.PickedColorForHSL = Color.FromHsla(PickedColor.Hue, 1.0d, 0.5d);
        }

        void UpdateHSLThumb()
        {
            var positionX = ValueXhslThumb / MaximumX * _hslPicker.Width;
            var positionY = ValueYhslThumb / MaximumY * _hslPicker.Height;

            if (positionX < 0)
                positionX = 0;
            if (positionY < 0)
                positionY = 0;

            _rgbthumb.TranslationX = positionX;
            _rgbthumb.TranslationY = positionY;
        }

        void UpdateRGBThumb()
        {
            var positionX = ValueXrgbThumb / MaximumX * _rgbPicker.Width;
            var positionY = ValueYrgbThumb / MaximumY * _rgbPicker.Height;

            if (positionX < 0)
                positionX = 0;
            if (positionY < 0)
                positionY = 0;

            _hueThumb.TranslationX = positionX;
            _hueThumb.TranslationY = positionY;
        }
    }
}