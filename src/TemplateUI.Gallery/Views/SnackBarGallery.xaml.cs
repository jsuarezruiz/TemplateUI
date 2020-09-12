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

        void OnShowSnackBarClicked(object sender, EventArgs e)
        {
            SnackBar.IsOpen = true;
        }
    }
}