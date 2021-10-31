using System;

namespace TemplateUI.Controls
{
    public class ValueChangedEventArgs : EventArgs
    {
        public ValueChangedEventArgs(double value)
        {
            Value = value;
        }

        public double Value { get; set; }
    }
}