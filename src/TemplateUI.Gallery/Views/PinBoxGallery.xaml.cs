using TemplateUI.Controls;
using Xamarin.Forms;

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