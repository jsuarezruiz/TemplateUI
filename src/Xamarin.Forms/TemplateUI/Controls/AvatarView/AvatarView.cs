using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace TemplateUI.Controls
{
    // TODO: Add Predefined Symbols (Dot, Busy, etc)
    public class AvatarView : TemplatedView
    {
        const string ElementContainer = "Part_Container";
        const string ElementImage = "Part_Image";
        const string ElementImageGeometry = "Part_ImageClip";
        const string ElementInitials = "Part_Initials";
        const string ElementBorder = "Part_Border";

        const double ExtraLargeSize = 64;
        const double LargeSize = 48;
        const double MediumSize = 36;
        const double SmallSize = 24;
        const double ExtraSmallSize = 18;

        const double ExtraLargeFontSize = 24;
        const double LargeFontSize = 18;
        const double MediumFontSize = 12;
        const double SmallFontSize = 10;
        const double ExtraSmallFontSize = 8;

        Grid _container;
        Image _image;
        Geometry _imageClip;
        Label _initials;
        Shape _border;

        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(AvatarView), null);

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public static readonly BindableProperty AvatarSizeProperty =
            BindableProperty.Create(nameof(AvatarSize), typeof(AvatarSize), typeof(AvatarView), AvatarSize.Medium);

        public AvatarSize AvatarSize
        {
            get => (AvatarSize)GetValue(AvatarSizeProperty);
            set => SetValue(AvatarSizeProperty, value);
        }

        public static readonly BindableProperty AvatarShapeProperty =
            BindableProperty.Create(nameof(AvatarShape), typeof(AvatarShape), typeof(AvatarView), AvatarShape.Circle);

        public AvatarShape AvatarShape
        {
            get => (AvatarShape)GetValue(AvatarShapeProperty);
            set => SetValue(AvatarShapeProperty, value);
        }

        public static readonly BindableProperty AvatarNameProperty =
            BindableProperty.Create(nameof(AvatarName), typeof(string), typeof(AvatarView), string.Empty);

        public string AvatarName
        {
            get => (string)GetValue(AvatarNameProperty);
            set => SetValue(AvatarNameProperty, value);
        }

        public static readonly BindableProperty AvatarSurNameProperty =
            BindableProperty.Create(nameof(AvatarSurName), typeof(string), typeof(AvatarView), string.Empty);

        public string AvatarSurName
        {
            get => (string)GetValue(AvatarSurNameProperty);
            set => SetValue(AvatarSurNameProperty, value);
        }

        public static new readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(AvatarView), Color.Default);

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(AvatarView), Color.Default);

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(nameof(BorderWidth), typeof(double), typeof(AvatarView), 1.0d);

        public double BorderWidth
        {
            get => (double)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(AvatarView), LargeFontSize);

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(AvatarView), string.Empty);

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(AvatarView), FontAttributes.None);

        public FontAttributes FontAttributes
        {
            get => (FontAttributes)GetValue(FontAttributesProperty);
            set => SetValue(FontAttributesProperty, value);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _container = GetTemplateChild(ElementContainer) as Grid;
            _image = GetTemplateChild(ElementImage) as Image;
            _imageClip = GetTemplateChild(ElementImageGeometry) as Geometry;
            _initials = GetTemplateChild(ElementInitials) as Label;
            _border = GetTemplateChild(ElementBorder) as Shape;

            UpdateAvatarSize();
            UpdateAvatarShape();
            UpdateInitials();
            UpdateBackgroundColor();
            UpdateSize();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == AvatarSizeProperty.PropertyName)
                UpdateAvatarSize();
            else if (propertyName == AvatarShapeProperty.PropertyName)
                UpdateAvatarShape();
            else if (propertyName == AvatarNameProperty.PropertyName || propertyName == AvatarSurNameProperty.PropertyName)
                UpdateInitials();
            else if (propertyName == AvatarSizeProperty.PropertyName)
                UpdateBackgroundColor();
            else if (propertyName == HeightProperty.PropertyName || propertyName == WidthProperty.PropertyName)
                UpdateSize();
        }

        void UpdateAvatarSize()
        {
            double size = MediumSize;
            double fontSize = MediumFontSize;

            switch (AvatarSize)
            {
                case AvatarSize.ExtraSmall:
                    size = ExtraSmallSize;
                    fontSize = ExtraSmallFontSize;
                    break;
                case AvatarSize.Small:
                    size = SmallSize;
                    fontSize = SmallFontSize;
                    break;
                case AvatarSize.Medium:
                    size = MediumSize;
                    fontSize = MediumFontSize;
                    break;
                case AvatarSize.Large:
                    size = LargeSize;
                    fontSize = LargeFontSize;
                    break;
                case AvatarSize.ExtraLarge:
                    size = ExtraLargeSize;
                    fontSize = ExtraLargeFontSize;
                    break;
            }

            _container.HeightRequest = _container.WidthRequest = size;
            _border.HeightRequest = _border.WidthRequest = size;

            if (AvatarShape == AvatarShape.Circle && _imageClip is EllipseGeometry ellipseGeometry)
            {
                ellipseGeometry.Center = new Point(size / 2, size / 2);
                ellipseGeometry.RadiusX = size / 2 - BorderWidth;
                ellipseGeometry.RadiusY = size / 2 - BorderWidth;
            }

            _initials.FontSize = fontSize;
        }

        void UpdateAvatarShape()
        {
            if (AvatarShape == AvatarShape.Circle)
            {
                Application.Current.Resources.TryGetValue("CircleAvatarViewTemplate", out object circleAvatarViewTemplate);
                ControlTemplate = circleAvatarViewTemplate as ControlTemplate;
            }
            else
            {
                Application.Current.Resources.TryGetValue("SquareAvatarViewTemplate", out object squareAvatarViewTemplate);
                ControlTemplate = squareAvatarViewTemplate as ControlTemplate;
            }
        }

        void UpdateInitials()
        {
            string name = $"{AvatarName} {AvatarSurName}";
            Regex extractInitials = new Regex(@"\s*([^\s])[^\s]*\s*");
            _initials.Text = extractInitials.Replace(name, "$1").ToUpper();
        }

        void UpdateBackgroundColor()
        {
            BackgroundColor = Color.Transparent;
            _container.BackgroundColor = Color.Transparent;
        }

        void UpdateSize()
        {
            if (!UseCustomSize())
                return;

            var height = Height;
            var width = Width;

            _container.HeightRequest = _container.WidthRequest = height;
            _border.HeightRequest = _border.WidthRequest = width;

            if (AvatarShape == AvatarShape.Circle && _imageClip is EllipseGeometry ellipseGeometry)
            {
                ellipseGeometry.Center = new Point(width / 2, height / 2);
                ellipseGeometry.RadiusX = height / 2 - BorderWidth;
                ellipseGeometry.RadiusY = width / 2 - BorderWidth;
            }

            _initials.FontSize = Math.Min(height, width) / 2;
        }

        bool UseCustomSize()
        {
            if (Height <= 0 || Width <= 0)
                return false;

            if (
                (Height == ExtraSmallSize && Width == ExtraSmallSize) ||
                (Height == SmallSize && Width == SmallSize) ||
                (Height == MediumSize && Width == MediumSize) ||
                (Height == LargeSize && Width == LargeSize) ||
                (Height == ExtraLargeSize && Width == ExtraLargeSize))
                return false;

            return true;
        }
    }
}