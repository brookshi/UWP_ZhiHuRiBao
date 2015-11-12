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

            Loaded += MainPage_Loaded;

            FavButton.Content = StringUtil.GetString("Favorite");
            DownloadButton.Content = StringUtil.GetString("DownloadOffline");
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

        private void MainListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var story = e.ClickedItem as Story;
            if (Misc.IsGroupItem(story.type))
                return;

            var storyId = story.id.ToString();
            DisplayStory(storyId);
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
            MainContentFrame.Navigate(typeof(MainContentPage), storyId);
            CommentFrame.Navigate(typeof(CommentPage), storyId);
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
