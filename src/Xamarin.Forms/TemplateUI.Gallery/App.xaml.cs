using TemplateUI.Gallery.Views;
using Xamarin.Forms;

namespace TemplateUI.Gallery
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            TemplateUI.Init();

            MainPage = new CustomNavigationPage(new MainView());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}