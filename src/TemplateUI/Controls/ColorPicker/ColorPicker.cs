﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using TemplateUI.Helpers;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class ColorPicker : TemplatedView, INotifyPropertyChanged
    {
        private enum EntryName
        {
            RED,
            GREEN,
            BLUE,
            HUE,
            SATURATION,
            LUMINOSITY
        }

        const string ElementRectSatLumPicker = "PART_Rect_SatLumPicker";
        const string ElementRectSatLumPickerThumb = "PART_Rect_SatLumPickerThumb";
        const string ElementRectHuePicker = "PART_Rect_HuePicker";
        const string ElementRectHuePickerThumb = "PART_Rect_HuePickerThumb";
        const string ElementRadialSatHuePicker = "PART_Radial_SatHuePicker";
        const string ElementRadialSatHuePickerThumb = "PART_Radial_SatHuePickerThumb";
        const string ElementRadialLumPicker = "PART_Radial_LumPicker";
        const string ElementRadialLumPickerThumb = "PART_Radial_LumPickerThumb";
        const string ElementEntryRed = "PART_ENTRY_red";
        const string ElementEntryGreen = "PART_ENTRY_green";
        const string ElementEntryBlue = "PART_ENTRY_blue";
        const string ElementEntryHue = "PART_ENTRY_hue";
        const string ElementEntrySaturation = "PART_ENTRY_saturation";
        const string ElementEntryLuminosity = "PART_ENTRY_luminosity";

        Frame _rectSatLumPickerThumb;
        Frame _rectHuePickerThumb;
        Frame _radialSatHuePickerThumb;
        Frame _radialLumPickerThumb;
        OpacityGradientLayout _rectSatLumPicker;
        GradientLayout _rectHuePicker;
        RadialPicker _radialSatHuePicker;
        GradientLayout _radialLumPicker;
        Entry _entryRed;
        Entry _entryGreen;
        Entry _entryBlue;
        Entry _entryHue;
        Entry _entrySaturation;
        Entry _entryLuminosity;

        double _rectSatLumPickerThumbPreviousPositionX;
        double _rectSatLumPickerThumbPreviousPostionY;
        double _rectHuePickerThumbPreviousPostionY;
        double _radialSatHuePickerThumbPreviousPositionX;
        double _radialSatHuePickerThumbPreviousPositionY;
        double _radialLumPickerThumbPreviousPositionY;

        private Dictionary<EntryName, bool> isUserInput = new Dictionary<EntryName, bool>();

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
                colorPicker.UpdateRectHuePickerThumbOnColorChanged();
                colorPicker.UpdateRectSatLumPickerThumbOnColorChanged();
                colorPicker.UpdateRadialSatHuePickerThumbOnColorChanged();
                colorPicker.UpdateRadialLumPickerOnColorChanged();
                colorPicker.UpdateRadialLumPickerThumbOnColorChanged();
            }
        }

        public Color PickedColor
        {
            get => (Color)GetValue(PickedColorProperty);
            set { SetValue(PickedColorProperty, value); }
        }
        /**********
         * RECT Saturation Luminosity Picker
         **********/
        public static readonly BindableProperty ValueXRectSatLumPickerThumbProperty =
            BindableProperty.Create(nameof(ValueXRectSatLumPickerThumb), typeof(double), typeof(ColorPicker), 0.0d,
                propertyChanged: OnRectSatLumValueChanged);

        public static readonly BindableProperty ValueYRectSatLumPickerThumbProperty =
            BindableProperty.Create(nameof(ValueYRectSatLumPickerThumb), typeof(double), typeof(ColorPicker), 0.0d,
                propertyChanged: OnRectSatLumValueChanged);

        static void OnRectSatLumValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ColorPicker colorPicker)
            {
                colorPicker.ValueChanged?.Invoke(bindable, new ValueChangedEventArgs((double)newValue));
                colorPicker.UpdateRectSatLumPickerThumb();
                colorPicker.CalculatePickedColorBasedOnRectThumbs();
            }
        }

        public double ValueXRectSatLumPickerThumb
        {
            get => (double)GetValue(ValueXRectSatLumPickerThumbProperty);
            set { SetValue(ValueXRectSatLumPickerThumbProperty, value); }
        }

        public double ValueYRectSatLumPickerThumb
        {
            get => (double)GetValue(ValueYRectSatLumPickerThumbProperty);
            set { SetValue(ValueYRectSatLumPickerThumbProperty, value); }
        }

        /**********
         * RECT Hue Picker
         **********/
        public static readonly BindableProperty ValueXRectHuePickerThumbProperty =
            BindableProperty.Create(nameof(ValueXRectHuePickerThumb), typeof(double), typeof(ColorPicker), 0.0d,
                propertyChanged: OnRectHueValueChanged);

        public double ValueXRectHuePickerThumb
        {
            get => (double)GetValue(ValueXRectHuePickerThumbProperty);
            set { SetValue(ValueXRectHuePickerThumbProperty, value); }
        }

        public static readonly BindableProperty ValueYRectHuePickerThumbProperty =
            BindableProperty.Create(nameof(ValueRectHuePickerThumb), typeof(double), typeof(ColorPicker), 0.0d,
                propertyChanged: OnRectHueValueChanged);

        public double ValueRectHuePickerThumb
        {
            get => (double)GetValue(ValueYRectHuePickerThumbProperty);
            set { SetValue(ValueYRectHuePickerThumbProperty, value); }
        }

        static void OnRectHueValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ColorPicker colorPicker)
            {
                colorPicker.ValueChanged?.Invoke(bindable, new ValueChangedEventArgs((double)newValue));
                colorPicker.UpdateRectHuePickerThumb();
                colorPicker.CalculatePickedColorBasedOnRectThumbs();
            }
        }

        /**********
         * RADIAL Saturation Hue Picker
         **********/
        public static readonly BindableProperty ValueXRadialSatHuePickerThumbProperty =
            BindableProperty.Create(nameof(ValueXRadialSatHuePickerThumb), typeof(double), typeof(ColorPicker), 0.0d,
                propertyChanged: OnRadialPickerSatHueValueChanged);

        public static readonly BindableProperty ValueYRadialSatHuePickerThumbProperty =
            BindableProperty.Create(nameof(ValueYRadialSatHuePickerThumb), typeof(double), typeof(ColorPicker), 0.0d,
                propertyChanged: OnRadialPickerSatHueValueChanged);

        static void OnRadialPickerSatHueValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ColorPicker colorPicker)
            {
                colorPicker.ValueChanged?.Invoke(bindable, new ValueChangedEventArgs((double)newValue));
                colorPicker.UpdateRadialSatHuePickerThumb();
                colorPicker.CalculatePickedColorBasedOnRadialThumbs();
            }
        }

        public double ValueXRadialSatHuePickerThumb
        {
            get => (double)GetValue(ValueXRadialSatHuePickerThumbProperty);
            set { SetValue(ValueXRadialSatHuePickerThumbProperty, value); }
        }

        public double ValueYRadialSatHuePickerThumb
        {
            get => (double)GetValue(ValueYRadialSatHuePickerThumbProperty);
            set { SetValue(ValueYRadialSatHuePickerThumbProperty, value); }
        }

        /**********
         * RADIAL Luminosity Picker
         **********/
        public static readonly BindableProperty ValueXRadialLumPickerThumbProperty =
            BindableProperty.Create(nameof(ValueXRadialLumPickerThumb), typeof(double), typeof(ColorPicker), 0.0d,
                propertyChanged: OnRadialPickerLumValueChanged);

        public double ValueXRadialLumPickerThumb
        {
            get => (double)GetValue(ValueXRadialLumPickerThumbProperty);
            set { SetValue(ValueXRadialLumPickerThumbProperty, value); }
        }

        public static readonly BindableProperty ValueYRadialLumPickerThumbProperty =
            BindableProperty.Create(nameof(ValueYRadialLumThumb), typeof(double), typeof(ColorPicker), 0.0d,
                propertyChanged: OnRadialPickerLumValueChanged);

        public double ValueYRadialLumThumb
        {
            get => (double)GetValue(ValueYRadialLumPickerThumbProperty);
            set { SetValue(ValueYRadialLumPickerThumbProperty, value); }
        }

        static void OnRadialPickerLumValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ColorPicker colorPicker)
            {
                colorPicker.ValueChanged?.Invoke(bindable, new ValueChangedEventArgs((double)newValue));
                colorPicker.UpdateRadialLumPickerThumb();
                colorPicker.CalculatePickedColorBasedOnRadialThumbs();
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

        private string colorsListBrightness = $"{ Color.FromRgba(0, 0, 0, 0).ToHex() },{ Color.FromRgba(0, 0, 0, 255).ToHex() }";
        public string ColorsListBrightness
        {
            get
            {
                return this.colorsListBrightness;
            }
            set
            {
                this.colorsListBrightness = value;
                this.OnPropertyChanged();
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

            _rectSatLumPickerThumb = (Frame)GetTemplateChild(ElementRectSatLumPickerThumb);
            _rectSatLumPicker = (OpacityGradientLayout)GetTemplateChild(ElementRectSatLumPicker);
            _rectHuePickerThumb = (Frame)GetTemplateChild(ElementRectHuePickerThumb);
            _rectHuePicker = (GradientLayout)GetTemplateChild(ElementRectHuePicker);
            _radialSatHuePicker = (RadialPicker)GetTemplateChild(ElementRadialSatHuePicker);
            _radialSatHuePickerThumb = (Frame)GetTemplateChild(ElementRadialSatHuePickerThumb);
            _radialLumPicker = (GradientLayout)GetTemplateChild(ElementRadialLumPicker);
            _radialLumPickerThumb = (Frame)GetTemplateChild(ElementRadialLumPickerThumb);
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

            UpdateRectSatLumPickerThumb();
            UpdateRectHuePickerThumb();
            UpdateRadialSatHuePickerThumb();
            UpdateRadialLumPickerThumb();
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
                _rectSatLumPickerThumb.GestureRecognizers.Add(panGestureRecognizer);

                var panGestureRecognizer2 = new PanGestureRecognizer();
                panGestureRecognizer2.PanUpdated += RgbPanGestureRecognizer_PanUpdated;
                _rectHuePickerThumb.GestureRecognizers.Add(panGestureRecognizer2);

                var panGestureRecognizer3 = new PanGestureRecognizer();
                panGestureRecognizer3.PanUpdated += RadialPickerThumbPanGestureRecognizer_PanUpdated;
                _radialSatHuePickerThumb.GestureRecognizers.Add(panGestureRecognizer3);

                var panGestureRecognizer4 = new PanGestureRecognizer();
                panGestureRecognizer4.PanUpdated += BrightnessPanGestureRecognizer_PanUpdated;
                _radialLumPickerThumb.GestureRecognizers.Add(panGestureRecognizer4);

                // R
                _entryRed.Completed += _entryRed_Completed;
                _entryRed.TextChanged += _entryRed_Changed;
                _entryRed.Focused += _entryRed_Focused;
                _entryRed.Unfocused += _entryRed_Unfocused;
                // G
                _entryGreen.Completed += _entryGreen_Completed;
                _entryGreen.TextChanged += _entryGreen_Changed;
                _entryGreen.Focused += _entryGreen_Focused;
                _entryGreen.Unfocused += _entryGreen_Unfocused;
                // B
                _entryBlue.Completed += _entryBlue_Completed;
                _entryBlue.TextChanged += _entryBlue_Changed;
                _entryBlue.Focused += _entryBlue_Focused;
                _entryBlue.Unfocused += _entryBlue_Unfocused;
                // HUE
                _entryHue.Completed += _entryHue_Completed;
                _entryHue.TextChanged += _entryHue_Changed;
                _entryHue.Focused += _entryHue_Focused;
                _entryHue.Unfocused += _entryHue_Unfocused;
                // SATURATION
                _entrySaturation.Completed += _entrySaturation_Completed;
                _entrySaturation.TextChanged += _entrySaturation_Changed;
                _entrySaturation.Focused += _entrySaturation_Focused;
                _entrySaturation.Unfocused += _entrySaturation_Unfocused;
                // LUMINOSITY
                _entryLuminosity.Completed += _entryLuminosity_Completed;
                _entryLuminosity.TextChanged += _entryLuminosity_Changed;
                _entryLuminosity.Focused += _entryLuminosity_Focused;
                _entryLuminosity.Unfocused += _entryLuminosity_Unfocused;

                // default thumb positions and default values
                this._radialSatHuePickerThumb.TranslationX = this._radialSatHuePicker.WidthRequest / 2;
                this._radialSatHuePickerThumb.TranslationY = this._radialSatHuePicker.HeightRequest / 2;
            }
            else
            {
                // unregistering event handlers
                _rectSatLumPickerThumb.GestureRecognizers.Clear();
                _rectHuePickerThumb.GestureRecognizers.Clear();
                _radialSatHuePickerThumb.GestureRecognizers.Clear();
                _radialLumPickerThumb.GestureRecognizers.Clear();
                _entryRed.Completed -= _entryRed_Completed;
                _entryRed.TextChanged -= _entryRed_Changed;
                _entryGreen.Completed -= _entryGreen_Completed;
                _entryGreen.TextChanged -= _entryGreen_Changed;
                _entryBlue.Completed -= _entryBlue_Completed;
                _entryBlue.TextChanged -= _entryBlue_Changed;
                _entryHue.Completed -= _entryHue_Completed;
                _entryHue.TextChanged -= _entryHue_Changed;
                _entrySaturation.Completed -= _entrySaturation_Completed;
                _entrySaturation.TextChanged -= _entrySaturation_Changed;
                _entryLuminosity.Completed -= _entryLuminosity_Completed;
                _entryLuminosity.TextChanged -= _entrySaturation_Changed;
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
                _entryLuminosity_Event(newValue);
            }
        }

        private void _entryLuminosity_Changed(object sender, TextChangedEventArgs e)
        {
            bool isUserInput = false;
            this.isUserInput.TryGetValue(EntryName.LUMINOSITY, out isUserInput);
            if (isUserInput)
            {
                double newValue = 1.0d;
                bool parseSuccess = double.TryParse(e.NewTextValue, out newValue);
                if (parseSuccess)
                {
                    _entryLuminosity_Event(newValue);
                }
            }
        }

        private void _entryLuminosity_Event(double newValue)
        {
            double maxLuminosity = ColorNumberHelper.MaxLuminosityFromSaturation(ColorNumberHelper.FromSourceToTargetSaturation(PickedColor.Saturation));
            double convertedNewValue = ColorNumberHelper.FromTargetToSourceLuminosity(newValue >= maxLuminosity ? maxLuminosity : newValue);
            Color newColor = Color.FromHsla(PickedColor.Hue, PickedColor.Saturation, convertedNewValue);
            PickedColor = newColor;
            PickedColorForHSL = Color.FromHsla(PickedColor.Hue, 1.0d, 0.5d);
        }

        // Human Interaction: Saturation
        private void _entrySaturation_Completed(object sender, EventArgs e)
        {
            double newValue = 1.0d;
            bool parseSuccess = double.TryParse(_entrySaturation.Text, out newValue);
            if (parseSuccess)
            {
                _entrySaturation_Event(newValue);
            }
        }

        private void _entrySaturation_Changed(object sender, TextChangedEventArgs e)
        {
            bool isUserInput = false;
            this.isUserInput.TryGetValue(EntryName.SATURATION, out isUserInput);
            if (isUserInput)
            {
                double newValue = 1.0d;
                bool parseSuccess = double.TryParse(e.NewTextValue, out newValue);
                if (parseSuccess)
                {
                    _entrySaturation_Event(newValue);
                }
            }
        }

        private void _entrySaturation_Event(double newValue)
        {
            double maxSaturation = ColorNumberHelper.MaxSaturationFromLuminosity(ColorNumberHelper.FromSourceToTargetLuminosity(PickedColor.Luminosity));
            double convertedNewValue = ColorNumberHelper.FromTargetToSourceSaturation(newValue >= maxSaturation ? maxSaturation : newValue);
            Color newColor = Color.FromHsla(PickedColor.Hue, convertedNewValue, PickedColor.Luminosity);
            PickedColor = newColor;
            PickedColorForHSL = Color.FromHsla(PickedColor.Hue, 1.0d, 0.5d);
        }

        // Human Interaction Hue
        private void _entryHue_Completed(object sender, EventArgs e)
        {
            double newValue = 1.0d;
            bool parseSuccess = double.TryParse(_entryHue.Text, out newValue);
            if (parseSuccess)
            {
                _entryHue_Event(newValue);
            }
        }

        private void _entryHue_Changed(object sender, TextChangedEventArgs e)
        {
            bool isUserInput = false;
            this.isUserInput.TryGetValue(EntryName.HUE, out isUserInput);
            if (isUserInput)
            {
                double newValue = 1.0d;
                bool parseSuccess = double.TryParse(e.NewTextValue, out newValue);
                if (parseSuccess)
                {
                    _entryHue_Event(newValue);
                }
            }
        }

        private void _entryHue_Event(double newValue)
        {
            double convertedNewValue = ColorNumberHelper.FromTargetToSourceHue(newValue);
            Color newColor = Color.FromHsla(convertedNewValue, PickedColor.Saturation, PickedColor.Luminosity);
            PickedColor = newColor;
            PickedColorForHSL = Color.FromHsla(PickedColor.Hue, 1.0d, 0.5d);
        }

        // Human Interaction Blue
        private void _entryBlue_Completed(object sender, EventArgs e)
        {
            double newValue = 1.0d;
            bool parseSuccess = double.TryParse(_entryBlue.Text, out newValue);
            if (parseSuccess)
            {
                _entryBlue_Event(newValue);
            }
        }

        private void _entryBlue_Changed(object sender, TextChangedEventArgs e)
        {
            bool isUserInput = false;
            this.isUserInput.TryGetValue(EntryName.BLUE, out isUserInput);
            if (isUserInput)
            {
                double newValue = 1.0d;
                bool parseSuccess = double.TryParse(e.NewTextValue, out newValue);
                if (parseSuccess)
                {
                    _entryBlue_Event(newValue);
                }
            }
        }

        private void _entryBlue_Event(double newValue)
        {
            double convertedNewValue = ColorNumberHelper.FromTargetToSourceRGB(newValue);
            Color newColor = Color.FromRgb(PickedColor.R, PickedColor.G, convertedNewValue);
            PickedColor = newColor;
            PickedColorForHSL = Color.FromHsla(PickedColor.Hue, 1.0d, 0.5d);
        }

        // Human Interaction Green
        private void _entryGreen_Completed(object sender, EventArgs e)
        {
            double newValue = 1.0d;
            bool parseSuccess = double.TryParse(_entryGreen.Text, out newValue);
            if (parseSuccess)
            {
                _entryGreen_Event(newValue);
            }
        }

        private void _entryGreen_Changed(object sender, TextChangedEventArgs e)
        {
            bool isUserInput = false;
            this.isUserInput.TryGetValue(EntryName.GREEN, out isUserInput);
            if (isUserInput)
            {
                double newValue = 1.0d;
                bool parseSuccess = double.TryParse(e.NewTextValue, out newValue);
                if (parseSuccess)
                {
                    _entryGreen_Event(newValue);
                }
            }
        }

        private void _entryGreen_Event(double newValue)
        {
            double convertedNewValue = ColorNumberHelper.FromTargetToSourceRGB(newValue);
            Color newColor = Color.FromRgb(PickedColor.R, convertedNewValue, PickedColor.B);
            PickedColor = newColor;
            PickedColorForHSL = Color.FromHsla(PickedColor.Hue, 1.0d, 0.5d);
        }

        // Human Interaction Red
        private void _entryRed_Completed(object sender, EventArgs e)
        {
            double newValue = 1.0d;
            bool parseSuccess = double.TryParse(_entryRed.Text, out newValue);
            if (parseSuccess)
            {
                _entryRed_Event(newValue);
            }
        }

        private void _entryRed_Changed(object sender, TextChangedEventArgs e)
        {
            bool isUserInput = false;
            this.isUserInput.TryGetValue(EntryName.RED, out isUserInput);
            if (isUserInput)
            {
                double newValue = 1.0d;
                bool parseSuccess = double.TryParse(e.NewTextValue, out newValue);
                if (parseSuccess)
                {
                    _entryRed_Event(newValue);
                }
            }
        }

        private void _entryRed_Event(double newValue)
        {
            double convertedNewValue = ColorNumberHelper.FromTargetToSourceRGB(newValue);
            Color newColor = Color.FromRgb(convertedNewValue, PickedColor.G, PickedColor.B);
            PickedColor = newColor;
            PickedColorForHSL = Color.FromHsla(PickedColor.Hue, 1.0d, 0.5d);
        }

        /**
         * Entry Focused & Entry Unfocused are needed for distinguishing between text changed through user and text changed through code, so that there's no 
         * circular event triggering.
         */
        private void _entryLuminosity_Focused(object sender, FocusEventArgs e)
        {
            isUserInput[EntryName.LUMINOSITY] = true;
        }

        private void _entryLuminosity_Unfocused(object sender, FocusEventArgs e)
        {
            isUserInput[EntryName.LUMINOSITY] = false;
        }

        private void _entrySaturation_Focused(object sender, FocusEventArgs e)
        {
            isUserInput[EntryName.SATURATION] = true;
        }

        private void _entrySaturation_Unfocused(object sender, FocusEventArgs e)
        {
            isUserInput[EntryName.SATURATION] = false;
        }

        private void _entryHue_Focused(object sender, FocusEventArgs e)
        {
            isUserInput[EntryName.HUE] = true;
        }

        private void _entryHue_Unfocused(object sender, FocusEventArgs e)
        {
            isUserInput[EntryName.HUE] = false;
        }

        private void _entryRed_Focused(object sender, FocusEventArgs e)
        {
            isUserInput[EntryName.RED] = true;
        }

        private void _entryRed_Unfocused(object sender, FocusEventArgs e)
        {
            isUserInput[EntryName.RED] = false;
        }

        private void _entryGreen_Focused(object sender, FocusEventArgs e)
        {
            isUserInput[EntryName.GREEN] = true;
        }

        private void _entryGreen_Unfocused(object sender, FocusEventArgs e)
        {
            isUserInput[EntryName.GREEN] = false;
        }

        private void _entryBlue_Focused(object sender, FocusEventArgs e)
        {
            isUserInput[EntryName.BLUE] = true;
        }

        private void _entryBlue_Unfocused(object sender, FocusEventArgs e)
        {
            isUserInput[EntryName.BLUE] = false;
        }

        // Human Interaction: Squared Picker Hue
        void PanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    deactivateUserInput();
                    _rectSatLumPickerThumbPreviousPositionX = e.TotalX;
                    _rectSatLumPickerThumbPreviousPostionY = e.TotalY;

                    if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS)
                    {
                        _rectSatLumPickerThumbPreviousPositionX += _rectSatLumPickerThumb.TranslationX;
                        _rectSatLumPickerThumbPreviousPostionY += _rectSatLumPickerThumb.TranslationY;
                    }
                    break;
                case GestureStatus.Running:
                    double totalX = _rectSatLumPickerThumbPreviousPositionX + e.TotalX;
                    double totalY = _rectSatLumPickerThumbPreviousPostionY + e.TotalY;

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        totalX += _rectSatLumPickerThumb.TranslationX;
                        totalY += _rectSatLumPickerThumb.TranslationY;
                    }

                    // Keep the position of thumbs in check
                    var positionX = totalX < 0 ? 0 : totalX > _rectSatLumPicker.Width ? _rectSatLumPicker.Width : totalX;
                    ValueXRectSatLumPickerThumb = positionX * MaximumX / _rectSatLumPicker.Width;
                    var positionY = totalY < 0 ? 0 : totalY > _rectSatLumPicker.Height ? _rectSatLumPicker.Height : totalY;
                    ValueYRectSatLumPickerThumb = positionY * MaximumY / _rectSatLumPicker.Height;
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
                    deactivateUserInput();
                    _rectHuePickerThumbPreviousPostionY = e.TotalY;

                    if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS)
                    {
                        _rectHuePickerThumbPreviousPostionY += _rectHuePickerThumb.TranslationY;
                    }
                    break;
                case GestureStatus.Running:
                    double totalY = _rectHuePickerThumbPreviousPostionY + e.TotalY;

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        totalY += _rectHuePickerThumb.TranslationY;
                    }

                    var positionY = totalY < 0 ? 0 : totalY > _rectHuePicker.Height ? _rectHuePicker.Height : totalY;
                    ValueRectHuePickerThumb = positionY * MaximumY / _rectHuePicker.Height;
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
                    deactivateUserInput();
                    _radialSatHuePickerThumbPreviousPositionX = e.TotalX;
                    _radialSatHuePickerThumbPreviousPositionY = e.TotalY;

                    if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS)
                    {
                        _radialSatHuePickerThumbPreviousPositionX += _radialSatHuePickerThumb.TranslationX;
                        _radialSatHuePickerThumbPreviousPositionY += _radialSatHuePickerThumb.TranslationY;
                    }
                    break;
                case GestureStatus.Running:
                    double totalX = _radialSatHuePickerThumbPreviousPositionX + e.TotalX;
                    double totalY = _radialSatHuePickerThumbPreviousPositionY + e.TotalY;

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        totalX += _radialSatHuePickerThumb.TranslationX;
                        totalY += _radialSatHuePickerThumb.TranslationY;
                    }

                    // Keep the position of thumbs in check
                    var positionX = totalX < 0 ? 0 : totalX > _radialSatHuePicker.Width ? _radialSatHuePicker.Width : totalX;
                    ValueXRadialSatHuePickerThumb = positionX * MaximumX / _radialSatHuePicker.Width;
                    var positionY = totalY < 0 ? 0 : totalY > _radialSatHuePicker.Height ? _radialSatHuePicker.Height : totalY;
                    ValueYRadialSatHuePickerThumb = positionY * MaximumY / _radialSatHuePicker.Height;
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
                    deactivateUserInput();
                    _radialLumPickerThumbPreviousPositionY = e.TotalY;

                    if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS)
                    {
                        _radialLumPickerThumbPreviousPositionY += _radialLumPickerThumb.TranslationY;
                    }
                    break;
                case GestureStatus.Running:
                    double totalY = _radialLumPickerThumbPreviousPositionY + e.TotalY;

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        totalY += _radialLumPickerThumb.TranslationY;
                    }

                    var positionY = totalY < 0 ? 0 : totalY > _radialLumPicker.Height ? _radialLumPicker.Height : totalY;
                    ValueYRadialLumThumb = positionY * MaximumY / _radialLumPicker.Height;
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
        void UpdateRectSatLumPickerThumb()
        {
            var positionX = ValueXRectSatLumPickerThumb / MaximumX * _rectSatLumPicker.Width;
            var positionY = ValueYRectSatLumPickerThumb / MaximumY * _rectSatLumPicker.Height;

            if (positionX < 0)
                positionX = 0;
            if (positionY < 0)
                positionY = 0;

            _rectSatLumPickerThumb.TranslationX = positionX;
            _rectSatLumPickerThumb.TranslationY = positionY;
        }

        // Move Hue Thumb on Human Interaction
        void UpdateRectHuePickerThumb()
        {
            var positionX = ValueXRectHuePickerThumb / MaximumX * _rectHuePicker.Width;
            var positionY = ValueRectHuePickerThumb / MaximumY * _rectHuePicker.Height;

            if (positionX < 0)
                positionX = 0;
            if (positionY < 0)
                positionY = 0;

            _rectHuePickerThumb.TranslationX = positionX;
            _rectHuePickerThumb.TranslationY = positionY;
        }

        // Move Radial Picker Hue Thumb on Human Interaction
        void UpdateRadialSatHuePickerThumb()
        {
            var centerPointX = this._radialSatHuePicker.Width / 2;
            var centerPointY = this._radialSatHuePicker.Height / 2;
            var radius = this._radialSatHuePicker.Width / 2;

            var positionX = ValueXRadialSatHuePickerThumb / MaximumX * _radialSatHuePicker.Width;
            var positionY = ValueYRadialSatHuePickerThumb / MaximumY * _radialSatHuePicker.Height;

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

            _radialSatHuePickerThumb.TranslationX = positionX;
            _radialSatHuePickerThumb.TranslationY = positionY;
        }

        // Move Radial Picker Luminosity Thumb on Human Interaction
        void UpdateRadialLumPickerThumb()
        {
            var positionX = ValueXRadialLumPickerThumb / MaximumX * _radialLumPicker.Width;
            var positionY = ValueYRadialLumThumb / MaximumY * _radialLumPicker.Height;

            if (positionX < 0)
                positionX = 0;
            if (positionY < 0)
                positionY = 0;

            _radialLumPickerThumb.TranslationX = positionX;
            _radialLumPickerThumb.TranslationY = positionY;
        }

        // Move Light & Saturation Thumb on SelectedColor changing
        void UpdateRectSatLumPickerThumbOnColorChanged()
        {
            _rectSatLumPickerThumb.TranslationX = PickedColor.Saturation * _rectSatLumPicker.Width;
            // the higher the saturation the lower the max luminosity
            double maxLuminosity = 100.0d - 0.5d * ColorNumberHelper.FromSourceToTargetSaturation(PickedColor.Saturation);
            maxLuminosity = ColorNumberHelper.FromTargetToSourceLuminosity(maxLuminosity);
            // luminosity not bigger than max luminosity
            double luminosity = PickedColor.Luminosity > maxLuminosity ? maxLuminosity : PickedColor.Luminosity;
            _rectSatLumPickerThumb.TranslationY = _rectSatLumPicker.Height - luminosity / maxLuminosity * _rectSatLumPicker.Height;
        }

        // Move Hue Thumb on SelectedColor changing
        void UpdateRectHuePickerThumbOnColorChanged()
        {
            _rectHuePickerThumb.TranslationY = PickedColor.Hue * _rectHuePicker.Height;
        }

        // Move Radial Picker Hue Thumb on SelectedColor changing
        void UpdateRadialSatHuePickerThumbOnColorChanged()
        {
            var centerPointX = this._radialSatHuePicker.Width / 2;
            var centerPointY = this._radialSatHuePicker.Height / 2;
            double radius = this._radialSatHuePicker.Width / 2;
            double hypothenuse = radius * PickedColor.Saturation;
            _radialSatHuePickerThumb.TranslationX = centerPointX + hypothenuse * Math.Sin(ColorNumberHelper.ConvertDegreesToRadians(PickedColor.Hue * 360.0));
            _radialSatHuePickerThumb.TranslationY = centerPointY - hypothenuse * Math.Cos(ColorNumberHelper.ConvertDegreesToRadians(PickedColor.Hue * 360.0));
        }

        // Update Radial Picker Luminosity Appearance
        void UpdateRadialLumPickerOnColorChanged()
        {
            // the higher the saturation the lower the max luminosity
            double maxLuminosity = 100.0d - 0.5d * ColorNumberHelper.FromSourceToTargetSaturation(PickedColor.Saturation);
            maxLuminosity = ColorNumberHelper.FromTargetToSourceLuminosity(maxLuminosity);
            Color pickedColorLumMin = Color.FromHsla(PickedColor.Hue, PickedColor.Saturation, 0.0);
            Color pickedColorLumMax = Color.FromHsla(PickedColor.Hue, PickedColor.Saturation, maxLuminosity);
            this.ColorsListBrightness = $"{ pickedColorLumMax.ToHex() },{ pickedColorLumMin.ToHex() }";
        }

        // Move Radial Picker Luminosity Thumb on SelectedColor changing
        void UpdateRadialLumPickerThumbOnColorChanged()
        {
            // the higher the saturation the lower the max luminosity
            double maxLuminosity = 100.0d - 0.5d * ColorNumberHelper.FromSourceToTargetSaturation(PickedColor.Saturation);
            maxLuminosity = ColorNumberHelper.FromTargetToSourceLuminosity(maxLuminosity);
            // luminosity not bigger than max luminosity
            double luminosity = PickedColor.Luminosity > maxLuminosity ? maxLuminosity : PickedColor.Luminosity;
            _radialLumPickerThumb.TranslationY = _radialLumPicker.Height - luminosity / maxLuminosity * _radialLumPicker.Height;
        }

        private void deactivateUserInput()
        {
            /**
             * isUserInput komplett auf false setzen
             */
            //foreach(KeyValuePair<EntryName, bool> item in isUserInput)
            //{
            //    isUserInput[item.Key] = false;
            //}
            this.isUserInput[EntryName.RED] = false;
            this.isUserInput[EntryName.GREEN] = false;
            this.isUserInput[EntryName.BLUE] = false;
            this.isUserInput[EntryName.HUE] = false;
            this.isUserInput[EntryName.SATURATION] = false;
            this.isUserInput[EntryName.LUMINOSITY] = false;
        }

        private void CalculatePickedColorBasedOnRectThumbs()
        {
            // HUE (0.0d to 360.0d)
            double hue = 360.0d * (_rectHuePickerThumb.TranslationY / _rectHuePicker.Height);

            // LUMINOSITY (0.0d to 100.0d)
            double positionXhslInv = _rectSatLumPicker.Width - _rectSatLumPickerThumb.TranslationX;
            double positionYhslInv = _rectSatLumPicker.Height - _rectSatLumPickerThumb.TranslationY;
            double minimumLuminosity = 50.0d;
            double maximumLuminosity = minimumLuminosity + (positionXhslInv / _rectSatLumPicker.Width * 50.0d);
            double luminosity = positionYhslInv / _rectSatLumPicker.Height * maximumLuminosity;

            // SATURATION (0.0d to 100.0d)
            double saturation = _rectSatLumPickerThumb.TranslationX / _rectSatLumPicker.Width * 100;

            // SETTING PICKED COLOR
            double technicalHue = ColorNumberHelper.FromTargetToSourceHue(hue);
            double technicalSaturation = ColorNumberHelper.FromTargetToSourceSaturation(saturation);
            double technicalLuminosity = ColorNumberHelper.FromTargetToSourceLuminosity(luminosity);
            this.PickedColor = Color.FromHsla(technicalHue, technicalSaturation, technicalLuminosity);
            this.PickedColorForHSL = Color.FromHsla(PickedColor.Hue, 1.0d, 0.5d);
        }

        private void CalculatePickedColorBasedOnRadialThumbs()
        {
            // HUE (0.0d to 360.0d)
            var positionX = _radialSatHuePickerThumb.TranslationX;
            var positionY = _radialSatHuePickerThumb.TranslationY;
            var centerPointX = this._radialSatHuePicker.Width / 2;
            var centerPointY = this._radialSatHuePicker.Height / 2;
            var positionXRelativeToCenter = positionX - centerPointX;
            var positionYRelativeToCenter = centerPointY - positionY;
            double hue = calculateAngleClockwise(positionXRelativeToCenter, positionYRelativeToCenter);

            // SATURATION (0.0d to 100.0d)
            var radius = this._radialSatHuePicker.Width / 2;
            var hypothenuse = Math.Sqrt(Math.Pow(positionXRelativeToCenter, 2.0) + Math.Pow(positionYRelativeToCenter, 2.0));
            double saturation = hypothenuse / radius * 100.0;

            // LUMINOSITY (0.0d to 100.0d)
            // the higher the saturation the lower the max luminosity
            double maxLuminosity = 100.0d - 0.5d * saturation;
            // luminosity not bigger than max luminosity
            double luminosity = (_radialLumPicker.Height - _radialLumPickerThumb.TranslationY) / _radialLumPicker.Height * maxLuminosity;

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