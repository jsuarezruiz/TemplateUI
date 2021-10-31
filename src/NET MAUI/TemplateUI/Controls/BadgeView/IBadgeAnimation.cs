using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace TemplateUI.Controls
{
    public interface IBadgeAnimation
    {
        Task OnAppearing(View badgeView);
        Task OnDisappering(View badgeView);
    }
}