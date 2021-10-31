using System.Collections;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace TemplateUI.DataVisualization
{
    public abstract class SerialChart : TemplatedView
    {
        public static readonly BindableProperty DataSourceProperty =
            BindableProperty.Create(nameof(DataSource), typeof(IEnumerable), typeof(SerialChart), null,
                propertyChanged: OnDataSourcePropertyChanged);

        public IEnumerable DataSource
        {
            get { return (IEnumerable)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        static void OnDataSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SerialChart chart = bindable as SerialChart;
            DetachOldDataSourceCollectionChangedListener(chart, oldValue);
            AttachDataSourceCollectionChangedListener(chart, newValue);
            chart.NotifyDataSourceChanges();
        }

        static void DetachOldDataSourceCollectionChangedListener(SerialChart chart, object dataSource)
        {
            if (dataSource != null && dataSource is INotifyCollectionChanged)
                (dataSource as INotifyCollectionChanged).CollectionChanged -= chart.OnDataSourceCollectionChanged;
        }

        static void AttachDataSourceCollectionChangedListener(SerialChart chart, object dataSource)
        {
            if (dataSource != null && dataSource is INotifyCollectionChanged)
                (dataSource as INotifyCollectionChanged).CollectionChanged += new NotifyCollectionChangedEventHandler(chart.OnDataSourceCollectionChanged);
        }

        void OnDataSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyDataSourceChanges();
        }

        public static readonly BindableProperty CategoryMemberPathProperty =
          BindableProperty.Create(nameof(CategoryMemberPath), typeof(string), typeof(SerialChart), string.Empty);

        public string CategoryMemberPath
        {
            get { return (string)GetValue(CategoryMemberPathProperty); }
            set { SetValue(CategoryMemberPathProperty, value); }
        }

        public static readonly BindableProperty ValueMemberPathProperty =
            BindableProperty.Create(nameof(ValueMemberPath), typeof(string), typeof(SerialChart), string.Empty);

        public string ValueMemberPath
        {
            get { return (string)GetValue(ValueMemberPathProperty); }
            set { SetValue(ValueMemberPathProperty, value); }
        }

        public static readonly BindableProperty TitleProperty =
           BindableProperty.Create(nameof(Title), typeof(string), typeof(SerialChart), string.Empty);

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly BindableProperty AxisColorProperty =
            BindableProperty.Create(nameof(AxisColor), typeof(Color), typeof(SerialChart), Color.Black);

        public Color AxisColor
        {
            get { return (Color)GetValue(AxisColorProperty); }
            set { SetValue(AxisColorProperty, value); }
        }

        public static readonly BindableProperty ColorProperty =
          BindableProperty.Create(nameof(Color), typeof(Color), typeof(SerialChart), Color.Black,
              propertyChanged: OnColorPropertyChanged);

        static void OnColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SerialChart chart = bindable as SerialChart;
            chart.NotifyColorChanges();
        }

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public virtual void UpdateDataSource()
        {

        }

        public virtual void UpdateColor()
        {

        }

        void NotifyDataSourceChanges()
        {
            UpdateDataSource();
        }

        void NotifyColorChanges()
        {
            UpdateColor();
        }
    }
}