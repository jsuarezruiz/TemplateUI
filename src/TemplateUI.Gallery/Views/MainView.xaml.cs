using System;
using Xamarin.Forms;

namespace TemplateUI.Gallery.Views
{
    public partial class MainView : ContentPage
    {
        public MainView()
        {
            InitializeComponent();
        }

        void OnAvatarViewClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AvatarViewGallery());
        }

        void OnBadgeViewClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BadgeViewGallery());
        }

        void OnCarouselViewClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CarouselViewGallery());
        }

        void OnDataVisualizationClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DataVisualizationGallery());
        }

        void OnGridSplitterClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GridSplitterGallery());
        }

        void OnMarqueeClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MarqueeGallery());
        }

        void OnRateClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RateGallery());
        }

        void OnSegmentedControlClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SegmentedControlGallery());
        }
    }
}