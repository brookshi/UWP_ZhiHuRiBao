using Brook.ZhiHuRiBao.Common;
using Brook.ZhiHuRiBao.Utils;
using Brook.ZhiHuRiBao.ViewModels;
using System.ComponentModel;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Brook.ZhiHuRiBao.Pages
{
    public sealed partial class MainContentPage : Page
    {
        public MainContentViewModel VM { get { return DataContext as MainContentViewModel; } }

        public MainContentPage()
        {
            InitializeComponent();
            WebViewUtil.RemoveParent();
            MainContent.Children.Add(WebViewUtil.GetWebViewWithBinding(VM, "MainHtmlContent"));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            var storyId = e.Parameter.ToString();
            if(!string.IsNullOrEmpty(storyId))
            {
                VM.UpdateStoryId(storyId);
            }
        }
    }
}
