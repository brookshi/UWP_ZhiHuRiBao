using LLQ;
using Brook.ZhiHuRiBao.Events;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Brook.ZhiHuRiBao.Models;

namespace Brook.ZhiHuRiBao.Pages
{
    public sealed partial class MainPage : Page
    {
        MainViewModel _vm;
        public MainPage()
        {
            this.InitializeComponent();
            _vm = DataContext as MainViewModel;
            MainList.Add(new MainItem() { Content = "DD373游戏交易平台为全球华人游戏玩家提供游戏币|点卡|元宝|装备", ImageUrl="ms-appx:///../Assets/StoreLogo.png" });
        }

        public ObservableCollection<MainItem> MainList {
            get { return _vm.MainList; }
        }
    }
}
