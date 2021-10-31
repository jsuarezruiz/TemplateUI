using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace TemplateUI.Gallery.Services
{
    public class DialogService
    {
        static DialogService _instance;

        public static DialogService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DialogService();
                return _instance;
            }
        }

        public async Task ShowInfoAlert(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "Ok");
        }
    }
}
