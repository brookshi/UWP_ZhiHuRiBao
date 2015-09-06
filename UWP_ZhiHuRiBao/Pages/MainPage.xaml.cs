using LLQ;
using Brook.ZhiHuRiBao.Events;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Brook.ZhiHuRiBao.Models;
using Brook.ZhiHuRiBao.Common;
using XPHttp;

namespace Brook.ZhiHuRiBao.Pages
{
    public sealed partial class MainPage : PageBase
    {
        public MainPage()
        {
            this.InitializeComponent();
            Initalize();
        }

        public ObservableCollection<Story> MainList {
            get { return GetVM<MainViewModel>().MainList; }
        }

        private void MainListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            GetVM<MainViewModel>().RequestMainContent((e.ClickedItem as Story).id.ToString());
        }
    }
}
