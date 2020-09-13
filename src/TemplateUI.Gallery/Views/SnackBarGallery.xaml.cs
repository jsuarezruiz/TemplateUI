using System;
using Xamarin.Forms;

namespace TemplateUI.Gallery.Views
{
    public partial class SnackBarGallery : ContentPage
    {
        public SnackBarGallery()
        {
            InitializeComponent();
        }

        void OnShowDefaultSnackBarClicked(object sender, EventArgs e)
        {
            DefaultSnackBar.IsOpen = true;
        }

        void OnShowCustomColorsSnackBarClicked(object sender, EventArgs e)
        {
            CustomColorsSnackBar.IsOpen = true;
        }

        void OnShowCornerRadiusSnackBarClicked(object sender, EventArgs e)
        {
            CornerRadiusSnackBar.IsOpen = true;
        }
    }
}