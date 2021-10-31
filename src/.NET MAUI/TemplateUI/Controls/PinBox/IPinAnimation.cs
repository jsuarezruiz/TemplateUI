using System.Threading.Tasks;

namespace TemplateUI.Controls
{
    public interface IPinAnimation
    {
        Task OnFocus(PinItem pinItem);
        Task OnUnfocus(PinItem pinItem);
    }
}