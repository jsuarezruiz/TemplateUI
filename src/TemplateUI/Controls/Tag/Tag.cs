using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class Tag : TemplatedView
    {
        const string ElementBorder = "PART_Border";
        const string ElementText = "PART_Text";
        const string ElementButtonClose = "PART_ButtonClose";

        Frame _border;
        Label _text;
        Grid _buttonClose;

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(Tag), string.Empty,
            propertyChanged: OnTextChanged);

        static void OnTextChanged(BindableObject bindable, object oldValue, object newValue) => ((Tag)bindable).UpdateText();

        void UpdateText() => _text.Text = Text;

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(Tag), Color.Default,
            propertyChanged: OnTextColorChanged);

        static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue) => ((Tag)bindable).UpdateTextColor();

        void UpdateTextColor() => _text.TextColor = TextColor;

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static new readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(Tag), Color.Default,
            propertyChanged: OnBackgroundColorChanged);

        static void OnBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue) => ((Tag)bindable).UpdateBackgroundColor();

        void UpdateBackgroundColor() => _border.BackgroundColor = BackgroundColor;

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public static readonly BindableProperty SelectedBackgroundColorProperty =
            BindableProperty.Create(nameof(SelectedBackgroundColor), typeof(Color), typeof(Tag), Color.Blue);

        public Color SelectedBackgroundColor
        {
            get => (Color)GetValue(SelectedBackgroundColorProperty);
            set => SetValue(SelectedBackgroundColorProperty, value);
        }

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(Tag), Color.Default,
            propertyChanged: OnBorderColorChanged);

        static void OnBorderColorChanged(BindableObject bindable, object oldValue, object newValue) => ((Tag)bindable).UpdateBorderColor();

        void UpdateBorderColor() => _border.BorderColor = BorderColor;

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty HasShadowProperty =
            BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(Tag), false,
            propertyChanged: OnHasShadowChanged);

        static void OnHasShadowChanged(BindableObject bindable, object oldValue, object newValue) => ((Tag)bindable).UpdateHasShadow();

        void UpdateHasShadow() => _border.HasShadow = HasShadow;

        public bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(Tag), Device.GetNamedSize(NamedSize.Small, typeof(Label)),
            propertyChanged: OnFontSizeChanged);

        static void OnFontSizeChanged(BindableObject bindable, object oldValue, object newValue) => ((Tag)bindable).UpdateFontSize();

        void UpdateFontSize() => _text.FontSize = FontSize;

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly BindableProperty FontFamilyProperty =
          BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(Tag), string.Empty,
            propertyChanged: OnFontFamilyChanged);

        static void OnFontFamilyChanged(BindableObject bindable, object oldValue, object newValue) => ((Tag)bindable).UpdateFontFamily();

        void UpdateFontFamily() => _text.FontFamily = FontFamily;

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set { SetValue(FontFamilyProperty, value); }
        }

        public static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(Tag), FontAttributes.None,
            propertyChanged: OnFontAttributesChanged);

        static void OnFontAttributesChanged(BindableObject bindable, object oldValue, object newValue) => ((Tag)bindable).UpdateFontAttributes();

        void UpdateFontAttributes() => _text.FontAttributes = FontAttributes;

        public FontAttributes FontAttributes
        {
            get => (FontAttributes)GetValue(FontAttributesProperty);
            set { SetValue(FontAttributesProperty, value); }
        }

        public static readonly BindableProperty ShowCloseButtonProperty =
            BindableProperty.Create(nameof(ShowCloseButton), typeof(bool), typeof(Tag), true);

        public bool ShowCloseButton
        {
            get => (bool)GetValue(ShowCloseButtonProperty);
            set { SetValue(ShowCloseButtonProperty, value); }
        }

        public static readonly BindableProperty SelectableProperty =
            BindableProperty.Create(nameof(Selectable), typeof(bool), typeof(Tag), false,
                propertyChanged: OnSelectableChanged);

        static void OnSelectableChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as Tag)?.UpdateSelectable();
        }

        public bool Selectable
        {
            get => (bool)GetValue(SelectableProperty);
            set { SetValue(SelectableProperty, value); }
        }

        public static readonly BindableProperty IsSelectedProperty =
          BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(Tag), false);

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(Tag), null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(Tag), null);

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set { SetValue(CommandParameterProperty, value); }
        }

        public event EventHandler Selected;
        public event EventHandler Closed;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _border = GetTemplateChild(ElementBorder) as Frame;
            _text = GetTemplateChild(ElementText) as Label;
            _buttonClose = GetTemplateChild(ElementButtonClose) as Grid;

            UpdateIsEnabled();
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
                _border.Opacity = 1.0d;

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += OnCloseButtonTapped;
                _buttonClose.GestureRecognizers.Add(tapGestureRecognizer);
            }
            else
            {
                _border.Opacity = 0.4d;

                _buttonClose.GestureRecognizers.Clear();
            }
        }

        void UpdateSelectable()
        {
            if (Selectable)
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += OnSelected;
                _border.GestureRecognizers.Add(tapGestureRecognizer);
            }
            else
            {
                // TODO: Remove only specific gestures.
                _border.GestureRecognizers.Clear();
            }
        }

        void OnCloseButtonTapped(object sender, EventArgs e)
        {
            Closed?.Invoke(this, EventArgs.Empty);
            Command?.Execute(CommandParameter);
        }

        void OnSelected(object sender, EventArgs e)
        {
            IsSelected = !IsSelected;

            if (IsSelected)
                _border.BackgroundColor = SelectedBackgroundColor;
            else
                _border.BackgroundColor = BackgroundColor;

            Selected?.Invoke(this, EventArgs.Empty);
        }
    }
}