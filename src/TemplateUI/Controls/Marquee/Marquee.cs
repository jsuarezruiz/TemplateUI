using System;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    [ContentProperty(nameof(Content))]
    public class Marquee : TemplatedView
    {
        const double DefaultSpeed = 50;

        const string ElementBorder = "PART_Border";
        const string ElementContent = "PART_Content";

        Frame _border;
        View _content;

        public Marquee()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(DefaultSpeed / AnimationSpeed), OnTimerTick);
        }

        public static readonly BindableProperty MarqueeDirectionProperty =
            BindableProperty.Create(nameof(Direction), typeof(MarqueeDirection), typeof(Marquee), MarqueeDirection.LeftToRight);

        public MarqueeDirection Direction
        {
            get => (MarqueeDirection)GetValue(MarqueeDirectionProperty);
            set => SetValue(MarqueeDirectionProperty, value);
        }

        public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(Marquee), null,
                propertyChanged: OnContentChanged);

        static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as Marquee)?.UpdateContent();
        }

        public View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(Marquee), string.Empty);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static new readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(Marquee), Color.Default);

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(Marquee), Color.Default);

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(Marquee), 0.0d);

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly BindableProperty AnimationSpeedProperty =
            BindableProperty.Create(nameof(AnimationSpeed), typeof(double), typeof(Marquee), 1.0d,
                propertyChanged: OnAnimationSpeedChanged);

        static void OnAnimationSpeedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as Marquee)?.UpdateAnimationSpeed();
        }

        public double AnimationSpeed
        {
            get => (double)GetValue(AnimationSpeedProperty);
            set => SetValue(AnimationSpeedProperty, value);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _border = GetTemplateChild(ElementBorder) as Frame;
            _content = GetTemplateChild(ElementContent) as View;
        }

        void UpdateContent()
        {
            if (_content == null)
                return;

            var presenter = _content as ContentPresenter;

            if(presenter?.Content is Label label)
            {
                label.LineBreakMode = LineBreakMode.NoWrap;
                label.MaxLines = 1;
                label.VerticalOptions = LayoutOptions.Center;
            }
        }

        void UpdateAnimationSpeed()
        {
            // TODO: Cancel previous animation.
            Device.StartTimer(TimeSpan.FromMilliseconds(DefaultSpeed / AnimationSpeed), OnTimerTick);
        }

        bool OnTimerTick()
        {
            if (Direction == MarqueeDirection.RightToLeft)
            {
                _content.TranslationX -= 5f;

                if (Math.Abs(_content.TranslationX) > Width)
                    _content.TranslationX = _content.Width;
            }
            else
            {
                _content.TranslationX += 5f;

                if (Math.Abs(_content.TranslationX) > Width)
                    _content.TranslationX = -_content.Width;
            }

            return IsEnabled;
        }
    }
}