using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace TemplateUI.Controls
{
    public interface ISnackBarAnimation
    {
        Task OnOpen(View snackBar);
        Task OnClose(View snackBar);
    }
}