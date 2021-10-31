using Xamarin.Forms;

namespace TemplateUI.Gallery.Views
{
    public partial class CustomNavigationPage : NavigationPage
    {
        public CustomNavigationPage()
        {
            InitializeComponent();
        }

        public CustomNavigationPage(ContentPage root) : base(root) 
        {
            InitializeComponent();
        }
    }
}