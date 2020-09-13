using System;
using System.Runtime.CompilerServices;
using TemplateUI.Layouts;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class Clock : TemplatedView
    {
        const string ElementTime = "PART_Time";
        const string ElementNumPanel = "PART_NumPanel";
        const string ElementHandContainer = "PART_HandContainer";
        const string ElementHand = "PART_Hand";
        const string ElementButtomAm = "PART_ButtomAm";
        const string ElementButtomPm = "PART_ButtomPm";

        Label _time;
        CircularLayout _circularLayout;
        AbsoluteLayout _handContainer;
        BoxView _hand;
        ToggleView _buttonAm;
        ToggleView _buttonPm;

        Point _center;
        double _radius;

        public Clock()
        {

        }

        public static readonly BindableProperty SelectedTimeProperty =
            BindableProperty.Create(nameof(SelectedTime), typeof(DateTime), typeof(Clock), null);

        public DateTime SelectedTime
        {
            get => (DateTime)GetValue(SelectedTimeProperty);
            set { SetValue(SelectedTimeProperty, value); }
        }

        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create(nameof(Color), typeof(Color), typeof(Clock), Color.Black,
                propertyChanged: OnColorChanged);

        static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as Clock)?.UpdateColor();
        }

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set { SetValue(ColorProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_hand != null)
                _hand.SizeChanged -= OnHandSizeChanged;

            if (_buttonAm != null)
                _buttonAm.Toggled -= OnButtonAmToggled;

            if (_buttonPm != null)
                _buttonPm.Toggled -= OnButtonPmToggled;

            _time = GetTemplateChild(ElementTime) as Label;
            _circularLayout = GetTemplateChild(ElementNumPanel) as CircularLayout;
            _handContainer = GetTemplateChild(ElementHandContainer) as AbsoluteLayout;
            _hand = GetTemplateChild(ElementHand) as BoxView;
            _buttonAm = GetTemplateChild(ElementButtomAm) as ToggleView;
            _buttonPm = GetTemplateChild(ElementButtomPm) as ToggleView;

            if (_hand != null)
                _hand.SizeChanged += OnHandSizeChanged;

            if (_buttonAm != null)
                _buttonAm.Toggled += OnButtonAmToggled;

            if (_buttonPm != null)
                _buttonPm.Toggled += OnButtonPmToggled;

            AddNumItems();
            UpdateIsEnabled();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
        }

        void AddNumItems()
        {
            for (int i = 0; i < 12; i++)
            {
                ToggleView numItem = CreateNumItem($"{i + 1}");

                CircularLayout.SetRadius(numItem, 70);
                CircularLayout.SetAngle(numItem, 30 * (i + 1));

                _circularLayout.Children.Add(numItem);
            }

            RaiseChild(_circularLayout);
        }

        ToggleView CreateNumItem(string content)
        {
            var toggleView = new ToggleView
            {
                CornerRadius = 24,
                UnCheckedColor = Color.Transparent,
                CheckedColor = Color,
                IsAnimated = true
            };

            var label = new Label
            {
                TextColor = Color.Gray,
                Text = content,
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 18,
                WidthRequest = 18
            };

            toggleView.Content = label;

            return toggleView;
        }

        void UpdateIsEnabled()
        {
            if (IsEnabled)
            {
                var panGestureRecognizer = new PanGestureRecognizer();
                panGestureRecognizer.PanUpdated += OnPanUpdated;
                _handContainer.GestureRecognizers.Add(panGestureRecognizer);
            }
            else
            {
                _handContainer.GestureRecognizers.Clear();
            }
        }

        void UpdateColor()
        {
            foreach (var children in _circularLayout.Children)
            {
                if (children is ToggleView toggleView)
                    toggleView.CheckedColor = Color;
            }
        }

        void OnHandSizeChanged(object sender, EventArgs e)
        {
            _center = new Point();
            _radius = Math.Min(_circularLayout.Width, _circularLayout.Height) / 2;

            _hand.TranslationX = _handContainer.Width / 2;
            _hand.TranslationY = _handContainer.Height / 2;

            AbsoluteLayout.SetLayoutBounds(_hand, new Rectangle(_center.X - _hand.Width / 2, _center.Y - _radius, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        }

        async void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    var point = new Point(e.TotalX, e.TotalY);
                    var angle = Math.Atan2(point.Y - _center.Y, point.X - _center.X) * 180 / Math.PI;

                    _hand.Rotation = 0;
                    _hand.AnchorY = _radius / _hand.Height;
                    await _hand.RotateTo(angle, 0);
                    break;
            }
        }

        void OnButtonAmToggled(object sender, ToggledEventArgs e)
        {
            if(e.Toggled)
            {
                _buttonAm.Checked = true;
                _buttonPm.Checked = false;
            }
            else
            {
                _buttonAm.Checked = false;
                _buttonPm.Checked = true;
            }
        }

        void OnButtonPmToggled(object sender, ToggledEventArgs e)
        {
            if (e.Toggled)
            {
                _buttonAm.Checked = false;
                _buttonPm.Checked = true;
            }
            else
            {
                _buttonAm.Checked = true;
                _buttonPm.Checked = false;
            }
        }
    }
}