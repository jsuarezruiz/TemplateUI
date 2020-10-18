using System;
using System.Diagnostics;
using TemplateUI.Controls;
using Xamarin.Forms;

namespace TemplateUI.Gallery.Views
{
    public partial class ToggleSwitchGallery : TabbedPage
    {
        public ToggleSwitchGallery()
        {
            InitializeComponent();
        }

        void OnNativeRenderModeClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PerformanceToggleSwitchGallery(RenderMode.Native));
        }

        void OnSkiaRenderModeClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PerformanceToggleSwitchGallery(RenderMode.Skia));
        }
    }

    public class PerformanceToggleSwitchGallery : ContentPage
    {
        readonly Stopwatch _stopwatch;

        public PerformanceToggleSwitchGallery(RenderMode renderMode, int numberOfViews = 10)
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            Title = "Performance ToggleSwitch Gallery";

            var layout = new StackLayout();

            for(int i = 0; i < numberOfViews; i++)
            {
                var toggleSwitch = new ToggleSwitch
                {
                    VisualType = VisualType.Material,
                    RenderMode = renderMode
                };

                layout.Children.Add(toggleSwitch);
            }

            Content = layout;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _stopwatch.Stop();

            Debug.WriteLine($"{_stopwatch.ElapsedMilliseconds} ms");
        }
    }
}