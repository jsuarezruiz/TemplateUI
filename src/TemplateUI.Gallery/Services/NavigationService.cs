using System.Threading.Tasks;
using Xamarin.Forms;

namespace TemplateUI.Gallery.Services
{
    public class NavigationService
    {
        static NavigationService _instance;

        public static NavigationService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NavigationService();
                return _instance;
            }
        }

        public async Task NavigateAsync(Page page)
        {
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}
