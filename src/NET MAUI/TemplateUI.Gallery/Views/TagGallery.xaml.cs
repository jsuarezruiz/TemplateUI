using System;
using Microsoft.Maui.Controls;

namespace TemplateUI.Gallery.Views
{
    public partial class TagGallery : TabbedPage
    {
        public TagGallery()
        {
            InitializeComponent();
        }

        void OnTagSelected(object sender, EventArgs e)
        {
            DisplayAlert("Tag", "Selected Event", "Ok");
        }

        void OnTagClosed(object sender, EventArgs e)
        {
            DisplayAlert("Tag", "Closed Event", "Ok");
        }
    }
}