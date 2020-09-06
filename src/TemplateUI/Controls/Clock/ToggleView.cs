using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    [ContentProperty(nameof(Content))]
    public class ToggleView : TemplatedView
    {
        public static readonly BindableProperty CheckedProperty =
            BindableProperty.Create(nameof(Checked), typeof(bool), typeof(ToggleView), false, BindingMode.TwoWay,
                null, propertyChanged: OnCheckedChanged);

        static async void OnCheckedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            await (bindable as ToggleView)?.UpdateCheckedAsync();
        }

        public bool Checked
        {
            get { return (bool)GetValue(CheckedProperty); }
            set { SetValue(CheckedProperty, value); }
        }

        public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(ToggleView), null);

        public View Content
        {
            get { return (View)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly BindableProperty CheckedColorProperty =
            BindableProperty.Create(nameof(CheckedColor), typeof(Color), typeof(ToggleView), Color.Default,
                propertyChanged: OnCurrentChanged);

        public Color CheckedColor
        {
            get { return (Color)GetValue(CheckedColorProperty); }
            set { SetValue(CheckedColorProperty, value); }
        }

        public static readonly BindableProperty UnCheckedColorProperty =
            BindableProperty.Create(nameof(UnCheckedColor), typeof(Color), typeof(ToggleView), Color.Default,
                propertyChanged: OnCurrentChanged);

        public Color UnCheckedColor
        {
            get { return (Color)GetValue(UnCheckedColorProperty); }
            set { SetValue(UnCheckedColorProperty, value); }
        }

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(ToggleView), 0.0d);

        static void OnCurrentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ToggleView)?.UpdateCurrent();
        }

        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty IsAnimatedProperty =
            BindableProperty.Create(nameof(IsAnimated), typeof(bool), typeof(ToggleView), false);

        public bool IsAnimated
        {
            get { return (bool)GetValue(IsAnimatedProperty); }
            set { SetValue(IsAnimatedProperty, value); }
        }

        internal static readonly BindablePropertyKey CurrentColorPropertyKey =
            BindableProperty.CreateReadOnly(nameof(CurrentColor), typeof(Color), typeof(ToggleView), Color.Default);

        public static readonly BindableProperty CurrentColorProperty = CurrentColorPropertyKey.BindableProperty;

        public Color CurrentColor
        {
            get
            {
                return (Color)GetValue(CurrentColorProperty);
            }
            private set
            {
                SetValue(CurrentColorPropertyKey, value);
            }
        }

        protected override async void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            UpdateIsEnabled();
            await UpdateCheckedAsync();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
        }

        async Task UpdateCheckedAsync()
        {
            UpdateCurrent();

            if (IsAnimated && Content != null)
            {
                await Content.ScaleTo(0.9, 50, Easing.Linear);
                await Task.Delay(100);
                await Content.ScaleTo(1, 50, Easing.Linear);
            }
        }

        void UpdateCurrent()
        {
            CurrentColor = !Checked || CheckedColor == Color.Default ? UnCheckedColor : CheckedColor;
        }

        void UpdateIsEnabled()
        {
            if(IsEnabled)
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += OnTapped;
                GestureRecognizers.Add(tapGestureRecognizer);
            }
            else
            {
                GestureRecognizers.Clear();
            }
        }

        void OnTapped(object sender, EventArgs e)
        {
            Checked = !Checked;
        }
    }
}