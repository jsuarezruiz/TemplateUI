using System.Threading.Tasks;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public interface IBadgeAnimation
    {
        Task OnAppearing(View badgeView);
        Task OnDisappering(View badgeView);
    }
}