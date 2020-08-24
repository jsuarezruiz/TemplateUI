using System.Collections.ObjectModel;
using TemplateUI.Gallery.Models;
using Xamarin.Forms;

namespace TemplateUI.Gallery.Views
{
    public partial class DataVisualizationGallery : TabbedPage
    {
        public DataVisualizationGallery()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public ObservableCollection<ChartDataItem> Data { get; } = new ObservableCollection<ChartDataItem>()
        {
            new ChartDataItem() { Title = "Jan", Value = 10 },
            new ChartDataItem() { Title = "Feb", Value = 60 },
            new ChartDataItem() { Title = "Mar", Value = 40 },
            new ChartDataItem() { Title = "Apr", Value = 125 },
            new ChartDataItem() { Title = "May", Value = 80 },
            new ChartDataItem() { Title = "Jun", Value = 50 },
            new ChartDataItem() { Title = "Jul", Value = 60 },
            new ChartDataItem() { Title = "Aug", Value = 30 },
            new ChartDataItem() { Title = "Sep", Value = 140 },
            new ChartDataItem() { Title = "Oct", Value = 80 },
            new ChartDataItem() { Title = "Nov", Value = 50 },
            new ChartDataItem() { Title = "Dec", Value = 70 }
        };
    }
}