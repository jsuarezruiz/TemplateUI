using System;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace TemplateUI.Controls
{
    public class ChatBubble : TemplatedView
    {
        const string ElementContainer = "PART_Container";
        const string ElementTail = "PART_Tail";
        const string ElementBody = "PART_Body";
        const string ElementText = "PART_Text";

        Grid _container;
        Shape _tail;
        Frame _body;
        View _text;

        public static readonly BindableProperty BubbleTypeProperty =
           BindableProperty.Create(nameof(BubbleType), typeof(ChatBubbleType), typeof(ChatBubble), ChatBubbleType.Sender,
               propertyChanged: OnBubbleTypeChanged);

        static void OnBubbleTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ChatBubble)?.UpdateBubbleType();
        }

        public ChatBubbleType BubbleType
        {
            get => (ChatBubbleType)GetValue(BubbleTypeProperty);
            set { SetValue(BubbleTypeProperty, value); }
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(ChatBubble), string.Empty);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty ContentProperty =
           BindableProperty.Create(nameof(Content), typeof(View), typeof(ChatBubble), null,
               propertyChanged: OnContentChanged);

        static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ChatBubble)?.UpdateTemplate();
        }

        public View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static new readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(ChatBubble), Color.Default);

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ChatBubble), Color.Default);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(ChatBubble), 12.0d);

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(ChatBubble), FontAttributes.None);

        public FontAttributes FontAttributes
        {
            get => (FontAttributes)GetValue(FontAttributesProperty);
            set { SetValue(FontAttributesProperty, value); }
        }

        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ChatBubble), string.Empty);

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set { SetValue(FontFamilyProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _container = GetTemplateChild(ElementContainer) as Grid;
            _tail = GetTemplateChild(ElementTail) as Shape;
            _body = GetTemplateChild(ElementBody) as Frame;
            _text = GetTemplateChild(ElementText) as View;

            UpdateTemplate();
            UpdateBubbleType();
        }

        void UpdateTemplate()
        {
            if (Content != null)
            {
                Application.Current.Resources.TryGetValue("ContentChatBubbleControlTemplate", out object contentChatBubbleControlTemplate);
                ControlTemplate = contentChatBubbleControlTemplate as ControlTemplate;
            }
            else
            {
                Application.Current.Resources.TryGetValue("TextChatBubbleControlTemplate", out object textChatBubbleControlTemplate);
                ControlTemplate = textChatBubbleControlTemplate as ControlTemplate;
            }
        }

        void UpdateBubbleType()
        {
            _container.WidthRequest = _text.Width;

            if (BubbleType == ChatBubbleType.Sender)
            {
                _container.HorizontalOptions = LayoutOptions.Start;
                _body.Margin = new Thickness(3, 0, 0, 0);
                _tail.HorizontalOptions = LayoutOptions.Start;
                _tail.RotationY = 180;
            }
            else
            {
                _container.HorizontalOptions = LayoutOptions.End;
                _body.Margin = new Thickness(0, 0, 3, 0);
                _tail.HorizontalOptions = LayoutOptions.End;
                _tail.RotationY = 0;
            }
        }
    }
}