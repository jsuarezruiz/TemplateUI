using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class CarouselView : TemplatedView, ICarouselViewController
    {
        const string ElementContent = "Part_Content";

        const double CompletedTransitionPercentage = 0.4;

        int _itemsCount = -1;
        Grid _content;
        PanGestureRecognizer _panGesture;
        View _currentView;
        View _backView;
        View _nextView;
        View _previousView;
        Dictionary<int, View> _existingViews;
        SwipeDirection _swipeDirection;
        INotifyCollectionChanged _currentObservableCollection;

        public static readonly BindableProperty ItemsSourceProperty =
          BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(CarouselView), null,
              propertyChanged: OnItemsSourceChanged);

        public IList ItemsSource
        {
            get => (IList)GetValue(ItemsSourceProperty);
            set { SetValue(ItemsSourceProperty, value); }
        }

        static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CarouselView)?.UpdateItemsSource();
        }

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(CarouselView), null);

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public static readonly BindableProperty PositionProperty =
           BindableProperty.Create(nameof(Position), typeof(int), typeof(CarouselView), -1, BindingMode.TwoWay,
               propertyChanged: OnPositionChanged);

        public int Position
        {
            get => (int)GetValue(PositionProperty);
            set { SetValue(PositionProperty, value); }
        }

        static void OnPositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CarouselView)?.UpdatePosition();
        }

        public static readonly BindableProperty PositionChangedCommandProperty =
           BindableProperty.Create(nameof(PositionChangedCommand), typeof(ICommand), typeof(CarouselView));

        public ICommand PositionChangedCommand
        {
            get => (ICommand)GetValue(PositionChangedCommandProperty);
            set { SetValue(PositionChangedCommandProperty, value); }
        }

        public static readonly BindableProperty PositionChangedCommandParameterProperty =
            BindableProperty.Create(nameof(PositionChangedCommandParameter), typeof(object), typeof(CarouselView));

        public object PositionChangedCommandParameter
        {
            get => GetValue(PositionChangedCommandProperty);
            set { SetValue(PositionChangedCommandProperty, value); }
        }

        public static readonly BindableProperty CurrentItemProperty =
            BindableProperty.Create(nameof(CurrentItem), typeof(object), typeof(CarouselView), null, BindingMode.TwoWay,
                propertyChanged: OnCurrentItemChanged);

        public object CurrentItem
        {
            get => GetValue(CurrentItemProperty);
            set => SetValue(CurrentItemProperty, value);
        }

        static void OnCurrentItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CarouselView)?.UpdateCurrentItem();
        }

        public static readonly BindableProperty IsSwipeEnabledProperty =
            BindableProperty.Create(nameof(IsSwipeEnabled), typeof(bool), typeof(CarouselView), true,
               propertyChanged: OnIsSwipeEnabledChanged);

        public bool IsSwipeEnabled
        {
            get { return (bool)GetValue(IsSwipeEnabledProperty); }
            set { SetValue(IsSwipeEnabledProperty, value); }
        }

        static void OnIsSwipeEnabledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CarouselView)?.UpdateIsSwipeEnabled((bool)newValue);
        }

        public static readonly BindableProperty IsLazyProperty =
            BindableProperty.Create(nameof(IsLazy), typeof(bool), typeof(CarouselView), false);

        public bool IsLazy
        {
            get => (bool)GetValue(IsLazyProperty);
            set => SetValue(IsLazyProperty, value);
        }

        public static readonly BindableProperty IsCyclicalProperty =
            BindableProperty.Create(nameof(IsCyclical), typeof(bool), typeof(CarouselView), false);

        public bool IsCyclical
        {
            get => (bool)GetValue(IsCyclicalProperty);
            set => SetValue(IsCyclicalProperty, value);
        }

        static readonly BindablePropertyKey IsDraggingPropertyKey =
            BindableProperty.CreateReadOnly(nameof(IsDragging), typeof(bool), typeof(CarouselView), false);

        public static readonly BindableProperty IsDraggingProperty = IsDraggingPropertyKey.BindableProperty;

        public bool IsDragging => (bool)GetValue(IsDraggingProperty);

        public bool IsScrollAnimated
        {
            get => (bool)GetValue(IsScrollAnimatedProperty);
            set => SetValue(IsScrollAnimatedProperty, value);
        }

        public static readonly BindableProperty IsScrollAnimatedProperty =
            BindableProperty.Create(nameof(IsScrollAnimated), typeof(bool), typeof(CarouselView), true);

        public static readonly BindableProperty TransitionProperty =
           BindableProperty.Create(nameof(Transition), typeof(ICarouselItemTransition), typeof(CarouselView), new CarouselItemTransition(),
               propertyChanged: OnTabViewItemTransitionChanged);

        public ICarouselItemTransition Transition
        {
            get => (ICarouselItemTransition)GetValue(TransitionProperty);
            set { SetValue(TransitionProperty, value); }
        }

        static void OnTabViewItemTransitionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CarouselView)?.UpdateTransition((ICarouselItemTransition)newValue);
        }

        public double Offset { get; private set; }

        public event EventHandler<CurrentItemChangedEventArgs> CurrentItemChanged;
        public event EventHandler<PositionChangedEventArgs> PositionChanged;
        public event EventHandler<ScrolledEventArgs> Scrolled;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _existingViews = new Dictionary<int, View>();
            _panGesture = new PanGestureRecognizer();

            _content = GetTemplateChild(ElementContent) as Grid;

            if (_content != null)
                _content.IsClippedToBounds = true;

            UpdateIsSwipeEnabled();
        }

        public async Task ScrollToAsync(int position, bool animated = true)
        {
            if (Position == position)
                return;

            var index = position > Position ? (position - 1) : (position + 1);
            UpdateOtherViews(index);

            var threshold = (Width * CompletedTransitionPercentage) + 1;
            double offset = position > Position ? -threshold : threshold;

            if (!UpdateBackView(offset))
                return;

            if (!IsValidOffset(offset))
                return;

            _backView.IsVisible = true;
            UpdateOffset(offset);

            if (animated)
                await RaiseSelectionChanging();

            SetViewsAndPosition();

            if (animated)
                await RaiseSelectionChanged();

            UpdatePosition();
            UpdateViews();
            ClearViews();
        }

        void UpdateItemsSource()
        {
            UpdateItemsCount();
        }

        void UpdatePosition()
        {
            var index = 0;

            if (_currentView != null)
            {
                for (var i = 0; i < _itemsCount; ++i)
                {
                    var bc = _currentView.BindingContext;

                    if (ItemsSource[i] == bc)
                    {
                        index = i;
                        break;
                    }
                }

                if (index < 0)
                    index = Position - 1;
            }

            if (index < 0)
                index = 0;

            Position = index;

            RaisePositionChanged();
        }

        void UpdateCurrentItem()
        {
            var selectedIndex = Position;

            if (IsCyclical)
                selectedIndex = GetCyclicalIndex(selectedIndex, _itemsCount);

            if (!IsIndexValid(selectedIndex))
            {
                CurrentItem = null;
                return;
            }

            CurrentItem = ItemsSource[selectedIndex];

            RaiseCurrentItemChanged();
        }

        void SetIsDragging(bool value)
        {
            SetValue(IsDraggingPropertyKey, value);
        }

        void UpdateIsSwipeEnabled(bool isSwipeEnabled = true)
        {
            if (isSwipeEnabled)
            {
                _panGesture.PanUpdated += OnPanUpdated;
                GestureRecognizers.Add(_panGesture);
            }
            else
            {
                _panGesture.PanUpdated -= OnPanUpdated;
                GestureRecognizers.Remove(_panGesture);
            }
        }

        void UpdateTransition(ICarouselItemTransition tabViewItemTransition)
        {
            Transition = tabViewItemTransition;
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (_itemsCount < 0)
                return;

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    TouchStarted();
                    break;
                case GestureStatus.Running:
                    TouchChanged(e.TotalX);
                    break;
                case GestureStatus.Canceled:
                case GestureStatus.Completed:
                    TouchEnded();
                    break;
            }
        }

        void UpdateViews()
        {
            if (ItemsSource != null && Position < _itemsCount)
            {
                UpdateCurrentView();

                bool addedCurrentView = AddView(_currentView);

                if (addedCurrentView)
                    AddOrUpdateCache(Position, _currentView);
            }

            UpdateOtherViews();
        }

        public void TouchStarted()
        {
            UpdateOtherViews();
        }

        public async void TouchChanged(double offset)
        {
            SetIsDragging(true);

            if (!UpdateBackView(offset))
                return;

            if (!IsValidOffset(offset))
                return;

            _backView.IsVisible = true;
            UpdateOffset(offset);

            await RaiseSelectionChanging();
            RaiseScrolled();
        }

        public async void TouchEnded()
        {
            SetIsDragging(false);

            if (!IsValidOffset(Offset))
                return;

            SetViewsAndPosition();
            await RaiseSelectionChanged();

            UpdatePosition();
            UpdateViews();
            ClearViews();

            UpdateOffset(0);
            RaiseScrolled();
        }

        void UpdateOffset(double offset)
        {
            Offset = offset;
        }

        void SetViewsAndPosition()
        {
            var absOffset = Math.Abs(Offset);

            if (absOffset > Width * CompletedTransitionPercentage)
            {
                var delta = -Math.Sign(Offset);

                var newSelectedIndex = Position + delta;

                if (newSelectedIndex < 0 || newSelectedIndex >= _itemsCount)
                    newSelectedIndex = GetNewIndex(newSelectedIndex);

                var backView = _currentView;
                _currentView = _backView;
                _backView = backView;

                Position = newSelectedIndex;
            }
        }

        bool IsValidOffset(double offset)
        {
            if (IsCyclical)
                return true;

            if (Position == 0 && offset > 0)
                return false;

            if (Position == (_itemsCount - 1) && offset < 0)
                return false;

            return true;
        }

        bool UpdateBackView(double offset)
        {
            View invisibleView;

            if (offset > 0)
            {
                _backView = _previousView;
                invisibleView = _nextView;
                _swipeDirection = SwipeDirection.RightToLeft;
            }
            else
            {
                _backView = _nextView;
                invisibleView = _previousView;
                _swipeDirection = SwipeDirection.LeftToRight;
            }

            if (invisibleView != null && invisibleView != _backView)
                invisibleView.IsVisible = false;

            return _backView != null;
        }

        void UpdateCurrentView()
        {
            _currentView = GetView(Position, null);
        }

        void UpdateOtherViews(int? index = null)
        {
            if (_itemsCount == 0)
                return;

            var currentIndex = index ?? Position;

            var nextIndex = currentIndex + 1;
            var prevIndex = currentIndex - 1;

            _nextView = GetView(nextIndex, SwipeDirection.LeftToRight);

            if (_nextView != null)
                _nextView.IsVisible = false;

            _previousView = GetView(prevIndex, SwipeDirection.RightToLeft);

            if (_previousView != null)
                _previousView.IsVisible = false;

            bool addedNextView = AddView(_nextView);

            if (addedNextView)
            {
                AddOrUpdateCache(nextIndex, _nextView);
                UpdateViewIndex(_nextView);
            }

            bool addedPreviousView = AddView(_previousView);

            if (addedPreviousView)
            {
                AddOrUpdateCache(prevIndex, _previousView);
                UpdateViewIndex(_previousView);
            }
        }

        void AddOrUpdateCache(int key, View value)
        {
            if (_existingViews == null)
                return;

            if (_existingViews.ContainsKey(key))
                _existingViews.Remove(key);

            _existingViews.Add(key, value);
        }

        View GetView(int index, SwipeDirection? scrollDirection)
        {
            if (_existingViews == null || _itemsCount < 0)
                return null;

            if (index < 0 || index >= _itemsCount)
            {
                if (IsCyclical)
                    index = GetCyclicalIndex(index, _itemsCount);
                else
                {
                    if (!IsLazy || (scrollDirection != null && _itemsCount < 2))
                        return null;

                    index = GetNewIndex(index);
                }
            }

            var context = ItemsSource[index];

            _existingViews.TryGetValue(index, out View view);

            if (view == null)
            {
                view = (View)ItemTemplate.CreateContent();
                view.BindingContext = context;
            }

            return view;
        }

        void ClearViews()
        {
            if (_content == null)
                return;

            List<View> toRemove = new List<View>();

            foreach (var children in _content.Children)
            {
                if (IsLazy)
                {
                    if (children != _currentView)
                        toRemove.Add(children);
                }
                else
                {
                    if (children != _currentView && children != _previousView && children != _nextView)
                        toRemove.Add(children);
                }
            }

            foreach (var remove in toRemove)
            {
                _content.Children.Remove(remove);

                if (_existingViews.ContainsValue(remove))
                {
                    var index = GetIndexByExistingView(remove);

                    if (index.HasValue)
                        _existingViews.Remove(index.Value);
                }
            }
        }

        int? GetIndexByExistingView(View view)
        {
            int? index = null;

            foreach (var existingView in _existingViews)
            {
                if (existingView.Value == view)
                {
                    index = existingView.Key;
                    break;
                }
            }

            return index;
        }

        void UpdateViewIndex(View view)
        {
            if (view == null)
                return;

            if (_currentView != null)
            {
                var currentIndex = _content.Children.IndexOf(_currentView);
                var backIndex = _content.Children.IndexOf(view);

                if (currentIndex < backIndex)
                {
                    _content.Children.Remove(view);
                    _content.Children.Insert(0, view);
                }
            }
        }

        void UpdateItemsCount()
        {
            if (_currentObservableCollection != null)
            {
                _currentObservableCollection.CollectionChanged -= OnObservableCollectionChanged;
            }

            if (ItemsSource is INotifyCollectionChanged observableCollection)
            {
                _currentObservableCollection = observableCollection;
                observableCollection.CollectionChanged += OnObservableCollectionChanged;
            }

            OnObservableCollectionChanged(ItemsSource, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        void OnObservableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _itemsCount = ItemsSource?.Count ?? -1;

            UpdatePosition();
            UpdateViews();
            ClearViews();
        }

        bool AddView(View view, int index = -1)
        {
            if (view == null || _content == null || _content.Children.Contains(view))
                return false;

            if (index < 0)
            {
                _content.Children.Add(view);
                return true;
            }

            _content.Children.Insert(index, view);

            return true;
        }

        int GetValidIndex(int index)
        {
            if (index < 0)
                index = 0;

            if (index >= _itemsCount)
                return _itemsCount;

            return index;
        }

        int GetNewIndex(int index)
        {
            while (index < 0 || index >= _itemsCount)
                index = Math.Abs(_itemsCount - Math.Abs(index));

            return index;
        }

        int GetCyclicalIndex(int selectedIndex, int itemsCount)
        {
            if (itemsCount <= 0)
                return -1;

            var result = selectedIndex % itemsCount;

            return result >= 0 ? result : result + itemsCount;
        }

        bool IsIndexValid(int index)
        {
            return index >= 0 && index < _itemsCount;
        }

        void RaiseCurrentItemChanged()
        {
            var args = new CurrentItemChangedEventArgs(CurrentItem);
            CurrentItemChanged?.Invoke(this, args);
        }

        void RaisePositionChanged()
        {
            var args = new PositionChangedEventArgs(Position);
            PositionChanged?.Invoke(this, args);

            if (PositionChangedCommand != null && PositionChangedCommand.CanExecute(PositionChangedCommandParameter))
                PositionChangedCommand.Execute(PositionChangedCommandParameter);
        }

        void RaiseScrolled()
        {
            if (!IsScrollAnimated)
                return;

            int centerItemIndex = Position;
            int firstVisibleItemIndex = centerItemIndex;
            int lastVisibleItemIndex = centerItemIndex;

            if (Offset < 0)
            {
                if (IsCyclical)
                    lastVisibleItemIndex = GetCyclicalIndex(centerItemIndex + 1, _itemsCount);
                else
                    lastVisibleItemIndex = GetValidIndex(centerItemIndex + 1);
            }

            if (Offset > 0)
            {
                if (IsCyclical)
                    firstVisibleItemIndex = GetCyclicalIndex(centerItemIndex - 1, _itemsCount);
                else
                    firstVisibleItemIndex = GetValidIndex(centerItemIndex - 1);
            }

            var args = new ScrolledEventArgs
            {
                Offset = Offset,
                FirstVisibleItemIndex = firstVisibleItemIndex,
                CenterItemIndex = centerItemIndex,
                LastVisibleItemIndex = lastVisibleItemIndex
            };

            Scrolled?.Invoke(this, args);
        }

        async Task RaiseSelectionChanging()
        {
            if (!IsScrollAnimated)
                return;

            if (Transition == null)
                return;

            var args = new SelectionChangingArgs
            {
                CurrentView = _currentView,
                NextView = _backView,
                Direction = _swipeDirection,
                Offset = Offset,
                Parent = this
            };

            await Transition.OnSelectionChanging(args);
        }

        async Task RaiseSelectionChanged()
        {
            if (!IsScrollAnimated)
                return;

            if (Transition == null)
                return;

            var absOffset = Math.Abs(Offset);

            if (absOffset > Width * CompletedTransitionPercentage)
            {
                var args = new SelectionChangedArgs
                {
                    CurrentView = _backView,
                    NextView = _currentView,
                    Status = SelectionChanged.Completed,
                    Direction = _swipeDirection,
                    Parent = this
                };

                await Transition.OnSelectionChanged(args);
            }
            else
            {
                var args = new SelectionChangedArgs
                {
                    CurrentView = _currentView,
                    NextView = _backView,
                    Status = SelectionChanged.Reset,
                    Direction = _swipeDirection,
                    Parent = this
                };

                await Transition.OnSelectionChanged(args);
            }
        }
    }
}