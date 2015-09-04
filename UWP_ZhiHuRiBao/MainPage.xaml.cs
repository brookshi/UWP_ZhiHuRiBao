using LLQ;
using Brook.ZhiHuRiBao.Events;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Brook.ZhiHuRiBao.Models;

namespace Brook.ZhiHuRiBao
{
    public sealed partial class MainPage : Page
    {
        ObservableCollection<MainItem> MainList = new ObservableCollection<MainItem>();

        public MainPage()
        {
            this.InitializeComponent();
            MainList.Add(new MainItem() { Content = "DD373游戏交易平台为全球华人游戏玩家提供游戏币|点卡|元宝|装备", ImageUrl="ms-appx:///Assets/StoreLogo.png" });
        }

    }
}
