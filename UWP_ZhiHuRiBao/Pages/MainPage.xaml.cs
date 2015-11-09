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
using Brook.ZhiHuRiBao.ViewModels;
using System;

namespace Brook.ZhiHuRiBao.Pages
{
    public sealed partial class MainPage : PageBase
    {
        public MainPage()
        {
            this.InitializeComponent();
            Initalize();
            NavigationCacheMode = NavigationCacheMode.Required;
            //UpdateBarStyle((Color)Application.Current.Resources["ColorPrimary"]);

            MainListView.Refresh = RefreshMainList;
            MainListView.LoadMore = LoadMoreStories;

            CommentListView.Refresh = RefreshCommentList;
            CommentListView.LoadMore = LoadMoreComments;

            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            MainListView.SetRefresh(true);
        }

        public MainViewModel VM { get { return GetVM<MainViewModel>(); } }

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

        public bool IsDesktopDevice { get { return !LLM.Utils.IsOnMobile; } }

        private async void RefreshMainList()
        {
            await VM.Refresh();
            MainListView.SetRefresh(false);
            DisplayStory(VM.CurrentStoryId);
        }

        private async void LoadMoreStories()
        {
            await VM.LoadMore();
            MainListView.FinishLoadingMore();
        }

        private async void RefreshCommentList()
        {
            await VM.RequestComments(VM.CurrentStoryId, false);
            CommentListView.SetRefresh(false);
        }

        private async void LoadMoreComments()
        {
            await VM.RequestComments(VM.CurrentStoryId, true);
            CommentListView.FinishLoadingMore();
        }

        private void MainListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var story = e.ClickedItem as Story;
            if (Misc.IsGroupItem(story.type))
                return;

            var storyId = story.id.ToString();
            DisplayStory(storyId);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(BlankPage1), true);
        }

        private void TapFlipImage(object sender, RoutedEventArgs e)
        {
            var storyId = (sender as FrameworkElement).Tag.ToString();
            if (storyId != Misc.Unvalid_Image_Id.ToString())
            {
                DisplayStory(storyId);
            }
        }

        private void DisplayStory(string storyId)
        {
            VM.CurrentStoryId = storyId;
            VM.RequestMainContent(storyId);
            CommentListView.SetRefresh(true);
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MainView.IsPaneOpen = !MainView.IsPaneOpen;
        }

        private void CategoryListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var category = e.ClickedItem as Others;
            VM.CurrentCategoryId = category.id;
            MainListView.SetRefresh(true);
        }
    }
}
