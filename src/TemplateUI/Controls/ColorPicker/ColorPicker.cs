using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace TemplateUI.Controls
{
    public class ColorPicker : TemplatedView, INotifyPropertyChanged
    {
        const string ElementThumb = "PART_Thumb";
        const string ElementSaturationAndLightPicker = "PART_SaturationAndLight";
        const string ElementHuePicker = "PART_HuePicker";
        const string ElementHueThumb = "PART_HueThumb";

        const double ExtraLargeSize = 64;
        const double LargeSize = 48;
        const double MediumSize = 36;
        const double SmallSize = 24;
        const double ExtraSmallSize = 18;

        const double ExtraLargeFontSize = 24;
        const double LargeFontSize = 18;
        const double MediumFontSize = 12;
        const double SmallFontSize = 10;
        const double ExtraSmallFontSize = 8;

        Frame _thumb;
        Frame _hueThumb;
        OpacityGradientLayout _hslPicker;
        GradientLayout _rgbPicker;

        double _previousPositionX;
        double _previousPostionY;
        double _hueThumbPreviousPostionY;

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
                colorPicker.UpdateHSLPicker();
                colorPicker.CalculatePickedColor();
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
                colorPicker.UpdateRGBPicker();
                colorPicker.CalculatePickedColor();
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
        private double red = 0.0d;
        public double Red
        {
            get
            {
                return this.red;
            }
            set
            {
                this.red = value;
                OnPropertyChanged();
                ConvertToHsl();
            }
        }

        private double blue = 0.0d;
        public double Blue
        {
            get
            {
                return this.blue;
            }
            set
            {
                this.blue = value;
                OnPropertyChanged();
                ConvertToHsl();
            }
        }

        private double green = 0.0d;
        public double Green
        {
            get
            {
                return this.green;
            }
            set
            {
                this.green = value;
                OnPropertyChanged();
                ConvertToHsl();
            }
        }

        private double hue = 0.0d;
        public double Hue
        {
            get
            {
                return this.hue;
            }
            set
            {
                this.hue = value;
                OnPropertyChanged();
                ConvertToRgb();
            }
        }

        private double saturation = 1.0d;
        public double Saturation
        {
            get
            {
                return this.saturation;
            }
            set
            {
                this.saturation = value;
                OnPropertyChanged();
                ConvertToRgb();
            }
        }

        private double lightness = 0.5d;
        public double Lightness
        {
            get
            {
                return this.lightness;
            }
            set
            {
                this.lightness = value;
                OnPropertyChanged();
                ConvertToRgb();
            }
        }

        private double hexCode;
        public double HexCode
        {
            get
            {
                return this.hexCode;
            }
            set
            {
                this.hexCode = value;
                OnPropertyChanged();
            }
        }

        private void ConvertToHsl()
        {
            //Color color = Color.FromRgb(this.Red, this.Blue, this.Green);
            //this.Hue = color.Hue;
            //this.Saturation = color.Saturation;
            //this.Lightness = color.Luminosity;
        }

        private void ConvertToRgb()
        {
            //Color color = Color.FromRgb(this.Hue, this.Saturation, this.Lightness);
            //this.Red = color.R;
            //this.Blue = color.B;
            //this.Green = color.G;
        }

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

        private Color pickedColorWithoutSaturationAndLightness;
        public Color PickedColorWithoutSaturationAndLightness
        {
            get
            {
                return this.pickedColorWithoutSaturationAndLightness;
            }
            set
            {
                this.pickedColorWithoutSaturationAndLightness = value;
                OnPropertyChanged();
            }
        }

        public double HslPicklerHeight
        {
            get
            {
                return this._hslPicker.Height;
            }
        }

        public double HslPickerWidth
        {
            get
            {
                return this._hslPicker.Width;
            }
        }

        public double RgbPickerHeight
        {
            get
            {
                return this._rgbPicker.Height;
            }
        }

        public double RgbPickerWidth
        {
            get
            {
                return this._rgbPicker.Width;
            }
        }

        /**************************************************
         * Hooks
         **************************************************/
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _thumb = (Frame)GetTemplateChild(ElementThumb);
            _hslPicker = (OpacityGradientLayout)GetTemplateChild(ElementSaturationAndLightPicker);
            _hueThumb = (Frame)GetTemplateChild(ElementHueThumb);
            _rgbPicker = (GradientLayout)GetTemplateChild(ElementHuePicker);

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

            UpdateHSLPicker();
            UpdateRGBPicker();
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
                _thumb.GestureRecognizers.Add(panGestureRecognizer);

                var panGestureRecognizer2 = new PanGestureRecognizer();
                panGestureRecognizer2.PanUpdated += RgbPanGestureRecognizer_PanUpdated;
                _hueThumb.GestureRecognizers.Add(panGestureRecognizer2);
            }
            else
            {
                _thumb.GestureRecognizers.Clear();
                _hueThumb.GestureRecognizers.Clear();
            }
        }

        void PanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _previousPositionX = e.TotalX;
                    _previousPostionY = e.TotalY;

                    if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS)
                    {
                        _previousPositionX += _thumb.TranslationX;
                        _previousPostionY += _thumb.TranslationY;
                    }
                    break;
                case GestureStatus.Running:
                    double totalX = _previousPositionX + e.TotalX;
                    double totalY = _previousPostionY + e.TotalY;

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        totalX += _thumb.TranslationX;
                        totalY += _thumb.TranslationY;
                    }

                    var positionX = totalX < 0 ? 0 : totalX > _hslPicker.Width - _thumb.Width ? _hslPicker.Width - _thumb.Width : totalX;
                    ValueXhslThumb = positionX * MaximumX / _hslPicker.Width;
                    var positionY = totalY < 0 ? 0 : totalY > _hslPicker.Height - _thumb.Height ? _hslPicker.Height - _thumb.Height : totalY;
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
                    _hueThumbPreviousPostionY = e.TotalY;

                    if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS)
                    {
                        _hueThumbPreviousPostionY += _hueThumb.TranslationY;
                    }
                    break;
                case GestureStatus.Running:
                    double totalY = _hueThumbPreviousPostionY + e.TotalY;

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        totalY += _hueThumb.TranslationY;
                    }

                    var positionY = totalY < 0 ? 0 : totalY > _rgbPicker.Height - _hueThumb.Height ? _rgbPicker.Height - _hueThumb.Height : totalY;
                    ValueYrgbThumb = positionY * MaximumY / _rgbPicker.Height;
                    break;
                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    break;
            }
        }

        private void CalculatePickedColor()
        {
            // HUE
            double positionY = ValueYrgbThumb / MaximumX * _rgbPicker.Height;
            double hue = 360.0d * (positionY / _rgbPicker.Height);
            //this.Hue = hue;

            // LIGHTNESS
            double positionXhsl = ValueXhslThumb / MaximumX * _hslPicker.Width;
            double positionXhslInv = _hslPicker.Width - positionXhsl;
            double positionYhsl = ValueYhslThumb / MaximumY * _hslPicker.Height;
            double positionYhslInv = _hslPicker.Height - positionYhsl;
            double minimumLightness = 50.0d;
            double maximumLightness = minimumLightness + (positionXhslInv / _hslPicker.Width * 50.0d);
            double lightness = positionYhslInv / _hslPicker.Height * maximumLightness;
            //this.Lightness = lightness;

            // SATURATION
            double saturation = positionXhsl / _hslPicker.Width * 100;
            //this.Saturation = saturation;

            // RED
            //this.PickedColor = Color.FromHsla(1.0 / 360 * this.Hue, this.Saturation / 100, this.Lightness / 100);
            this.PickedColor = Color.FromHsla(1.0 / 360 * hue, saturation / 100, lightness / 100);
            this.PickedColorWithoutSaturationAndLightness = Color.FromHsla(1.0 / 360 * hue, 1.0d, 0.5d);
            //this.Red = PickedColor.R;
            //this.Blue = PickedColor.B;
            //this.Green = PickedColor.G;
        }

        void UpdateHSLPicker()
        {
            var positionX = ValueXhslThumb / MaximumX * _hslPicker.Width;
            var positionY = ValueYhslThumb / MaximumX * _hslPicker.Height;

            if (positionX < 0)
                positionX = 0;
            if (positionY < 0)
                positionY = 0;

            _thumb.TranslationX = positionX;
            _thumb.TranslationY = positionY;
        }

        void UpdateRGBPicker()
        {
            var positionX = ValueXrgbThumb / MaximumX * _rgbPicker.Width;
            var positionY = ValueYrgbThumb / MaximumX * _rgbPicker.Height;

            if (positionX < 0)
                positionX = 0;
            if (positionY < 0)
                positionY = 0;

            _hueThumb.TranslationX = positionX;
            _hueThumb.TranslationY = positionY;
        }
    }
}