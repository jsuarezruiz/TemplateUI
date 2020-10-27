using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class OpacityGradientLayout : AbsoluteLayout
    {
        public static BindableProperty SelectedColorProperty =
            BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(OpacityGradientLayout), Color.White, defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnColorChanged);

        static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is OpacityGradientLayout opacityGradientLayout)
            {
                opacityGradientLayout.ValueChanged?.Invoke(bindable, null);
                opacityGradientLayout.SelectedColor = (Color)newValue;
                //opacityGradientLayout.OnPropertyChanged();
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public GradientColorStackMode Mode { get; set; }
    }
}
