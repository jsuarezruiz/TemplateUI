using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace TemplateUI.Controls
{
    public class PinAnimation : IPinAnimation
    {
        public async Task OnFocus(PinItem pinItem)
        {
            if (pinItem is VisualElement visualElement)
                await visualElement.ScaleTo(1.1, 150);
        }

        public async Task OnUnfocus(PinItem pinItem)
        {
            if (pinItem is VisualElement visualElement)
                await visualElement.ScaleTo(1.0, 100);
        }
    }
}