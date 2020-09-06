using System.Runtime.CompilerServices;
using TemplateUI.Layouts;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class Clock : TemplatedView
    {
        const string ElementNumPanel = "PART_NumPanel";
        const string ElementHand = "PART_Hand";

        CircularLayout _circularLayout;
        BoxView _hand;

        public Clock()
        {
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

            _circularLayout = GetTemplateChild(ElementNumPanel) as CircularLayout;
            _hand = GetTemplateChild(ElementHand) as BoxView;

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
        }

        ToggleView CreateNumItem(string content)
        {
            var toggleView = new ToggleView
            {
                CornerRadius = 24,
                UnCheckedColor = Color.Transparent,
                CheckedColor = Color
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
            if(IsEnabled)
            {
                var panGestureRecognizer = new PanGestureRecognizer();
                panGestureRecognizer.PanUpdated += OnPanUpdated;
                _hand.GestureRecognizers.Add(panGestureRecognizer);
            }
            else
            {
                _hand.GestureRecognizers.Clear();
            }
        }

        void UpdateColor()
        {
            foreach(var children in _circularLayout.Children)
            {
                if (children is ToggleView toggleView)
                    toggleView.CheckedColor = Color;
            }
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
       
        }
    }
}