using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class Shield : TemplatedView
    {
        const string ElementBorder = "PART_Border";

        Frame _border;

        public static readonly BindableProperty SubjectProperty =
              BindableProperty.Create(nameof(Subject), typeof(string), typeof(Shield), string.Empty);

        public string Subject
        {
            get => (string)GetValue(SubjectProperty);
            set => SetValue(SubjectProperty, value);
        }

        public static readonly BindableProperty StatusProperty =
            BindableProperty.Create(nameof(Status), typeof(string), typeof(Shield), string.Empty);

        public string Status
        {
            get => (string)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create(nameof(Color), typeof(Color), typeof(Shield), Color.Default);

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(Shield), Color.Default);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(Shield), null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(Shield), null);

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public event EventHandler Tapped;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _border = GetTemplateChild(ElementBorder) as Frame;

            UpdateIsEnabled(); 
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
        }

        void UpdateIsEnabled()
        {
            if (IsEnabled)
            {
                _border.Opacity = 1.0d;

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += OnCloseButtonTapped;
                _border.GestureRecognizers.Add(tapGestureRecognizer);
            }
            else
            {
                _border.Opacity = 0.4d;

                // TODO: Remove only specific gestures.
                _border.GestureRecognizers.Clear();
            }


            void OnCloseButtonTapped(object sender, EventArgs e)
            {
                Tapped?.Invoke(this, EventArgs.Empty);
                Command?.Execute(CommandParameter);
            }
        }
    }
}