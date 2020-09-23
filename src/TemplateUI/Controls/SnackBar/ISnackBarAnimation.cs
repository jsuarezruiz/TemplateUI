using System.Threading.Tasks;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public interface ISnackBarAnimation
    {
        Task OnOpen(View snackBar);
        Task OnClose(View snackBar);
    }
}