using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class SegmentedItem : TemplatedView
    {
        const string ElementContainer = "PART_Container";
        const string ElementIcon = "PART_Icon";
        const string ElementText = "PART_Text";

        Grid _container;
        Image _icon;
        Label _text;

        public static readonly BindableProperty TextProperty =
           BindableProperty.Create(nameof(Text), typeof(string), typeof(SegmentedItem), string.Empty);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static new readonly BindableProperty BackgroundColorProperty =
          BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(SegmentedItem), Color.Default,
              propertyChanged: OnSegmentedItemPropertyChanged);

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly BindableProperty BackgroundColorSelectedProperty =
        BindableProperty.Create(nameof(BackgroundColorSelected), typeof(Color), typeof(SegmentedItem), Color.Default);

        public Color BackgroundColorSelected
        {
            get => (Color)GetValue(BackgroundColorSelectedProperty);
            set { SetValue(BackgroundColorSelectedProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty =
           BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(SegmentedItem), Color.Default,
               propertyChanged: OnSegmentedItemPropertyChanged);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty TextColorSelectedProperty =
            BindableProperty.Create(nameof(TextColorSelected), typeof(Color), typeof(SegmentedItem), Color.Default);

        public Color TextColorSelected
        {
            get => (Color)GetValue(TextColorSelectedProperty);
            set { SetValue(TextColorSelectedProperty, value); }
        }

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(SegmentedItem), Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                propertyChanged: OnSegmentedItemPropertyChanged);

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly BindableProperty FontSizeSelectedProperty =
            BindableProperty.Create(nameof(FontSizeSelected), typeof(double), typeof(SegmentedItem), Device.GetNamedSize(NamedSize.Small, typeof(Label)));

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSizeSelected
        {
            get => (double)GetValue(FontSizeSelectedProperty);
            set { SetValue(FontSizeSelectedProperty, value); }
        }

        public static readonly BindableProperty FontFamilyProperty =
          BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(SegmentedItem), string.Empty,
              propertyChanged: OnSegmentedItemPropertyChanged);

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set { SetValue(FontFamilyProperty, value); }
        }

        public static readonly BindableProperty FontFamilySelectedProperty =
            BindableProperty.Create(nameof(FontFamilySelected), typeof(string), typeof(SegmentedItem), string.Empty,
                propertyChanged: OnSegmentedItemPropertyChanged);

        public string FontFamilySelected
        {
            get => (string)GetValue(FontFamilySelectedProperty);
            set { SetValue(FontFamilySelectedProperty, value); }
        }

        public static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(SegmentedItem), FontAttributes.None,
                propertyChanged: OnSegmentedItemPropertyChanged);

        public FontAttributes FontAttributes
        {
            get => (FontAttributes)GetValue(FontAttributesProperty);
            set { SetValue(FontAttributesProperty, value); }
        }

        public static readonly BindableProperty FontAttributesSelectedProperty =
            BindableProperty.Create(nameof(FontAttributesSelected), typeof(FontAttributes), typeof(SegmentedItem), FontAttributes.None,
                propertyChanged: OnSegmentedItemPropertyChanged);

        public FontAttributes FontAttributesSelected
        {
            get => (FontAttributes)GetValue(FontAttributesSelectedProperty);
            set { SetValue(FontAttributesSelectedProperty, value); }
        }

        public static readonly BindableProperty IconProperty =
          BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(SegmentedItem), null,
              propertyChanged: OnSegmentedItemPropertyChanged);

        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set { SetValue(IconProperty, value); }
        }

        public static readonly BindableProperty IconSelectedProperty =
          BindableProperty.Create(nameof(IconSelected), typeof(ImageSource), typeof(SegmentedItem), null,
              propertyChanged: OnSegmentedItemPropertyChanged);

        public ImageSource IconSelected
        {
            get => (ImageSource)GetValue(IconSelectedProperty);
            set { SetValue(IconSelectedProperty, value); }
        }

        public static readonly BindableProperty IsSelectedProperty =
         BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(SegmentedItem), false,
             propertyChanged: OnIsSelectedChanged);

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set { SetValue(IsSelectedProperty, value); }
        }

        static void OnIsSelectedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SegmentedItem)?.UpdateCurrent();
        }

        static void OnSegmentedItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SegmentedItem)?.UpdateCurrent();
        }

        internal static readonly BindablePropertyKey CurrentBackgroundColorPropertyKey =
            BindableProperty.CreateReadOnly(nameof(CurrentBackgroundColor), typeof(Color), typeof(SegmentedItem), Color.Default);

        public static readonly BindableProperty CurrentBackgroundColorProperty = CurrentBackgroundColorPropertyKey.BindableProperty;

        public Color CurrentBackgroundColor
        {
            get
            {
                return (Color)GetValue(CurrentBackgroundColorProperty);
            }
            private set
            {
                SetValue(CurrentBackgroundColorPropertyKey, value);
            }
        }

        internal static readonly BindablePropertyKey CurrentTextColorPropertyKey =
            BindableProperty.CreateReadOnly(nameof(CurrentTextColor), typeof(Color), typeof(SegmentedItem), Color.Default);

        public static readonly BindableProperty CurrentTextColorProperty = CurrentTextColorPropertyKey.BindableProperty;

        public Color CurrentTextColor
        {
            get
            {
                return (Color)GetValue(CurrentTextColorProperty);
            }
            private set
            {
                SetValue(CurrentTextColorPropertyKey, value);
            }
        }

        internal static readonly BindablePropertyKey CurrentFontSizePropertyKey =
            BindableProperty.CreateReadOnly(nameof(CurrentFontSize), typeof(double), typeof(SegmentedItem), null);

        public static readonly BindableProperty CurrentFontSizeProperty = CurrentFontSizePropertyKey.BindableProperty;

        public double CurrentFontSize
        {
            get
            {
                return (double)GetValue(CurrentFontSizeProperty);
            }
            private set
            {
                SetValue(CurrentFontSizePropertyKey, value);
            }
        }

        internal static readonly BindablePropertyKey CurrentFontFamilyPropertyKey =
            BindableProperty.CreateReadOnly(nameof(CurrentFontFamily), typeof(string), typeof(SegmentedItem), string.Empty);

        public static readonly BindableProperty CurrentFontFamilyProperty = CurrentFontFamilyPropertyKey.BindableProperty;

        public string CurrentFontFamily
        {
            get
            {
                return (string)GetValue(CurrentFontFamilyProperty);
            }
            private set
            {
                SetValue(CurrentFontFamilyPropertyKey, value);
            }
        }

        internal static readonly BindablePropertyKey CurrentFontAttributesPropertyKey =
            BindableProperty.CreateReadOnly(nameof(CurrentFontAttributes), typeof(FontAttributes), typeof(SegmentedItem), FontAttributes.None);

        public static readonly BindableProperty CurrentFontAttributesProperty = CurrentFontAttributesPropertyKey.BindableProperty;

        public FontAttributes CurrentFontAttributes
        {
            get
            {
                return (FontAttributes)GetValue(CurrentFontAttributesProperty);
            }
            private set
            {
                SetValue(CurrentFontAttributesPropertyKey, value);
            }
        }

        internal static readonly BindablePropertyKey CurrentIconPropertyKey = BindableProperty.CreateReadOnly(nameof(CurrentIcon), typeof(ImageSource), typeof(SegmentedItem), null);

        public static readonly BindableProperty CurrentIconProperty = CurrentIconPropertyKey.BindableProperty;

        public ImageSource CurrentIcon
        {
            get
            {
                return (ImageSource)GetValue(CurrentIconProperty);
            }
            private set
            {
                SetValue(CurrentIconPropertyKey, value);
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _container = GetTemplateChild(ElementContainer) as Grid;
            _icon = GetTemplateChild(ElementIcon) as Image;
            _text = GetTemplateChild(ElementText) as Label;
        }

        void UpdateCurrent()
        {
            CurrentBackgroundColor = !IsSelected || BackgroundColorSelected == Color.Default ? BackgroundColor : BackgroundColorSelected;
            CurrentTextColor = !IsSelected || TextColorSelected == Color.Default ? TextColor : TextColorSelected;
            CurrentFontSize = !IsSelected || FontSizeSelected == FontSize ? FontSize : FontSizeSelected;
            CurrentFontFamily = !IsSelected || string.IsNullOrEmpty(FontFamilySelected) ? FontFamily : FontFamilySelected;
            CurrentFontAttributes = !IsSelected || FontAttributesSelected == FontAttributes.None ? FontAttributes : FontAttributesSelected;
            CurrentIcon = !IsSelected || IconSelected == null ? Icon : IconSelected;

            UpdateLayout();
        }

        void UpdateLayout()
        {
            if (_text == null || _icon == null)
                return;

            if (Icon != null && !string.IsNullOrEmpty(Text))
            {
                Grid.SetRow(_icon, 0);
                Grid.SetRowSpan(_icon, 1);

                Grid.SetRow(_text, 1);
                Grid.SetRowSpan(_text, 1);
            }
            else if (Icon == null && !string.IsNullOrEmpty(Text))
            {
                Grid.SetRow(_text, 0);
                Grid.SetRowSpan(_text, 2);
            }
            else
            {
                Grid.SetRow(_icon, 0);
                Grid.SetRowSpan(_icon, 2);
            }
        }
    }
}