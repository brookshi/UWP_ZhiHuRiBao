using System.ComponentModel;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace CN.Brook.UI
{
    [ContentProperty(Name = "TitleBarControl")]
    public sealed partial class CustomTitleBar : UserControl, INotifyPropertyChanged
    {
        private CoreApplicationViewTitleBar _titleBar = CoreApplication.GetCurrentView().TitleBar;
        public event PropertyChangedEventHandler PropertyChanged;

        private Brush _titleBarBackgroundColor = new SolidColorBrush(Colors.Transparent);
        public Brush TitleBarBackgroundColor
        {
            get { return _titleBarBackgroundColor; }
            set
            {
                if(value != _titleBarBackgroundColor)
                {
                    _titleBarBackgroundColor = value;
                    Notify("TitleBarBackgroundColor");
                }
            }
        }

        private string _title = "";
        public string Title
        {
            get { return _title; }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    Notify("Title");
                }
            }
        }

        private Thickness _titleMargin = new Thickness(10, 0, 0, 0);
        public Thickness TitleMargin
        {
            get { return _titleMargin; }
            set
            {
                if (value != _titleMargin)
                {
                    _titleMargin = value;
                    Notify("TitleMargin");
                }
            }
        }

        private ImageSource _titleBarIcon;
        public ImageSource TitleBarIcon
        {
            get { return _titleBarIcon; }
            set
            {
                if (value != _titleBarIcon)
                {
                    _titleBarIcon = value;
                    Notify("TitleBarIcon");
                }
            }
        }

        Thickness _iconMargin = new Thickness(5);
        public Thickness IconMargin
        {
            get { return _iconMargin; }
            set
            {
                if (value != _iconMargin)
                {
                    _iconMargin = value;
                    Notify("IconMargin");
                }
            }
        }

        public static readonly DependencyProperty TitleBarControlProperty = DependencyProperty.Register("TitleBarControl",
            typeof(object), typeof(CustomTitleBar), new PropertyMetadata(null));
        public object TitleBarControl
        {
            get { return GetValue(TitleBarControlProperty); }
            set { SetValue(TitleBarControlProperty, value); }
        }

        public CustomTitleBar()
        {
            this.InitializeComponent();
        }

        private void CustomTitleBarControl_Loaded(object sender, RoutedEventArgs e)
        {
            _titleBar.LayoutMetricsChanged += (s, o) => UpdateLayoutMetrics();
            InitBarStyle();
        }

        void InitBarStyle()
        {
            UpdateBarStyle();
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(TitleBarBackground);
        }

        void UpdateBarStyle()
        {
            var bgColor = (TitleBarBackgroundColor as SolidColorBrush).Color;
            UpdateBarStyle(bgColor);
        }
        
        void UpdateBarStyle(Color color)
        {
            ApplicationView.GetForCurrentView().TitleBar.BackgroundColor = color;
            ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = color;
        }

        private void UpdateLayoutMetrics()
        {
            Notify("TitleBarHeight");
            Notify("TitleBarPadding");
        }

        private void Notify(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public double TitleBarHeight
        {
            get { return _titleBar.Height; }
        }

        public Thickness TitleBarPadding
        {
            get
            {
                if (FlowDirection == FlowDirection.LeftToRight)
                {
                    return new Thickness(_titleBar.SystemOverlayLeftInset, 0, _titleBar.SystemOverlayRightInset, 0);
                }
                else
                {
                    return new Thickness(_titleBar.SystemOverlayRightInset, 0, _titleBar.SystemOverlayLeftInset, 0);
                }
            }
        }

        public void HideTitle()
        {
            TitleBarTitle.Visibility = Visibility.Collapsed;
        }
    }
}
