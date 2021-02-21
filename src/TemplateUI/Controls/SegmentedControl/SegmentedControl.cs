using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    [ContentProperty(nameof(SegmentedItems))]
    public class SegmentedControl : TemplatedView
    {
        const string ElementBorder = "PART_Border";
        const string ElementHolder = "PART_Holder";

        Grid _holder;
        int _counter;

        public SegmentedControl()
        {
            SegmentedItems = new ObservableCollection<SegmentedItem>();

            UpdateIsEnabled();
        }

        public ObservableCollection<SegmentedItem> SegmentedItems { get; set; }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(SegmentedControl), null,
                propertyChanged: OnTabButtonsPropertyChanged, defaultBindingMode: BindingMode.TwoWay);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        static void OnTabButtonsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SegmentedControl)?.UpdateItemsSource();
        }

        public static readonly BindableProperty SelectedIndexProperty =
            BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(SegmentedControl), 0, BindingMode.TwoWay,
               propertyChanged: OnSelectedIndexChanged);

        static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SegmentedControl)?.UpdateSelectedIndex();
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(SegmentedControl), 4.0d);

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty HasShadowProperty =
            BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(SegmentedControl), false);

        public bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set { SetValue(HasShadowProperty, value); }
        }

        public static new readonly BindableProperty BackgroundColorProperty =
          BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(SegmentedControl), Color.Default);

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly BindableProperty BackgroundColorSelectedProperty =
        BindableProperty.Create(nameof(BackgroundColorSelected), typeof(Color), typeof(SegmentedControl), Color.Default);

        public Color BackgroundColorSelected
        {
            get => (Color)GetValue(BackgroundColorSelectedProperty);
            set { SetValue(BackgroundColorSelectedProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(SegmentedControl), Color.Default);

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty =
           BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(SegmentedControl), Color.Default);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty TextColorSelectedProperty =
            BindableProperty.Create(nameof(TextColorSelected), typeof(Color), typeof(SegmentedControl), Color.Default);

        public Color TextColorSelected
        {
            get => (Color)GetValue(TextColorSelectedProperty);
            set { SetValue(TextColorSelectedProperty, value); }
        }
        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(SegmentedControl), Device.GetNamedSize(NamedSize.Small, typeof(Label)));

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly BindableProperty FontSizeSelectedProperty =
            BindableProperty.Create(nameof(FontSizeSelected), typeof(double), typeof(SegmentedControl), Device.GetNamedSize(NamedSize.Small, typeof(Label)));

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSizeSelected
        {
            get => (double)GetValue(FontSizeSelectedProperty);
            set { SetValue(FontSizeSelectedProperty, value); }
        }

        public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(SegmentedControl), string.Empty);

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set { SetValue(FontFamilyProperty, value); }
        }

        public static readonly BindableProperty FontFamilySelectedProperty =
            BindableProperty.Create(nameof(FontFamilySelected), typeof(string), typeof(SegmentedControl), string.Empty);

        public string FontFamilySelected
        {
            get => (string)GetValue(FontFamilySelectedProperty);
            set { SetValue(FontFamilySelectedProperty, value); }
        }

        public static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(SegmentedControl), FontAttributes.None);

        public FontAttributes FontAttributes
        {
            get => (FontAttributes)GetValue(FontAttributesProperty);
            set { SetValue(FontAttributesProperty, value); }
        }

        public static readonly BindableProperty FontAttributesSelectedProperty =
            BindableProperty.Create(nameof(FontAttributesSelected), typeof(FontAttributes), typeof(SegmentedControl), FontAttributes.None);

        public FontAttributes FontAttributesSelected
        {
            get => (FontAttributes)GetValue(FontAttributesSelectedProperty);
            set { SetValue(FontAttributesSelectedProperty, value); }
        }

        public static new readonly BindableProperty HeightProperty =
            BindableProperty.Create(nameof(Height), typeof(double), typeof(SegmentedControl), 36.0d);

        public new double Height
        {
            get => (double)GetValue(HeightProperty);
            set { SetValue(HeightProperty, value); }
        }

        public event EventHandler<SelectedIndexEventArgs> SelectedIndexChanged;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _holder = GetTemplateChild(ElementHolder) as Grid;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
        }

        void UpdateItemsSource()
        {
            if (_holder == null)
                return;

            ClearSegmentedItems();

            int index = 0;

            foreach (var item in ItemsSource)
            {
                var segmentedItem = new SegmentedItem
                {
                    Text = item.ToString()
                };

                UpdateSegmentedItem(segmentedItem);
                if (AddSegmentedItem(segmentedItem))
                    Grid.SetColumn(segmentedItem, index++);
            }

            UpdateIsEnabled();
            UpdateSelectedIndex();
        }

        void OnSegmentedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                _counter = 0;

                foreach (SegmentedItem oldSegmentedItem in e.OldItems)
                {
                    ClearSegmentedItem(oldSegmentedItem);
                }
            }

            if (e.NewItems != null)
            {
                foreach (SegmentedItem newSegmentedItem in e.NewItems)
                {
                    UpdateSegmentedItem(newSegmentedItem);
                    if (AddSegmentedItem(newSegmentedItem))
                        Grid.SetColumn(newSegmentedItem, _counter++);
                }
            }

            UpdateIsEnabled();
            UpdateSelectedIndex();
        }

        void UpdateSelectedIndex()
        {
            var selectedIndexEventArgs = new SelectedIndexEventArgs(SelectedIndex);
            SelectedIndexChanged?.Invoke(this, selectedIndexEventArgs);

            int index = 0;

            foreach (var child in _holder.Children)
            {
                if (child is SegmentedItem segmentedItem)
                    segmentedItem.IsSelected = SelectedIndex == index;

                index++;
            }
        }

        void UpdateIsEnabled()
        {
            if (_holder == null)
                return;

            if (IsEnabled)
            {
                SegmentedItems.CollectionChanged += OnSegmentedItemsCollectionChanged;

                foreach (var children in _holder.Children)
                {
                    if (children is SegmentedItem segmentedItem)
                    {
                        var tapGestureRecognizer = new TapGestureRecognizer();
                        tapGestureRecognizer.Tapped += OnSegmentedItemTapped;

                        if (!segmentedItem.GestureRecognizers.Contains(tapGestureRecognizer))
                            segmentedItem.GestureRecognizers.Add(tapGestureRecognizer);
                    }
                }
            }
            else
            {
                SegmentedItems.CollectionChanged -= OnSegmentedItemsCollectionChanged;

                foreach (var children in _holder.Children)
                {
                    if (children is SegmentedItem segmentedItem)
                    {
                        segmentedItem.GestureRecognizers.Clear();
                    }
                }
            }
        }

        void OnSegmentedItemTapped(object sender, EventArgs e)
        {
            if (sender is SegmentedItem segmentedItem)
            {
                var selectedIndex = _holder.Children.IndexOf(segmentedItem);

                int index = 0;

                foreach (var child in _holder.Children)
                {
                    if (child is SegmentedItem si)
                        si.IsSelected = selectedIndex == index;

                    index++;
                }
            }
        }

        void UpdateSegmentedItem(SegmentedItem segmentedItem)
        {
            if (segmentedItem.BackgroundColor == Color.Default)
                segmentedItem.BackgroundColor = BackgroundColor;

            if (segmentedItem.BackgroundColorSelected == Color.Default)
                segmentedItem.BackgroundColorSelected = BackgroundColorSelected;

            if (segmentedItem.TextColor == Color.Default)
                segmentedItem.TextColor = TextColor;

            if (segmentedItem.TextColorSelected == Color.Default)
                segmentedItem.TextColorSelected = TextColorSelected;

            if (segmentedItem.FontSize == Device.GetNamedSize(NamedSize.Small, typeof(Label)))
                segmentedItem.FontSize = FontSize;

            if (segmentedItem.FontSizeSelected == Device.GetNamedSize(NamedSize.Small, typeof(Label)))
                segmentedItem.FontSizeSelected = FontSizeSelected;

            if (!string.IsNullOrEmpty(segmentedItem.FontFamily))
                segmentedItem.FontFamily = FontFamily;

            if (!string.IsNullOrEmpty(segmentedItem.FontFamily))
                segmentedItem.FontFamilySelected = FontFamilySelected;

            if (segmentedItem.FontAttributes == FontAttributes.None)
                segmentedItem.FontAttributes = FontAttributes;

            if (segmentedItem.FontAttributesSelected == FontAttributes.None)
                segmentedItem.FontAttributesSelected = FontAttributesSelected;
        }

        void ClearSegmentedItems()
        {
            if (_holder == null)
                return;

            _holder.Children.Clear();
        }

        void ClearSegmentedItem(SegmentedItem segmentedItem)
        {
            if (_holder == null)
                return;

            _holder.Children.Remove(segmentedItem);
        }

        bool AddSegmentedItem(SegmentedItem segmentedItem)
        {
            if (_holder == null || _holder.Children.Contains(segmentedItem))
                return false;

            _holder.Children.Add(segmentedItem);

            return true;
        }
    }
}