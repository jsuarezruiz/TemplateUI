using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Graphics;
using TemplateUI.Gallery.Models;
using TemplateUI.Gallery.Services;

namespace TemplateUI.Gallery.ViewModels
{
    public class MainViewModel : BindableObject
    {
        ObservableCollection<GalleryItem> _trending;
        ObservableCollection<GalleryItem> _gallery;

        public MainViewModel()
        {
            LoadData();
        }

        public ObservableCollection<GalleryItem> Trending
        {
            get { return _trending; }
            set
            {
                _trending = value;
                OnPropertyChanged();
            }
        }

        public ICommand TrendingCommand => new Command<GalleryItem>(NavigateToGallery);

        public ObservableCollection<GalleryItem> Gallery
        {
            get { return _gallery; }
            set
            {
                _gallery = value;
                OnPropertyChanged();
            }
        }

        public ICommand GalleryCommand => new Command<GalleryItem>(NavigateToGallery);

        public ICommand GitHubCommand => new Command(OpenGitHubCommand);

        void LoadData()
        {
            Trending = new ObservableCollection<GalleryItem>
            {
                new GalleryItem { Title = "DataVisualization", SubTitle = "Several series graphs.", Icon = "chart.png", Color = Colors.LightCoral },
                new GalleryItem { Title = "Rate", SubTitle = "Allows users to select a rating value from a group of visual symbols like stars.", Icon = "rate.png", Color = Colors.DarkTurquoise },
                new GalleryItem { Title = "SegmentedControl", SubTitle = "Is a linear segment made up of multiple segments and allow users to select between multiple options.", Icon = "segmentedcontrol.png", Color = Colors.DarkKhaki }
             };

            Gallery = new ObservableCollection<GalleryItem>
            {
                new GalleryItem { Title = "AvatarView", SubTitle = "Is a graphical representation of the user image view that can be customized by adding icon, text, etc.", Icon = "avatarview.png", Color = Colors.LightPink },
                new GalleryItem { Title = "BadgeView", SubTitle = "Control used to  used to notify users notifications, or status of something.", Icon = "badgeview.png", Color = Colors.LightSkyBlue, Status = GalleryItemStatus.Preview },
                new GalleryItem { Title = "CarouselView", SubTitle = "Allow to navigate through a collection of views.", Icon = "carouselview.png", Color = Colors.LimeGreen },
                new GalleryItem { Title = "ChatBubble", SubTitle = "Allow to show a speech bubble message.", Icon = "chatbubble.png", Color = Colors.DarkSeaGreen },
                new GalleryItem { Title = "CircularLayout", SubTitle = "Is a simple Layout derivative that lays out its children in a circular arrangement.", Icon = "circularlayout.png", Color = Colors.BlueViolet },
                //new GalleryItem { Title = "CircleProgressBar", SubTitle = "Shows a control that indicates the progress percentage of an on-going operation by circular shape.", Icon = "circleprogressbar.png", Color = Color.LightGray, Status = GalleryItemStatus.InProgress },
                new GalleryItem { Title = "ComparerView", SubTitle = "Provides an option for displaying a split-screen of two views, which can help you to make comparisons.", Icon = "comparerview.png", Color = Colors.DarkViolet, Status = GalleryItemStatus.InProgress },
                new GalleryItem { Title = "DataVisualization", SubTitle = "Several series graphs.", Icon = "chart.png", Color = Colors.LightCoral, Status = GalleryItemStatus.Preview  },
                new GalleryItem { Title = "Divider", SubTitle = "Displays a separator between views.", Icon = "divider.png", Color = Colors.Orchid  },
                new GalleryItem { Title = "DockLayout", SubTitle = "Makes it easy to dock content in all four directions (top, bottom, left and right).", Icon = "docklayout.png", Color = Colors.MediumSpringGreen },
                new GalleryItem { Title = "GridSplitter", SubTitle = "Represents the control that redistributes space between columns or rows of a Grid control.", Icon = "gridsplitter.png", Color = Colors.DarkOrchid },
                new GalleryItem { Title = "HexLayout", SubTitle = "A Layout that arranges the elements in a honeycomb pattern.", Icon = "hexlayout.png", Color = Colors.LightGoldenrodYellow },
                new GalleryItem { Title = "Marquee", SubTitle = "Use this control to add an attention–getting text message that scrolls continuously across the screen.", Icon = "marquee.png", Color = Colors.DarkRed },
                new GalleryItem { Title = "PinBox", SubTitle = "Allow to introduce a PIN or verification Code.", Icon = "pinbox.png", Color = Colors.PaleVioletRed },
                new GalleryItem { Title = "ProgressBar", SubTitle = "Provides a customizable visual to indicate the progress of a task.", Icon = "progressbar.png", Color = Colors.DodgerBlue, Status = GalleryItemStatus.InProgress  },
                new GalleryItem { Title = "Rate", SubTitle = "Allows users to select a rating value from a group of visual symbols like stars.", Icon = "rate.png", Color = Colors.DarkTurquoise },
                new GalleryItem { Title = "SegmentedControl", SubTitle = "Is a linear segment made up of multiple segments and allow users to select between multiple options.", Icon = "segmentedcontrol.png", Color = Colors.DarkKhaki, Status = GalleryItemStatus.InProgress },
                new GalleryItem { Title = "Shield", SubTitle = "Shield is a type of badge.", Icon = "shield.png", Color = Colors.DarkOliveGreen },
                new GalleryItem { Title = "Slider", SubTitle = "Is a horizontal bar that can be manipulated by the user to select a double value from a continuous range.", Icon = "slider.png", Color = Colors.ForestGreen, Status = GalleryItemStatus.InProgress },
                new GalleryItem { Title = "SnackBar", SubTitle = "Provide brief messages about app processes at the bottom of the screen.", Icon = "snackbar.png", Color = Colors.IndianRed, Status = GalleryItemStatus.InProgress },
                new GalleryItem { Title = "Tag", SubTitle = "Is a tagging control.", Icon = "tag.png", Color = Colors.DarkSalmon },
                new GalleryItem { Title = "ToggleSwitch", SubTitle = "A View control that provides a toggled value.", Icon = "toggleswitch.png", Color = Colors.DeepPink, Status = GalleryItemStatus.Preview  },
                new GalleryItem { Title = "TreeView", SubTitle = "Enables a hierarchical list with expanding and collapsing nodes that contain nested items.", Icon = "tag.png", Color = Colors.MediumPurple, Status = GalleryItemStatus.InProgress  }
            };
        }

        void OpenGitHubCommand()
        {
            Browser.OpenAsync("https://github.com/jsuarezruiz/TemplateUI", BrowserLaunchMode.External);
        }

        async void NavigateToGallery(GalleryItem galleryItem)
        {
            Type type = Type.GetType($"TemplateUI.Gallery.Views.{galleryItem.Title}Gallery");

            if (type != null)
            {
                if (Activator.CreateInstance(type) is Page page)
                {
                    if (galleryItem.Status == GalleryItemStatus.InProgress)
                        await DialogService.Instance.ShowInfoAlert("Work in progress", "This control is in progress and may present visual or functional errors.");

                    await NavigationService.Instance.NavigateAsync(page);
                }
            }
        }
    }
}