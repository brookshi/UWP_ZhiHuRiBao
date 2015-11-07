using LLQ;
using Brook.ZhiHuRiBao.Events;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Brook.ZhiHuRiBao.Models;
using Brook.ZhiHuRiBao.Common;
using XPHttp;
using Windows.UI.Core;
using Brook.ZhiHuRiBao.Utils;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace Brook.ZhiHuRiBao.Pages
{
    public sealed partial class MainPage : PageBase
    {
        public MainPage()
        {
            this.InitializeComponent();
            Initalize();
            //UpdateBarStyle((Color)Application.Current.Resources["ColorPrimary"]);
            
            NavigationCacheMode = NavigationCacheMode.Required;
            MainListView.Refresh = RefreshMainList;
            MainListView.LoadMore = LoadMoreForMainList;

            Loaded += MainPage_Loaded;
            CommentListView.Loaded += (s, e) =>
            {
                var view = GetScrollViewer(CommentListView);
                view.ViewChanged += (sender, arg) =>
                {
                    if (view.ExtentHeight - view.VerticalOffset - view.ViewportHeight < 300)
                    {
                        CommentList.RequestComments();
                    }
                };
            };
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            MainListView.SetRefresh(true);
        }

        private MainViewModel VM { get { return GetVM<MainViewModel>(); } }

        void UpdateBarStyle(Color color)
        {
            ApplicationView.GetForCurrentView().TitleBar.BackgroundColor = color;
            ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = color;
        }

        public static ScrollViewer GetScrollViewer(DependencyObject obj)
        {
            if (obj is ScrollViewer)
                return obj as ScrollViewer;

            for(int i=0;i<VisualTreeHelper.GetChildrenCount(obj);i++)
            {
                var view = GetScrollViewer(VisualTreeHelper.GetChild(obj, i));
                if (view != null)
                    return view;
            }

            return null;
        }

        public ObservableCollection<Story> StoryList { get { return VM.StoryDataList; } }

        public CommentLoadMoreCollection CommentList { get { return VM.CommentList; } }

        public bool IsDesktopDevice { get { return !LLM.Utils.IsOnMobile; } }

        private async void RefreshMainList()
        {
            await GetVM<MainViewModel>().Refresh();
            MainListView.SetRefresh(false);
        }

        private async void LoadMoreForMainList()
        {
            await GetVM<MainViewModel>().LoadMore();
            MainListView.FinishLoadingMore();
        }

        private void MainListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var story = e.ClickedItem as Story;
            if (Misc.IsGroupItem(story.type))
                return;

            var storyId = story.id.ToString();
            GetVM<MainViewModel>().RequestMainContent(storyId);
            GetVM<MainViewModel>().RefreshComments(storyId);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(BlankPage1), true);
        }

        private void TapFlipImage(object sender, RoutedEventArgs e)
        {
            var id = (sender as FrameworkElement).Tag.ToString();
            GetVM<MainViewModel>().RequestMainContent(id);
        }
    }
}
