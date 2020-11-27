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
        const string ElementRadialPicker = "PART_RadialPicker";
        const string ElementRadialPickerThumb = "PART_RadialPickerThumb";
        const string ElementBrightnessPicker = "PART_BrightnessPicker";
        const string ElementBrightnessPickerThumb = "PART_BrightnessThumb";
        const string ElementEntryRed = "PART_ENTRY_red";
        const string ElementEntryGreen = "PART_ENTRY_green";
        const string ElementEntryBlue = "PART_ENTRY_blue";
        const string ElementEntryHue = "PART_ENTRY_hue";
        const string ElementEntrySaturation = "PART_ENTRY_saturation";
        const string ElementEntryLuminosity = "PART_ENTRY_luminosity";

        Frame _rgbthumb;
        Frame _hueThumb;
        Frame _radialPickerThumb;
        Frame _brightnessPickerThumb;
        OpacityGradientLayout _hslPicker;
        GradientLayout _rgbPicker;
        RadialPicker _radialPicker;
        GradientLayout _brightnessPicker;
        Entry _entryRed;
        Entry _entryGreen;
        Entry _entryBlue;
        Entry _entryHue;
        Entry _entrySaturation;
        Entry _entryLuminosity;

        double _hslPreviousPositionX;
        double _hslPreviousPostionY;
        double _rgbThumbPreviousPostionY;
        double _radialPickerPreviousPositionX;
        double _radialPickerPreviousPositionY;
        double _brightnessThumbPreviousPostionY;

        /**************************************************
         * Bindable Properties
         **************************************************/
        /**********
         * Shared
         **********/
        public static readonly BindableProperty PickedColorProperty =
            BindableProperty.Create(nameof(PickedColor), typeof(Color), typeof(ColorPicker), Color.White,
                propertyChanged: OnPickedColorChanged);

        static void OnPickedColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ColorPicker colorPicker)
            {
                colorPicker.UpdateRGBThumbOnColorChanged();
                colorPicker.UpdateHSLThumbOnColorChanged();
                colorPicker.UpdateRadialThumbOnColorChanged();
                colorPicker.UpdateBrightnessThumbOnColorChanged();
            }
        }

        public Color PickedColor
        {
            get => (Color)GetValue(PickedColorProperty);
            set { SetValue(PickedColorProperty, value); }
        }
        /**********
         * SL Picker
         **********/
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

        /**********
         * Hue Picker
         **********/
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

        /**********
         * RadialPicker
         **********/
        public static readonly BindableProperty ValueXRadialPickerThumbProperty =
            BindableProperty.Create(nameof(ValueXRadialPickerThumb), typeof(double), typeof(ColorPicker), 0.0d,
                propertyChanged: OnRadialPickerValueChanged);

        public static readonly BindableProperty ValueYRadialPickerThumbProperty =
            BindableProperty.Create(nameof(ValueYRadialPickerThumb), typeof(double), typeof(ColorPicker), 0.0d,
                propertyChanged: OnRadialPickerValueChanged);

        static void OnRadialPickerValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ColorPicker colorPicker)
            {
                colorPicker.ValueChanged?.Invoke(bindable, new ValueChangedEventArgs((double)newValue));
                colorPicker.UpdateRadialThumb();
                colorPicker.CalculatePickedColorBasedOnRadiantThumbs();
            }
        }

        public double ValueXRadialPickerThumb
        {
            get => (double)GetValue(ValueXRadialPickerThumbProperty);
            set { SetValue(ValueXRadialPickerThumbProperty, value); }
        }

        public double ValueYRadialPickerThumb
        {
            get => (double)GetValue(ValueYRadialPickerThumbProperty);
            set { SetValue(ValueYRadialPickerThumbProperty, value); }
        }

        /**********
         * Brightness Picker
         **********/
        public static readonly BindableProperty ValueXbrightnessThumbProperty =
            BindableProperty.Create(nameof(ValueXbrightnessThumb), typeof(double), typeof(ColorPicker), 0.0d,
                propertyChanged: OnBrightnessValueChanged);

        public double ValueXbrightnessThumb
        {
            get => (double)GetValue(ValueXbrightnessThumbProperty);
            set { SetValue(ValueXbrightnessThumbProperty, value); }
        }

        public static readonly BindableProperty ValueYbrightnessThumbProperty =
            BindableProperty.Create(nameof(ValueYbrightnessThumb), typeof(double), typeof(ColorPicker), 0.0d,
                propertyChanged: OnBrightnessValueChanged);

        public double ValueYbrightnessThumb
        {
            get => (double)GetValue(ValueYbrightnessThumbProperty);
            set { SetValue(ValueYbrightnessThumbProperty, value); }
        }

        static void OnBrightnessValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ColorPicker colorPicker)
            {
                colorPicker.ValueChanged?.Invoke(bindable, new ValueChangedEventArgs((double)newValue));
                colorPicker.UpdateBrightnessThumb();
                colorPicker.CalculatePickedColorBasedOnRadiantThumbs();
            }
        }

        /**********
         * Helping Properties
         **********/
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
        public string ColorsList
        {
            get
            {
                string colorsList = "";
                // 100 different Hues
                for (double d = 0.00; d <= 1.0; d = d + 0.01d)
                {
                    Color color = Color.FromHsla(d, 1.0, 0.5);
                    colorsList = colorsList + color.ToHex() + ",";
                }
                return colorsList;
            }
        }

        public string ColorsListBrightness
        {
            get
            {
                string brightnessList;
                Color brightness100 = Color.FromRgba(0, 0, 0, 0);
                Color brightness0 = Color.FromRgba(0, 0, 0, 255);
                brightnessList = $"{ brightness100.ToHex() },{ brightness0.ToHex() }";
                return brightnessList;
            }
        }

        // TODO entfernen. wird nicht mehr gebraucht
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
            _radialPicker = (RadialPicker)GetTemplateChild(ElementRadialPicker);
            _radialPickerThumb = (Frame)GetTemplateChild(ElementRadialPickerThumb);
            _brightnessPicker = (GradientLayout)GetTemplateChild(ElementBrightnessPicker);
            _brightnessPickerThumb = (Frame)GetTemplateChild(ElementBrightnessPickerThumb);
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
            UpdateRadialThumb();
            UpdateBrightnessThumb();
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
                // registering event handlers
                var panGestureRecognizer = new PanGestureRecognizer();
                panGestureRecognizer.PanUpdated += PanGestureRecognizer_PanUpdated;
                _rgbthumb.GestureRecognizers.Add(panGestureRecognizer);

                var panGestureRecognizer2 = new PanGestureRecognizer();
                panGestureRecognizer2.PanUpdated += RgbPanGestureRecognizer_PanUpdated;
                _hueThumb.GestureRecognizers.Add(panGestureRecognizer2);

                var panGestureRecognizer3 = new PanGestureRecognizer();
                panGestureRecognizer3.PanUpdated += RadialPickerThumbPanGestureRecognizer_PanUpdated;
                _radialPickerThumb.GestureRecognizers.Add(panGestureRecognizer3);

                var panGestureRecognizer4 = new PanGestureRecognizer();
                panGestureRecognizer4.PanUpdated += BrightnessPanGestureRecognizer_PanUpdated;
                _brightnessPickerThumb.GestureRecognizers.Add(panGestureRecognizer4);

                _entryRed.Completed += _entryRed_Completed;
                _entryGreen.Completed += _entryGreen_Completed;
                _entryBlue.Completed += _entryBlue_Completed;
                _entryHue.Completed += _entryHue_Completed;
                _entrySaturation.Completed += _entrySaturation_Completed;
                _entryLuminosity.Completed += _entryLuminosity_Completed;

                // default thumb positions and default values
                this._radialPickerThumb.TranslationX = this._radialPicker.WidthRequest / 2;
                this._radialPickerThumb.TranslationY = this._radialPicker.HeightRequest / 2;
            }
            else
            {
                // unregistering event handlers
                _rgbthumb.GestureRecognizers.Clear();
                _hueThumb.GestureRecognizers.Clear();
                _radialPickerThumb.GestureRecognizers.Clear();
                _brightnessPickerThumb.GestureRecognizers.Clear();
                _entryRed.Completed -= _entryRed_Completed;
                _entryGreen.Completed -= _entryGreen_Completed;
                _entryBlue.Completed -= _entryBlue_Completed;
                _entryHue.Completed -= _entryHue_Completed;
                _entrySaturation.Completed -= _entrySaturation_Completed;
                _entryLuminosity.Completed -= _entryLuminosity_Completed;
            }
        }

        /****************************************
         * Human Interaction Event Handlers
         ****************************************/
        // Human Interaction: Luminosity
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
            }
        }

        // Human Interaction: Saturation
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
            }
        }

        // Human Interaction Hue
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
            }
        }

        // Human Interaction Blue
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
            }
        }

        // Human Interaction Green
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
            }
        }

        // Human Interaction Red
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
            }
        }

        // Human Interaction: Squared Picker Hue
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

        // Human Interaction: Squared Picker Luminosity
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

        // Human Interaction: Radial Picker Hue
        void RadialPickerThumbPanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _radialPickerPreviousPositionX = e.TotalX;
                    _radialPickerPreviousPositionY = e.TotalY;

                    if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS)
                    {
                        _radialPickerPreviousPositionX += _radialPickerThumb.TranslationX;
                        _radialPickerPreviousPositionY += _radialPickerThumb.TranslationY;
                    }
                    break;
                case GestureStatus.Running:
                    double totalX = _radialPickerPreviousPositionX + e.TotalX;
                    double totalY = _radialPickerPreviousPositionY + e.TotalY;

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        totalX += _radialPickerThumb.TranslationX;
                        totalY += _radialPickerThumb.TranslationY;
                    }

                    // Keep the position of thumbs in check
                    var positionX = totalX < 0 ? 0 : totalX > _radialPicker.Width ? _radialPicker.Width : totalX;
                    ValueXRadialPickerThumb = positionX * MaximumX / _radialPicker.Width;
                    var positionY = totalY < 0 ? 0 : totalY > _radialPicker.Height ? _radialPicker.Height : totalY;
                    ValueYRadialPickerThumb = positionY * MaximumY / _radialPicker.Height;
                    break;
                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    break;
            }
        }

        // Human Interaction: Radial Picker Luminosity
        void BrightnessPanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _brightnessThumbPreviousPostionY = e.TotalY;

                    if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS)
                    {
                        _brightnessThumbPreviousPostionY += _brightnessPickerThumb.TranslationY;
                    }
                    break;
                case GestureStatus.Running:
                    double totalY = _brightnessThumbPreviousPostionY + e.TotalY;

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        totalY += _brightnessPickerThumb.TranslationY;
                    }

                    var positionY = totalY < 0 ? 0 : totalY > _brightnessPicker.Height ? _brightnessPicker.Height : totalY;
                    ValueYbrightnessThumb = positionY * MaximumY / _brightnessPicker.Height;
                    break;
                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    break;
            }
        }

        /****************************************
         * Move Thumbs on Human Interaction or Selected Color changed
         ****************************************/
        // Move Light & Saturation Thumb on Human Interaction
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

        // Move Hue Thumb on Human Interaction
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

        // Move Radial Picker Hue Thumb on Human Interaction
        void UpdateRadialThumb()
        {
            var centerPointX = this._radialPicker.Width / 2;
            var centerPointY = this._radialPicker.Height / 2;
            var radius = this._radialPicker.Width / 2;

            var positionX = ValueXRadialPickerThumb / MaximumX * _radialPicker.Width;
            var positionY = ValueYRadialPickerThumb / MaximumY * _radialPicker.Height;

            var positionXRelativeToCenter = Math.Abs(centerPointX - positionX);
            var positionYRelativeToCenter = Math.Abs(centerPointY - positionY);
            var hypothenuse = Math.Sqrt(Math.Pow(positionXRelativeToCenter, 2.0) + Math.Pow(positionYRelativeToCenter, 2.0));

            // outside of circle
            if (hypothenuse > radius)
            {
                return;
            }

            if (positionX < 0)
                positionX = 0;
            if (positionY < 0)
                positionY = 0;

            _radialPickerThumb.TranslationX = positionX;
            _radialPickerThumb.TranslationY = positionY;
        }

        // Move Radial Picker Luminosity Thumb on Human Interaction
        void UpdateBrightnessThumb()
        {
            var positionX = ValueXbrightnessThumb / MaximumX * _brightnessPicker.Width;
            var positionY = ValueYbrightnessThumb / MaximumY * _brightnessPicker.Height;

            if (positionX < 0)
                positionX = 0;
            if (positionY < 0)
                positionY = 0;

            _brightnessPickerThumb.TranslationX = positionX;
            _brightnessPickerThumb.TranslationY = positionY;
        }

        // Move Light & Saturation Thumb on SelectedColor changing
        void UpdateHSLThumbOnColorChanged()
        {
            _rgbthumb.TranslationX = PickedColor.Saturation * _hslPicker.Width;
            _rgbthumb.TranslationY = _hslPicker.Height - (PickedColor.Luminosity * _hslPicker.Height + (_rgbthumb.TranslationX / _hslPicker.Width) * (_hslPicker.Height / 2));
        }

        // Move Hue Thumb on SelectedColor changing
        void UpdateRGBThumbOnColorChanged()
        {
            _hueThumb.TranslationY = PickedColor.Hue * _rgbPicker.Height;
        }

        // Move Radial Picker Hue Thumb on SelectedColor changing
        void UpdateRadialThumbOnColorChanged()
        {
            var centerPointX = this._radialPicker.Width / 2;
            var centerPointY = this._radialPicker.Height / 2;
            double radius = this._radialPicker.Width / 2;
            double hypothenuse = radius * PickedColor.Saturation;
            _radialPickerThumb.TranslationX = centerPointX + hypothenuse * Math.Sin(ColorNumberHelper.ConvertDegreesToRadians(PickedColor.Hue * 360.0));
            _radialPickerThumb.TranslationY = centerPointY - hypothenuse * Math.Cos(ColorNumberHelper.ConvertDegreesToRadians(PickedColor.Hue * 360.0));
        }

        // Move Radial Picker Luminosity Thumb on SelectedColor changing
        void UpdateBrightnessThumbOnColorChanged()
        {
            _brightnessPickerThumb.TranslationY = _radialPicker.Height - ((PickedColor.Luminosity / 1.0d) * _brightnessPicker.Height);
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

        private void CalculatePickedColorBasedOnRadiantThumbs()
        {
            // HUE (0.0d to 359.0d)
            var positionX = _radialPickerThumb.TranslationX;
            var positionY = _radialPickerThumb.TranslationY;
            var centerPointX = this._radialPicker.Width / 2;
            var centerPointY = this._radialPicker.Height / 2;
            var positionXRelativeToCenter = positionX - centerPointX;
            var positionYRelativeToCenter = centerPointY - positionY;
            double hue = calculateAngleClockwise(positionXRelativeToCenter, positionYRelativeToCenter);
            Console.WriteLine($"Hue { hue }");

            // LUMINOSITY (0.0d to 100.0d)
            double luminosity = (_brightnessPicker.Height - _brightnessPickerThumb.TranslationY) / _brightnessPicker.Height * 100.0;

            // SATURATION (0.0d to 100.0d)
            var radius = this._radialPicker.Width / 2;
            var hypothenuse = Math.Sqrt(Math.Pow(positionXRelativeToCenter, 2.0) + Math.Pow(positionYRelativeToCenter, 2.0));
            double saturation = hypothenuse / radius * 100.0;

            // SETTING PICKED COLOR
            double technicalHue = ColorNumberHelper.FromTargetToSourceHue(hue);
            double technicalSaturation = ColorNumberHelper.FromTargetToSourceSaturation(saturation);
            double technicalLuminosity = ColorNumberHelper.FromTargetToSourceLuminosity(luminosity);
            this.PickedColor = Color.FromHsla(technicalHue, technicalSaturation, technicalLuminosity);
            this.PickedColorForHSL = Color.FromHsla(PickedColor.Hue, 1.0d, 0.5d);
        }

        private double calculateAngleClockwise(double xDiffCenter, double yDiffCenter)
        {
            double angle = Math.Atan2(xDiffCenter, yDiffCenter) * 180.0 / Math.PI;
            if (angle < 0)
            {
                angle = 360.0 + angle;
            }
            return angle;
        }
    }
}