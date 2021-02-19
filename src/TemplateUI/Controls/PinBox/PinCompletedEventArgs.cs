using System;

namespace TemplateUI.Controls
{
    public class PinCompletedEventArgs : EventArgs
    {
        public PinCompletedEventArgs(string password)
        {
            Password = password;
        }

        public string Password { get; set; }
    }
}