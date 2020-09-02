using System.ComponentModel;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class PinItem : TemplatedView
    {
        const string ElementBorder = "PART_Border";
        const string ElementText = "PART_Text";

        Frame _border;
        Label _text;

        public static new readonly BindableProperty IsFocusedProperty =
             BindableProperty.Create(nameof(IsFocused), typeof(bool), typeof(PinItem), false,
                 propertyChanged: OnPinItemPropertyChanged);

        public new bool IsFocused
        {
            get => (bool)GetValue(IsFocusedProperty);
            set { SetValue(IsFocusedProperty, value); }
        }

        public static readonly BindableProperty PasswordCharProperty =
            BindableProperty.Create(nameof(PasswordChar), typeof(char), typeof(PinItem), PinBox.DefaultPasswordChart);

        public char PasswordChar
        {
            get => (char)GetValue(PasswordCharProperty);
            set => SetValue(PasswordCharProperty, value);
        }

        public static readonly BindableProperty ItemSizeProperty =
            BindableProperty.Create(nameof(ItemSize), typeof(double), typeof(PinItem), PinBox.DefaultItemSize);

        public double ItemSize
        {
            get => (double)GetValue(ItemSizeProperty);
            set => SetValue(ItemSizeProperty, value);
        }
     
        public static readonly BindableProperty ItemCornerRadiusProperty =
            BindableProperty.Create(nameof(ItemCornerRadius), typeof(double), typeof(PinBox), 0.0d);

        public double ItemCornerRadius
        {
            get => (double)GetValue(ItemCornerRadiusProperty);
            set => SetValue(ItemCornerRadiusProperty, value);
        }

        public static readonly BindableProperty ItemMarginProperty =
            BindableProperty.Create(nameof(ItemMargin), typeof(Thickness), typeof(PinItem), new Thickness());

        public Thickness ItemMargin
        {
            get => (Thickness)GetValue(ItemMarginProperty);
            set => SetValue(ItemMarginProperty, value);
        }

        public static readonly BindableProperty ItemColorProperty =      
            BindableProperty.Create(nameof(ItemColor), typeof(Color), typeof(PinItem), Color.Default,
                propertyChanged: OnPinItemPropertyChanged);

        public Color ItemColor
        {
            get => (Color)GetValue(ItemColorProperty);
            set => SetValue(ItemColorProperty, value);
        }

        public static readonly BindableProperty ItemColorFocusedProperty =
            BindableProperty.Create(nameof(ItemColorFocused), typeof(Color), typeof(PinItem), Color.Default, 
                propertyChanged: OnPinItemPropertyChanged);

        public Color ItemColorFocused
        {
            get => (Color)GetValue(ItemColorFocusedProperty);
            set => SetValue(ItemColorFocusedProperty, value);
        }

        public static readonly BindableProperty ItemBorderColorProperty =
            BindableProperty.Create(nameof(ItemBorderColor), typeof(Color), typeof(PinItem), Color.Default, 
                propertyChanged: OnPinItemPropertyChanged);

        public Color ItemBorderColor
        {
            get => (Color)GetValue(ItemBorderColorProperty);
            set => SetValue(ItemBorderColorProperty, value);
        }

        public static readonly BindableProperty ItemBorderColorFocusedProperty =
            BindableProperty.Create(nameof(ItemBorderColorFocused), typeof(Color), typeof(PinItem), Color.Default, 
                propertyChanged: OnPinItemPropertyChanged);

        public Color ItemBorderColorFocused
        {
            get => (Color)GetValue(ItemBorderColorFocusedProperty);
            set => SetValue(ItemBorderColorFocusedProperty, value);
        }

        static void OnPinItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PinItem)?.UpdateCurrent();
        }

        internal static readonly BindablePropertyKey CurrentItemColorPropertyKey = BindableProperty.CreateReadOnly(nameof(CurrentItemColor), typeof(Color), typeof(PinItem), Color.Default);

        public static readonly BindableProperty CurrentItemColorProperty = CurrentItemColorPropertyKey.BindableProperty;

        public Color CurrentItemColor
        {
            get
            {
                return (Color)GetValue(CurrentItemColorProperty);
            }
            private set
            {
                SetValue(CurrentItemColorPropertyKey, value);
            }
        }

        internal static readonly BindablePropertyKey CurrentItemBorderColorPropertyKey = BindableProperty.CreateReadOnly(nameof(CurrentItemBorderColor), typeof(Color), typeof(PinItem), Color.Default);

        public static readonly BindableProperty CurrentItemBorderColorProperty = CurrentItemBorderColorPropertyKey.BindableProperty;

        public Color CurrentItemBorderColor
        {
            get
            {
                return (Color)GetValue(CurrentItemBorderColorProperty);
            }
            private set
            {
                SetValue(CurrentItemBorderColorPropertyKey, value);
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            _border = GetTemplateChild(ElementBorder) as Frame;
            _text = GetTemplateChild(ElementText) as Label;
        }

        public void UpdatePasswordCharIsVisible(bool isVisible)
        {
            if (_text == null)
                return;

            _text.Text = isVisible ? PasswordChar.ToString() : string.Empty;
        }

        void UpdateCurrent()
        {
            CurrentItemColor = !IsFocused || ItemColorFocused == Color.Default ? ItemColor : ItemColorFocused;
            CurrentItemBorderColor = !IsFocused || ItemBorderColorFocused == Color.Default ? ItemBorderColor : ItemBorderColorFocused;
        }
    }
}
