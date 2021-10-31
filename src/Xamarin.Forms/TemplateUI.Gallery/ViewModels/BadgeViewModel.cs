using System.Windows.Input;
using Xamarin.Forms;

namespace TemplateUI.Gallery.ViewModels
{
    public class BadgeViewModel : BindableObject
    {
        int _counter;

        public BadgeViewModel()
        {
            Counter = 3;
        }

        public int Counter
        {
            get { return _counter; }
            set
            {
                _counter = value;
                OnPropertyChanged();
            }
        }

        public ICommand IncreaseCommand => new Command(Increase);
        public ICommand DecreaseCommand => new Command(Decrease);

        void Increase()
        {
            Counter++;
        }

        void Decrease()
        {
            if (Counter == 0)
                return;

            Counter--;
        }
    }
}
