using System;

namespace TemplateUI.Controls
{
    public class PositionChangedEventArgs : EventArgs
    {
        public PositionChangedEventArgs(int position)
        {
            CurrentPosition = position;
        }

        public int CurrentPosition { get; set; }
    }
}