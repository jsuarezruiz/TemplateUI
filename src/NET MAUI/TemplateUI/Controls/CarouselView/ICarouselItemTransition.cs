using System.Threading.Tasks;

namespace TemplateUI.Controls
{
    public interface ICarouselItemTransition
    {
        Task OnSelectionChanging(SelectionChangingArgs args);
        Task OnSelectionChanged(SelectionChangedArgs args);
    }
}