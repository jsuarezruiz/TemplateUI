using System;
using Xamarin.Forms;

namespace TemplateUI.Gallery.Views
{
    public partial class ShieldGallery : TabbedPage
    {
        public ShieldGallery()
        {
            InitializeComponent();
        }

        void OnShieldTapped(object sender, EventArgs e)
        {
            DisplayAlert("Shield Event", "C# Shield Tapped", "Ok");
        }
    }
}