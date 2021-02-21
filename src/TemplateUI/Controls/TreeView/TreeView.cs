using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    // TODO: Add ItemsSource property, TreeViewItem, Load on Demand, Animations, etc.
    [ContentProperty(nameof(RootNodes))]
    public class TreeView : TemplatedView
    {
        const string ElementContainer = "PART_Container";

        StackLayout _container;

        public static readonly BindableProperty RootNodesProperty =
          BindableProperty.Create(nameof(RootNodes), typeof(TreeViewNodes), typeof(TreeView), new TreeViewNodes(),
              propertyChanged: OnRootNodesChanged);

        static void OnRootNodesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TreeView)?.UpdatetNodes();
        }

        public TreeViewNodes RootNodes
        {
            get => (TreeViewNodes)GetValue(RootNodesProperty);
            set { SetValue(RootNodesProperty, value); }
        }

        public static readonly BindableProperty SelectionModeProperty =
            BindableProperty.Create(nameof(SelectionMode), typeof(SelectionMode), typeof(TreeView), SelectionMode.Single);

        public SelectionMode SelectionMode
        {
            get => (SelectionMode)GetValue(SelectionModeProperty);
            set { SetValue(SelectionModeProperty, value); }
        }

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(TreeViewNode), typeof(TreeView), null);

        public TreeViewNode SelectedItem
        {
            get => (TreeViewNode)GetValue(SelectedItemProperty);
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly BindableProperty IndentationProperty =
            BindableProperty.Create(nameof(Indentation), typeof(double), typeof(TreeView), 24.0d);

        public double Indentation
        {
            get => (double)GetValue(IndentationProperty);
            set { SetValue(IndentationProperty, value); }
        }

        public static new readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(TreeView), Color.Default);

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly BindableProperty BackgroundColorSelectedProperty =
            BindableProperty.Create(nameof(BackgroundColorSelected), typeof(Color), typeof(TreeView), Color.Default);

        public Color BackgroundColorSelected
        {
            get => (Color)GetValue(BackgroundColorSelectedProperty);
            set { SetValue(BackgroundColorSelectedProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TreeView), Color.Black);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty TextColorSelectedProperty =
            BindableProperty.Create(nameof(TextColorSelected), typeof(Color), typeof(TreeView), Color.Default);

        public Color TextColorSelected
        {
            get => (Color)GetValue(TextColorSelectedProperty);
            set { SetValue(TextColorSelectedProperty, value); }
        }

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(TreeView), Device.GetNamedSize(NamedSize.Small, typeof(Label)));

        [Xamarin.Forms.TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly BindableProperty FontSizeSelectedProperty =
            BindableProperty.Create(nameof(FontSizeSelected), typeof(double), typeof(TreeView), Device.GetNamedSize(NamedSize.Small, typeof(Label)));

        [Xamarin.Forms.TypeConverter(typeof(FontSizeConverter))]
        public double FontSizeSelected
        {
            get => (double)GetValue(FontSizeSelectedProperty);
            set { SetValue(FontSizeSelectedProperty, value); }
        }

        public event EventHandler SelectedItemChanged;
        public event EventHandler<NodeTappedEventArgs> ItemTapped;
        public event EventHandler<NodeDoubleTappedEventArgs> ItemDoubleTapped;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _container = GetTemplateChild(ElementContainer) as StackLayout;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal void UpdateSelectedItem(TreeViewNode selectedItem, bool isSelected)
        {
            if (SelectionMode == SelectionMode.None)
                return;

            if (SelectedItem == selectedItem)
                return;

            UnSelectItems(RootNodes);
            selectedItem.IsSelected = isSelected;
            SelectedItem = isSelected ? selectedItem : null;
            SelectedItemChanged?.Invoke(this, EventArgs.Empty);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal void SendItemTapped(NodeTappedEventArgs args)
        {
            ItemTapped?.Invoke(this, args);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal void SendItemDoubleTapped(NodeDoubleTappedEventArgs args)
        {
            ItemDoubleTapped?.Invoke(this, args);
        }

        void UnSelectItems(TreeViewNodes treeViewNodes)
        {
            foreach (var childNode in treeViewNodes)
            {
                childNode.IsSelected = false;
                UnSelectItems(childNode.Children);
            }
        }

        void UpdatetNodes()
        {
            if (RootNodes == null || RootNodes.Count == 0)
                return;

            foreach (var childNode in RootNodes)
            {
                if (!_container.Children.Contains(childNode))
                {
                    UpdateTreeViewNodes(childNode);
                    _container.Children.Add(childNode);
                }
            }
        }

        void UpdateTreeViewNodes(TreeViewNode treeViewNode)
        {
            UpdateTreeViewNode(treeViewNode);

            foreach (var childNode in treeViewNode.Children)
                UpdateTreeViewNodes(childNode);
        }

        void UpdateTreeViewNode(TreeViewNode treeViewNode)
        {
            if (treeViewNode == null)
                return;

            if (treeViewNode.Indentation == 0)
                treeViewNode.Indentation = Indentation;

            if (treeViewNode.BackgroundColor == Color.Default)
                treeViewNode.BackgroundColor = BackgroundColor;

            if (treeViewNode.BackgroundColorSelected == Color.Default)
                treeViewNode.BackgroundColorSelected = BackgroundColorSelected;

            if (treeViewNode.TextColor == Color.Default)
                treeViewNode.TextColor = TextColor;

            if (treeViewNode.TextColorSelected == Color.Default)
                treeViewNode.TextColorSelected = TextColorSelected;

            if (treeViewNode.FontSize == Device.GetNamedSize(NamedSize.Small, typeof(Label)))
                treeViewNode.FontSize = FontSize;

            if (treeViewNode.FontSizeSelected == Device.GetNamedSize(NamedSize.Small, typeof(Label)))
                treeViewNode.FontSizeSelected = FontSizeSelected;
        }
    }
}