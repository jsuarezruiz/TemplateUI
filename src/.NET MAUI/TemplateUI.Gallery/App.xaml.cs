using TemplateUI.Gallery.Views;
using Application = Microsoft.Maui.Controls.Application;

namespace TemplateUI.Gallery
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainView();
        }
    }
}
