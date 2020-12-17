using System;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class RadialPicker : AbsoluteLayout
    {
        public static BindableProperty SelectedColorProperty =
            BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(OpacityGradientLayout), Color.White, defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnColorChanged);

        static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialPicker opacityGradientLayout)
            {
                opacityGradientLayout.ValueChanged?.Invoke(bindable, null);
                opacityGradientLayout.SelectedColor = (Color)newValue;
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public RadialPicker()
        {
        }
    }
}
