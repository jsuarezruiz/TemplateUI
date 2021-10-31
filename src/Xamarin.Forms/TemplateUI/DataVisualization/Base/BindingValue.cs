using System.ComponentModel;
using Xamarin.Forms;

namespace TemplateUI.DataVisualization
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class BindingValue : BindableObject
    {
        readonly string _propertyPath;

        public BindingValue(string propertyPath)
        {
            _propertyPath = propertyPath;
        }

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create("Value", typeof(object), typeof(BindingValue), null);

        public object Eval(object source)
        {
            ClearValue(ValueProperty);

            var binding = new Binding
            {
                Path = _propertyPath,
                Mode = BindingMode.OneTime,
                Source = source
            };

            SetBinding(ValueProperty, binding);

            return GetValue(ValueProperty);
        }
    }
}