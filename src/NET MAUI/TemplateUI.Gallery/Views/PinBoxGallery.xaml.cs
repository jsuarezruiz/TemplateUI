using TemplateUI.Controls;
using Microsoft.Maui.Controls;

namespace TemplateUI.Gallery.Views
{
    public partial class PinBoxGallery : TabbedPage
    {
        public PinBoxGallery()
        {
            InitializeComponent();
        }

        void OnPinBoxCompleted(object sender, PinCompletedEventArgs e)
        {
            DisplayAlert("PinBox", $"Pin completed: {e.Password}", "Ok");
        }
    }
}