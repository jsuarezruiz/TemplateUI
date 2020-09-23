using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace TemplateUI.Controls
{
    public class ToggleSwitch : TemplatedView
    {
        public const string ToggleSwitchOnVisualState = "On";
        public const string ToggleSwitchOffVisualState = "Off";

        const string ElementContainer = "PART_Container";
        const string ElementBackground = "PART_Background";
        const string ElementThumb = "PART_Thumb";

        Grid _container;
        Shape _background;
        Shape _thumb;

        public static readonly BindableProperty IsToggledProperty =
            BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(ToggleSwitch), false,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    if (bindable is ToggleSwitch toggleSwitch)
                    {
                        toggleSwitch.Toggled?.Invoke(bindable, new ToggledEventArgs((bool)newValue));
                        toggleSwitch.UpdateCurrent();
                        toggleSwitch.UpdateIsToggled();
                        toggleSwitch.UpdateVisualState();
                    }
                }, defaultBindingMode: BindingMode.TwoWay);

        public bool IsToggled
        {
            get => (bool)GetValue(IsToggledProperty);
            set { SetValue(IsToggledProperty, value); }
        }

        public static readonly BindableProperty VisualTypeProperty =
          BindableProperty.Create(nameof(VisualType), typeof(VisualType), typeof(ToggleSwitch), VisualType.Cupertino,
              propertyChanged: OnVisualTypeChanged);

        static void OnVisualTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ToggleSwitch)?.UpdateVisualType();
        }

        public VisualType VisualType
        {
            get => (VisualType)GetValue(VisualTypeProperty);
            set { SetValue(VisualTypeProperty, value); }
        }

        public static readonly BindableProperty ThumbColorProperty =
            BindableProperty.Create(nameof(ThumbColor), typeof(Color), typeof(ToggleSwitch), Color.WhiteSmoke);

        public Color ThumbColor
        {
            get => (Color)GetValue(ThumbColorProperty);
            set { SetValue(ThumbColorProperty, value); }
        }

        public static readonly BindableProperty OnColorProperty =
            BindableProperty.Create(nameof(OnColor), typeof(Color), typeof(ToggleSwitch), Color.DarkGray,
                propertyChanged: OnColorPropertyChanged);

        public Color OnColor
        {
            get => (Color)GetValue(OnColorProperty);
            set { SetValue(OnColorProperty, value); }
        }

        public static readonly BindableProperty OffColorProperty =
            BindableProperty.Create(nameof(OffColor), typeof(Color), typeof(ToggleSwitch), Color.LightGray,
                propertyChanged: OnColorPropertyChanged);

        public Color OffColor
        {
            get => (Color)GetValue(OffColorProperty);
            set { SetValue(OffColorProperty, value); }
        }

        static void OnColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ToggleSwitch)?.UpdateCurrent();
        }

        public static readonly BindableProperty BorderWidthProperty =
          BindableProperty.Create(nameof(BorderWidth), typeof(double), typeof(ToggleSwitch), 0.0d);

        public double BorderWidth
        {
            get => (double)GetValue(BorderWidthProperty);
            set { SetValue(BorderWidthProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty =
          BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(ToggleSwitch), Color.Default);

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(ToggleSwitch),
                (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS) ? 6.0d : 24.0d);

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set { SetValue(CornerRadiusProperty, value); }
        }

        internal static readonly BindablePropertyKey CurrentColorPropertyKey =
            BindableProperty.CreateReadOnly(nameof(CurrentColor), typeof(Color), typeof(ToggleSwitch), Color.Default);

        public static readonly BindableProperty CurrentColorProperty = CurrentColorPropertyKey.BindableProperty;

        public Color CurrentColor
        {
            get
            {
                return (Color)GetValue(CurrentColorProperty);
            }
            private set
            {
                SetValue(CurrentColorPropertyKey, value);
            }
        }

        public event EventHandler Toggled;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _container = GetTemplateChild(ElementContainer) as Grid;
            _background = GetTemplateChild(ElementBackground) as Shape;
            _thumb = GetTemplateChild(ElementThumb) as Shape;

            UpdateIsEnabled();
            UpdateCurrent();
            UpdateIsToggled();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
        }

        void UpdateIsEnabled()
        {
            if (IsEnabled)
            {
                var tapBackground = new TapGestureRecognizer();
                tapBackground.Tapped += OnBackgroundTapped;
                _background.GestureRecognizers.Add(tapBackground);

                var tapThumb = new TapGestureRecognizer();
                tapThumb.Tapped += OnThumbTapped;
                _thumb.GestureRecognizers.Add(tapThumb);

                _container.Opacity = 1.0;
            }
            else
            {
                // TODO: Remove only specific gestures.
                _background.GestureRecognizers.Clear();
                _thumb.GestureRecognizers.Clear();

                _container.Opacity = 0.75;
            }
        }

        void UpdateVisualType()
        {
            switch(VisualType)
            {
                case VisualType.Cupertino:
                    Application.Current.Resources.TryGetValue("CupertinoControlTemplate", out object cupertinoControlTemplate);
                    ControlTemplate = cupertinoControlTemplate as ControlTemplate;
                    break;
                case VisualType.Fluent:
                    Application.Current.Resources.TryGetValue("FluentControlTemplate", out object fluentControlTemplate);
                    ControlTemplate = fluentControlTemplate as ControlTemplate;
                    break;
                case VisualType.Material:
                    Application.Current.Resources.TryGetValue("MaterialControlTemplate", out object materialControlTemplate);
                    ControlTemplate = materialControlTemplate as ControlTemplate;
                    break;
            }
        }

        void UpdateCurrent()
        {
            CurrentColor = IsToggled ? OnColor : OffColor;
        }

        void UpdateVisualState()
        {
            if (IsEnabled && IsToggled)
                VisualStateManager.GoToState(this, ToggleSwitchOnVisualState);
            else if (IsEnabled && !IsToggled)
                VisualStateManager.GoToState(this, ToggleSwitchOffVisualState);
        }

        void OnBackgroundTapped(object sender, EventArgs e)
        {
            IsToggled = !IsToggled;
        }

        void OnThumbTapped(object sender, EventArgs e)
        {
            IsToggled = !IsToggled;
        }

        void UpdateIsToggled()
        {
            // TODO: Animate the Thumb.

            if (IsToggled)
                _thumb.HorizontalOptions = LayoutOptions.End;
            else
                _thumb.HorizontalOptions = LayoutOptions.Start;
        }
    }
}