using System;
using System.Runtime.CompilerServices;
using TemplateUI.Helpers;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    [ContentProperty(nameof(Children))]
    public class TreeViewNode : TemplatedView
    {
        const uint ExpandAnimationLength = 100;
        const uint CollapseAnimationLength = 50;

        const string ElementExpandIndicator = "PART_ExpandIndicator";
        const string ElementText = "PART_Text";
        const string ElementChildContainer = "PART_ChildContainer";

        Grid _expandIndicator;
        Label _text;
        StackLayout _childContainer;
        TreeView _treeView;

        public static readonly BindableProperty ChildrenProperty =
            BindableProperty.Create(nameof(Children), typeof(TreeViewNodes), typeof(TreeViewNode), new TreeViewNodes(),
                propertyChanged: OnChildrenChanged);

        static void OnChildrenChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TreeViewNode)?.UpdateChildren();
        }

        public new TreeViewNodes Children
        {
            get => (TreeViewNodes)GetValue(ChildrenProperty);
            set { SetValue(ChildrenProperty, value); }
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(TreeViewNode), null);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty IconProperty =
            BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(TreeViewNode), null,
                propertyChanged: CurrentChanged);

        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set { SetValue(IconProperty, value); }
        }

        public static readonly BindableProperty IconSelectedProperty =
            BindableProperty.Create(nameof(IconSelected), typeof(ImageSource), typeof(TreeViewNode), null,
                propertyChanged: CurrentChanged);

        public ImageSource IconSelected
        {
            get => (ImageSource)GetValue(IconSelectedProperty);
            set { SetValue(IconSelectedProperty, value); }
        }

        public static readonly BindableProperty IndentationProperty =
            BindableProperty.Create(nameof(Indentation), typeof(double), typeof(TreeViewNode), 0.0d,
                propertyChanged: CurrentChanged);

        public double Indentation
        {
            get => (double)GetValue(IndentationProperty);
            set { SetValue(IndentationProperty, value); }
        }

        public static readonly BindableProperty IsExpandedProperty =
            BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(TreeViewNode), false,
                propertyChanged: IsExpandedChanged);

        static void IsExpandedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TreeViewNode)?.UpdateIsExpanded();
        }

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set { SetValue(IsExpandedProperty, value); }
        }

        public static readonly BindableProperty IsSelectedProperty =
          BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(TreeViewNode), false,
              propertyChanged: CurrentChanged);

        static void CurrentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TreeViewNode)?.UpdateCurrent();
        }

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set { SetValue(IsSelectedProperty, value); }
        }

        public static new readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(TreeViewNode), Color.Default,
                propertyChanged: CurrentChanged);

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly BindableProperty BackgroundColorSelectedProperty =
            BindableProperty.Create(nameof(BackgroundColorSelected), typeof(Color), typeof(TreeViewNode), Color.Default,
                propertyChanged: CurrentChanged);

        public Color BackgroundColorSelected
        {
            get => (Color)GetValue(BackgroundColorSelectedProperty);
            set { SetValue(BackgroundColorSelectedProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TreeViewNode), Color.Default,
                propertyChanged: CurrentChanged);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty TextColorSelectedProperty =
            BindableProperty.Create(nameof(TextColorSelected), typeof(Color), typeof(TreeViewNode), Color.Default,
                propertyChanged: CurrentChanged);

        public Color TextColorSelected
        {
            get => (Color)GetValue(TextColorSelectedProperty);
            set { SetValue(TextColorSelectedProperty, value); }
        }

        public static readonly BindableProperty FontSizeProperty =
          BindableProperty.Create(nameof(FontSize), typeof(double), typeof(TreeViewNode), Device.GetNamedSize(NamedSize.Small, typeof(Label)),
              propertyChanged: CurrentChanged);

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly BindableProperty FontSizeSelectedProperty =
            BindableProperty.Create(nameof(FontSizeSelected), typeof(double), typeof(TreeViewNode), Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                propertyChanged: CurrentChanged);

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSizeSelected
        {
            get => (double)GetValue(FontSizeSelectedProperty);
            set { SetValue(FontSizeSelectedProperty, value); }
        }

        internal static readonly BindablePropertyKey CurrentBackgroundColorPropertyKey =
            BindableProperty.CreateReadOnly(nameof(CurrentBackgroundColor), typeof(Color), typeof(TreeViewNode), Color.Default);

        public static readonly BindableProperty CurrentBackgroundColorProperty = CurrentBackgroundColorPropertyKey.BindableProperty;

        public Color CurrentBackgroundColor
        {
            get
            {
                return (Color)GetValue(CurrentBackgroundColorProperty);
            }
            private set
            {
                SetValue(CurrentBackgroundColorPropertyKey, value);
            }
        }

        internal static readonly BindablePropertyKey CurrentIconPropertyKey =
            BindableProperty.CreateReadOnly(nameof(CurrentIcon), typeof(ImageSource), typeof(TreeViewNode), null);

        public static readonly BindableProperty CurrentIconProperty = CurrentIconPropertyKey.BindableProperty;

        public ImageSource CurrentIcon
        {
            get
            {
                return (ImageSource)GetValue(CurrentIconProperty);
            }
            private set
            {
                SetValue(CurrentIconPropertyKey, value);
            }
        }

        internal static readonly BindablePropertyKey CurrentTextColorPropertyKey =
            BindableProperty.CreateReadOnly(nameof(CurrentTextColor), typeof(Color), typeof(TreeViewNode), Color.Default);

        public static readonly BindableProperty CurrentTextColorProperty = CurrentTextColorPropertyKey.BindableProperty;

        public Color CurrentTextColor
        {
            get
            {
                return (Color)GetValue(CurrentTextColorProperty);
            }
            private set
            {
                SetValue(CurrentTextColorPropertyKey, value);
            }
        }

        internal static readonly BindablePropertyKey CurrentFontSizePropertyKey =
            BindableProperty.CreateReadOnly(nameof(CurrentFontSize), typeof(double), typeof(TreeViewNode), Device.GetNamedSize(NamedSize.Small, typeof(Label)));

        public static readonly BindableProperty CurrentFontSizeProperty = CurrentFontSizePropertyKey.BindableProperty;

        public double CurrentFontSize
        {
            get
            {
                return (double)GetValue(CurrentFontSizeProperty);
            }
            private set
            {
                SetValue(CurrentFontSizePropertyKey, value);
            }
        }

        public event EventHandler<NodeExpandedCollapsedEventArgs> Expanded;
        public event EventHandler<NodeExpandedCollapsedEventArgs> Collapsed;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _expandIndicator = GetTemplateChild(ElementExpandIndicator) as Grid;
            _text = GetTemplateChild(ElementText) as Label;
            _childContainer = GetTemplateChild(ElementChildContainer) as StackLayout;

            UpdateIsEnabled();
            UpdateIsExpanded();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            UpdateLayout();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
        }

        void UpdateChildren()
        {
            if (Children == null || Children.Count == 0)
                return;

            foreach (var childNode in Children)
                if (!_childContainer.Children.Contains(childNode))
                    _childContainer.Children.Add(childNode);
        }

        void UpdateIsEnabled()
        {
            if (IsEnabled)
            {
                var indicatorGesture = new TapGestureRecognizer();
                indicatorGesture.Tapped += OnIndicatorTapped;
                _expandIndicator.GestureRecognizers.Add(indicatorGesture);

                var textGesturedTap = new TapGestureRecognizer();
                textGesturedTap.Tapped += OnTextTapped;
                _text.GestureRecognizers.Add(textGesturedTap);

                var textGesturedDoubleTap = new TapGestureRecognizer
                {
                    NumberOfTapsRequired = 2
                };

                textGesturedDoubleTap.Tapped += OnTextDoubleTapped;
                _text.GestureRecognizers.Add(textGesturedDoubleTap);
            }
            else
            {
                // TODO: Remove only specific gestures.
                _expandIndicator.GestureRecognizers.Clear();
                _text.GestureRecognizers.Clear();
            }
        }

        void UpdateCurrent()
        {
            // TODO: Refactor (Optimize).
            if (_treeView == null)
                _treeView = VisualTreeHelper.GetParent<TreeView>(this);

            _treeView?.UpdateSelectedItem(this, IsSelected);

            CurrentIcon = !IsSelected || IconSelected == null ? Icon : IconSelected;
            CurrentBackgroundColor = !IsSelected || BackgroundColorSelected == Color.Default ? BackgroundColor : BackgroundColorSelected;
            CurrentTextColor = !IsSelected || TextColorSelected == Color.Default ? TextColor : TextColorSelected;
            CurrentFontSize = !IsSelected || FontSizeSelected == Device.GetNamedSize(NamedSize.Small, typeof(Label)) ? FontSize : FontSizeSelected;

            UpdateLayout();
        }

        void UpdateLayout()
        {
            if (_childContainer == null)
                return;

            _childContainer.Margin = new Thickness(Indentation, 0, 0, 0);

            if (_text == null)
                return;

            if (_text.Parent is View parent)
                parent.HeightRequest = _text.Height + 6;
        }

        void OnIndicatorTapped(object sender, EventArgs e)
        {
            if (Children == null || Children.Count == 0)
                return;

            IsExpanded = !IsExpanded;
        }

        void OnTextTapped(object sender, EventArgs e)
        {
            if (_treeView == null)
                _treeView = VisualTreeHelper.GetParent<TreeView>(this);

            if (_treeView != null)
            {
                _treeView.SendItemTapped(new NodeTappedEventArgs(this));

                if (_treeView.SelectionMode == SelectionMode.None)
                    return;
            }

            IsSelected = !IsSelected;
        }

        void OnTextDoubleTapped(object sender, EventArgs e)
        {
            if (Children == null || Children.Count == 0)
                return;

            if (_treeView == null)
                _treeView = VisualTreeHelper.GetParent<TreeView>(this);

            if (_treeView != null)
                _treeView.SendItemDoubleTapped(new NodeDoubleTappedEventArgs(this));

            IsExpanded = !IsExpanded;
        }

        void UpdateIsExpanded()
        {
            _childContainer.IsVisible = IsExpanded;

            if (IsExpanded)
            {
                _expandIndicator.RotateTo(90, ExpandAnimationLength);
                var treeViewNodeExpandedCollapsedEventArgs = new NodeExpandedCollapsedEventArgs(this);
                Expanded?.Invoke(this, treeViewNodeExpandedCollapsedEventArgs);
            }
            else
            {
                _expandIndicator.RotateTo(0, CollapseAnimationLength);
                var treeViewNodeExpandedCollapsedEventArgs = new NodeExpandedCollapsedEventArgs(this);
                Collapsed?.Invoke(this, treeViewNodeExpandedCollapsedEventArgs);
            }
        }
    }
}