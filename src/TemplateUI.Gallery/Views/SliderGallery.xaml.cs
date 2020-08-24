using System.Diagnostics;
using Xamarin.Forms;

namespace TemplateUI.Gallery.Views
{
    public partial class SliderGallery : ContentPage
    {
        public SliderGallery()
        {
            InitializeComponent();
        }

        void OnSliderValueChanged(object sender, Controls.ValueChangedEventArgs e)
        {
            Debug.WriteLine($"Slider Value: {e.Value}");
        }
    }
}