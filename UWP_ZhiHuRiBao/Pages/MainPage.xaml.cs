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

namespace Brook.ZhiHuRiBao.Pages
{
    public sealed partial class MainPage : PageBase
    {
        public MainPage()
        {
            this.InitializeComponent();
            Initalize();
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        public ObservableCollection<Story> MainList { get { return GetVM<MainViewModel>().MainList; } }

        public ObservableCollection<Comment> CommentList { get { return GetVM<MainViewModel>().CommentList; } }

        private void MainListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            GetVM<MainViewModel>().RequestMainContent((e.ClickedItem as Story).id.ToString());
            GetVM<MainViewModel>().RequestComments((e.ClickedItem as Story).id.ToString(), false);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(BlankPage1), true);
        }
    }
}
