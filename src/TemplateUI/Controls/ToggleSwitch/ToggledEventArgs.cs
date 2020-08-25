using System;

namespace TemplateUI.Controls
{
    public class ToggledEventArgs : EventArgs
    {
        public ToggledEventArgs(bool toggled)
        {
            Toggled = toggled;
        }

        public bool Toggled { get; set; }
    }
}
