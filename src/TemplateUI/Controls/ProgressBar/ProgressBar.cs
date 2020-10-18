using System;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    // TODO: Add Indeterminate State
    public class ProgressBar : TemplatedView
    {
        const string ElementBackground = "PART_Background";
        const string ElementProgress = "PART_Progress";
        const string ElementText = "PART_Text";

        View _background;
        View _progress;
        Label _text;

        public ProgressBar()
        {

        }

        public static readonly BindableProperty ProgressProperty =
            BindableProperty.Create(nameof(Progress), typeof(double), typeof(ProgressBar), 0.0d,
                propertyChanged: OnValueChanged);

        public double Progress
        {
            get => (double)GetValue(ProgressProperty);
            set { SetValue(ProgressProperty, value); }
        }

        static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ProgressBar)?.UpdateProgress();
        }

        public static new readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(ProgressBar), Color.Default);

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly BindableProperty ProgressColorProperty =
            BindableProperty.Create(nameof(ProgressColor), typeof(Color), typeof(ProgressBar), Color.Default);

        public Color ProgressColor
        {
            get => (Color)GetValue(ProgressColorProperty);
            set { SetValue(ProgressColorProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(ProgressBar), Color.Default);

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(ProgressBar), string.Empty);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ProgressBar), Color.Default);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(ProgressBar), 0.0d);

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set { SetValue(CornerRadiusProperty, value); }
        }

        internal static readonly BindablePropertyKey PercentagePropertyKey =
            BindableProperty.CreateReadOnly(nameof(Percentage), typeof(double), typeof(ProgressBar), 0.0d);

        public static readonly BindableProperty PercentageProperty = PercentagePropertyKey.BindableProperty;

        public double Percentage
        {
            get
            {
                return (double)GetValue(PercentageProperty);
            }
            private set
            {
                SetValue(PercentagePropertyKey, value);
            }
        }

        public static readonly BindableProperty RenderModeProperty =
           BindableProperty.Create(nameof(RenderMode), typeof(RenderMode), typeof(ProgressBar), RenderMode.Native,
               propertyChanged: OnRenderModeChanged);

        static void OnRenderModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ProgressBar)?.UpdateControlTemplate();
        }

        public RenderMode RenderMode
        {
            get => (RenderMode)GetValue(RenderModeProperty);
            set { SetValue(RenderModeProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _background = GetTemplateChild(ElementBackground) as View;
            _progress = GetTemplateChild(ElementProgress) as View;
            _text = GetTemplateChild(ElementText) as Label;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            UpdateProgress();
        }

        void UpdateProgress()
        {
            var percentage = _progress.WidthRequest = Math.Floor(_background.Width * Progress);

            var textTranslationX = percentage / 2 - _text.X / 2;

            if (textTranslationX <= 0)
            {
                textTranslationX = _background.Width / 2 - _text.X;
                _text.TextColor = TextColor;
            }
            else
            {
                textTranslationX = percentage / 2 - _text.X;
                _text.TextColor = BackgroundColor;
            }

            _text.TranslationX = textTranslationX;

            Percentage = Math.Round(Progress * 100, 1);
        }

        void UpdateControlTemplate()
        {
            var template = TemplateBuilder.GetControlTemplate(GetType().Name, RenderMode, null);
            Application.Current.Resources.TryGetValue(template, out object controlTemplate);

            if (controlTemplate == null)
                throw new ArgumentNullException("To use Skia RenderMode you must use TemplateUISkia.Init(); in your Xamarin.Forms Application class.");

            ControlTemplate = controlTemplate as ControlTemplate;
        }
    }
}