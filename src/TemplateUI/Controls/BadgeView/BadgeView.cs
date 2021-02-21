using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    [ContentProperty(nameof(Content))]
    public class BadgeView : TemplatedView
    {
        const string ElementIndicator = "PART_Indicator";
        const string ElementIndicatorBackground = "PART_IndicatorBackground";
        const string ElementText = "PART_Text";
        const string ElementContent = "Part_Content";

        Grid _indicatorContainer;
        Frame _indicatorBackground;
        Label _text;
        View _content;
        bool _isVisible;
        bool _placementDone;

        public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(BadgeView), null);

        public View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly BindableProperty BadgePositionProperty =
            BindableProperty.Create(nameof(BadgePosition), typeof(BadgePosition), typeof(BadgeView), BadgePosition.TopRight,
                propertyChanged: OnBadgePositionChanged);

        public BadgePosition BadgePosition
        {
            get => (BadgePosition)GetValue(BadgePositionProperty);
            set => SetValue(BadgePositionProperty, value);
        }

        static void OnBadgePositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as BadgeView)?.UpdateBadgeViewPlacement(true);
        }

        public static BindableProperty AutoHideProperty =
            BindableProperty.Create(nameof(AutoHide), typeof(bool), typeof(BadgeView), defaultValue: true,
                propertyChanged: OnAutoHideChanged);

        public bool AutoHide
        {
            get { return (bool)GetValue(AutoHideProperty); }
            set { SetValue(AutoHideProperty, value); }
        }

        static async void OnAutoHideChanged(BindableObject bindable, object oldValue, object newValue)
        {
            await (bindable as BadgeView)?.UpdateVisibilityAsync();
        }

        public static BindableProperty IsAnimatedProperty =
           BindableProperty.Create(nameof(IsAnimated), typeof(bool), typeof(BadgeView), defaultValue: true);

        public bool IsAnimated
        {
            get { return (bool)GetValue(IsAnimatedProperty); }
            set { SetValue(IsAnimatedProperty, value); }
        }

        public static BindableProperty BadgeAnimationProperty =
            BindableProperty.Create(nameof(BadgeAnimation), typeof(IBadgeAnimation), typeof(BadgeView), new BadgeAnimation());

        public IBadgeAnimation BadgeAnimation
        {
            get { return (IBadgeAnimation)GetValue(BadgeAnimationProperty); }
            set { SetValue(BadgeAnimationProperty, value); }
        }

        public new static BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(BadgeView), defaultValue: Color.Default);

        public new Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(BadgeView), Color.Default);

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty HasShadowProperty =
          BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(BadgeView), false);

        public bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }

        public static BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(BadgeView), defaultValue: Color.Default);

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public static BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(BadgeView), defaultValue: "0",
                propertyChanged: OnTextChanged);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        static async void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            await (bindable as BadgeView)?.UpdateVisibilityAsync();
        }

        public static BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(BadgeView), 10.0d);

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(BadgeView), string.Empty);

        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public static BindableProperty FontAttributesProperty =
            BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(BadgeView), FontAttributes.None);

        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)GetValue(FontAttributesProperty); }
            set { SetValue(FontAttributesProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _indicatorContainer = GetTemplateChild(ElementIndicator) as Grid;
            _indicatorBackground = GetTemplateChild(ElementIndicatorBackground) as Frame;
            _text = GetTemplateChild(ElementText) as Label;
            _content = GetTemplateChild(ElementContent) as View;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            SetInheritedBindingContext(Content, BindingContext);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            UpdateBadgeViewPlacement();
        }

        void UpdateBadgeViewPlacement(bool force = false)
        {
            if (_content.Height <= 0 && _content.Width <= 0)
                return;

            if (force)
                _placementDone = false;

            if (_placementDone)
                return;

            const double Padding = 6;

            double size = Math.Max(_text.Height, _text.Width) + Padding;

            _indicatorBackground.HeightRequest = size / 1.5;

            double verticalMargin;
            double horizontalMargin;

            switch (BadgePosition)
            {
                case BadgePosition.TopLeft:
                    verticalMargin = size / 2;
                    horizontalMargin = _content.Width + verticalMargin;
                    _indicatorContainer.Margin = new Thickness(0, 0, horizontalMargin, 0);
                    _content.Margin = new Thickness(verticalMargin, verticalMargin, 0, 0);
                    break;
                case BadgePosition.TopRight:
                    verticalMargin = size / 2;
                    horizontalMargin = _content.Width - verticalMargin;
                    _indicatorContainer.Margin = new Thickness(horizontalMargin, 0, 0, 0);
                    _content.Margin = new Thickness(0, verticalMargin, 0, 0);
                    break;
                case BadgePosition.BottomLeft:
                    verticalMargin = size / 2;
                    double bottomLeftverticalMargin = _content.Height - verticalMargin;
                    _indicatorContainer.Margin = new Thickness(0, bottomLeftverticalMargin, 0, 0);
                    _content.Margin = new Thickness(verticalMargin, 0, 0, 0);
                    break;
                case BadgePosition.BottomRight:
                    verticalMargin = size / 2;
                    double bottomRightverticalMargin = _content.Height - verticalMargin;
                    horizontalMargin = _content.Width - verticalMargin;
                    _indicatorContainer.Margin = new Thickness(horizontalMargin, bottomRightverticalMargin, 0, 0);
                    _content.Margin = new Thickness(0, 0, 0, verticalMargin);
                    break;
            }

            _placementDone = true;
        }

        async Task UpdateVisibilityAsync()
        {
            if (_indicatorContainer == null)
                return;

            string badgeText = _text.Text;

            if (string.IsNullOrEmpty(badgeText))
            {
                IsVisible = false;
                return;
            }

            bool badgeIsVisible = !AutoHide || !badgeText.Trim().Equals("0");

            if (IsAnimated)
            {
                if (badgeIsVisible == _isVisible)
                    return;

                if (badgeIsVisible)
                {
                    _indicatorContainer.IsVisible = true;
                    await BadgeAnimation.OnAppearing(_indicatorContainer);
                }
                else
                {
                    await BadgeAnimation.OnDisappering(_indicatorContainer);
                    _indicatorContainer.IsVisible = false;
                }

                _isVisible = badgeIsVisible;
            }
            else
                _indicatorContainer.IsVisible = badgeIsVisible;
        }
    }
}