using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class PinBox : TemplatedView
    {
        public static char DefaultPasswordChart = '●';
        public static double DefaultItemSize = 48.0d;

        const string ElementContainer = "PART_Container";
        const string ElementPinContainer = "PART_PinContainer";

        Grid _container;
        Entry _internalEntry;
        StackLayout _pinContainer;

        public static readonly BindableProperty LengthProperty =
             BindableProperty.Create(nameof(Length), typeof(int), typeof(PinBox), 4,
                 propertyChanged: LengthChanged);

        static void LengthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PinBox)?.UpdateLength();
        }

        public int Length
        {
            get => (int)GetValue(LengthProperty);
            set => SetValue(LengthProperty, value);
        }

        public static readonly BindableProperty PasswordProperty =
            BindableProperty.Create(nameof(Password), typeof(string), typeof(PinBox), string.Empty,
                propertyChanged: OnPasswordChanged);

        static void OnPasswordChanged(BindableObject bindable, object oldValue, object newValue)
        {
            string newPassword = newValue.ToString();
            string oldPassword = oldValue.ToString();

            (bindable as PinBox)?.UpdatePassword(oldPassword, newPassword);
        }

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public static readonly BindableProperty PasswordCharProperty =
            BindableProperty.Create(nameof(PasswordChar), typeof(char), typeof(PinBox), DefaultPasswordChart, 
                propertyChanged: OnPinBoxChanged);

        public char PasswordChar
        {
            get => (char)GetValue(PasswordCharProperty);
            set => SetValue(PasswordCharProperty, value);
        }

        public static readonly BindableProperty ItemSizeProperty =
            BindableProperty.Create(nameof(ItemSize), typeof(double), typeof(PinBox), DefaultItemSize, 
                propertyChanged: OnPinBoxChanged);

        public double ItemSize
        {
            get => (double)GetValue(ItemSizeProperty);
            set => SetValue(ItemSizeProperty, value);
        }

        public static readonly BindableProperty ItemCornerRadiusProperty =
            BindableProperty.Create(nameof(ItemCornerRadius), typeof(double), typeof(PinBox), 0.0d,
                propertyChanged: OnPinBoxChanged);

        public double ItemCornerRadius
        {
            get => (double)GetValue(ItemCornerRadiusProperty);
            set => SetValue(ItemCornerRadiusProperty, value);
        }

        public static readonly BindableProperty ItemMarginProperty =
          BindableProperty.Create(nameof(ItemMargin), typeof(Thickness), typeof(PinBox), new Thickness(),
              propertyChanged: OnPinBoxChanged);

        public Thickness ItemMargin
        {
            get => (Thickness)GetValue(ItemMarginProperty);
            set => SetValue(ItemMarginProperty, value);
        }

        public static readonly BindableProperty ItemColorProperty =
            BindableProperty.Create(nameof(ItemColor), typeof(Color), typeof(PinBox), Color.Default,
                propertyChanged: OnPinBoxChanged);

        public Color ItemColor
        {
            get => (Color)GetValue(ItemColorProperty);
            set => SetValue(ItemColorProperty, value);
        }

        public static readonly BindableProperty ItemColorFocusedProperty =
            BindableProperty.Create(nameof(ItemColorFocused), typeof(Color), typeof(PinBox), Color.Default,   
                propertyChanged: OnPinBoxChanged);

        public Color ItemColorFocused
        {
            get => (Color)GetValue(ItemColorFocusedProperty);
            set => SetValue(ItemColorFocusedProperty, value);
        }

        public static readonly BindableProperty ItemBorderColorProperty =
            BindableProperty.Create(nameof(ItemBorderColor), typeof(Color), typeof(PinBox), Color.Default,
                propertyChanged: OnPinBoxChanged);

        public Color ItemBorderColor
        {
            get => (Color)GetValue(ItemBorderColorProperty);
            set => SetValue(ItemBorderColorProperty, value);
        }

        public static readonly BindableProperty ItemBorderColorFocusedProperty =
            BindableProperty.Create(nameof(ItemBorderColorFocused), typeof(Color), typeof(PinBox), Color.Default,
                propertyChanged: OnPinBoxChanged);

        public Color ItemBorderColorFocused
        {
            get => (Color)GetValue(ItemBorderColorFocusedProperty);
            set => SetValue(ItemBorderColorFocusedProperty, value);
        }

        static void OnPinBoxChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PinBox)?.UpdatePinItems();
        }

        public static BindableProperty IsAnimatedProperty =
            BindableProperty.Create(nameof(IsAnimated), typeof(bool), typeof(PinBox), defaultValue: true);

        public bool IsAnimated
        {
            get { return (bool)GetValue(IsAnimatedProperty); }
            set { SetValue(IsAnimatedProperty, value); }
        }

        public static BindableProperty AnimationProperty =
            BindableProperty.Create(nameof(Animation), typeof(IPinAnimation), typeof(PinBox), new PinAnimation());

        public IPinAnimation Animation
        {
            get { return (IPinAnimation)GetValue(AnimationProperty); }
            set { SetValue(AnimationProperty, value); }
        }

        public event EventHandler Focused; 
        public event EventHandler Unfocused;
        public event EventHandler<PinCompletedEventArgs> Completed;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _internalEntry = new Entry
            {
                Keyboard = Keyboard.Numeric,
                Opacity = 0,
                HorizontalOptions = LayoutOptions.Center
            };

            _container = GetTemplateChild(ElementContainer) as Grid;
            _pinContainer = GetTemplateChild(ElementPinContainer) as StackLayout;

            if (!_container.Children.Contains(_internalEntry))
            {
                _container.Children.Add(_internalEntry);
                _container.RaiseChild(_pinContainer);
            }

            _internalEntry.TextChanged += OnInternalEntryTextChanged;
            _internalEntry.Focused += OnInternalEntryFocused;
            _internalEntry.Unfocused += OnInternalEntryUnfocused;

            UpdateLength();
            UpdatePinItems();
            UpdateIsEnabled();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
        }

        void UpdateLength()
        {
            _pinContainer.Children.Clear();

            for (int i = 0; i < Length; i++)
            {
                PinItem pinItem = new PinItem();
                _pinContainer.Children.Add(pinItem);
            }

            UpdateSize();
        }

        async Task UpdatePassword(string oldPassword, string newPassword)
        {
            if (newPassword.Length == 0 && oldPassword.Length == 0)
                return;

            _internalEntry.Text = Password;

            var pinItems = _pinContainer.Children.Select(x => x as PinItem).ToArray();

            for (int i = 0; i < Length; i++)
            {
                if (i < newPassword.Length)
                    pinItems[i].UpdatePasswordCharIsVisible(true);
                else
                {
                    pinItems[i].UpdatePasswordCharIsVisible(false);
                    pinItems[i].IsFocused = false;
                    await Animation.OnUnfocus(pinItems[i]);
                }
            }

            if (_internalEntry.IsFocused)
            {
                if (newPassword.Length == Length)
                {
                    pinItems[newPassword.Length - 1].IsFocused = true;
                    await Animation.OnFocus(pinItems[newPassword.Length - 1]);
                }
                else
                {
                    pinItems[newPassword.Length].IsFocused = true;
                    await Animation.OnFocus(pinItems[newPassword.Length]);
                }
            }
        }

        void UpdateSize()
        {
            _internalEntry.WidthRequest = (ItemSize + ItemMargin.Left + ItemMargin.Right) * Length;
        }

        void UpdateIsEnabled()
        {
            _internalEntry.IsEnabled = IsEnabled;
            _container.Opacity = IsEnabled ? 1.0 : 0.75;
        }

        void UpdatePinItems()
        {
            if (_pinContainer == null)
                return;

            foreach(var children in _pinContainer.Children)
            {
                if(children is PinItem pinItem)
                {
                    if (pinItem.PasswordChar == DefaultPasswordChart)
                        pinItem.PasswordChar = PasswordChar;

                    if (pinItem.ItemSize == DefaultItemSize)
                        pinItem.ItemSize = ItemSize;
                    
                    if (pinItem.ItemCornerRadius == 0)
                        pinItem.ItemCornerRadius = ItemCornerRadius;

                    if (pinItem.ItemMargin == new Thickness())
                        pinItem.ItemMargin = ItemMargin;

                    if (pinItem.ItemColor == Color.Default)
                        pinItem.ItemColor = ItemColor; 
                    
                    if (pinItem.ItemColorFocused == Color.Default)
                        pinItem.ItemColorFocused = ItemColorFocused;

                    if (pinItem.ItemBorderColor == Color.Default)
                        pinItem.ItemBorderColor = ItemBorderColor;

                    if (pinItem.ItemBorderColorFocused == Color.Default)
                        pinItem.ItemBorderColorFocused = ItemBorderColorFocused;
                }
            }

            UpdateSize();
        }

        void OnInternalEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            string newPassword = e.NewTextValue;

            Password = newPassword;

            if (newPassword.Length >= Length)
            {
                (sender as Entry).Unfocus();

                Completed?.Invoke(this, new PinCompletedEventArgs(Password));
            }
        }

        async void OnInternalEntryFocused(object sender, FocusEventArgs e)
        {
            Focused?.Invoke(this, EventArgs.Empty);

            var length = string.IsNullOrEmpty(Password) ? 0 : Password.Length;

            _internalEntry.CursorPosition = length;

            if (!IsAnimated)
                return;

            var pinItems = _pinContainer.Children.Select(x => x as PinItem).ToArray();

            if (length == Length)
            {
                pinItems[length - 1].IsFocused = true;
                await Animation.OnFocus(pinItems[length - 1]);
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    if (i == length)
                    {
                        pinItems[i].IsFocused = true;
                        await Animation.OnFocus(pinItems[i]);
                    }
                    else
                    {
                        pinItems[i].IsFocused = false;
                        await Animation.OnUnfocus(pinItems[i]);
                    }
                }
            }
        }

        async void OnInternalEntryUnfocused(object sender, FocusEventArgs e)
        {
            Unfocused?.Invoke(this, EventArgs.Empty);

            if (!IsAnimated)
                return;

            var pinItems = _pinContainer.Children.Select(x => x as PinItem).ToArray();

            for (int i = 0; i < pinItems.Count(); i++)
            {
                pinItems[i].IsFocused = false;
                await Animation.OnUnfocus(pinItems[i]);
            }
        }
    }
}