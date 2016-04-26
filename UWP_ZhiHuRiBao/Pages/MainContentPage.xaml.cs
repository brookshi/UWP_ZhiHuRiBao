using Brook.ZhiHuRiBao.Common;
using Brook.ZhiHuRiBao.Utils;
using Brook.ZhiHuRiBao.ViewModels;
using System.ComponentModel;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using LLQ;
using Brook.ZhiHuRiBao.Events;

namespace Brook.ZhiHuRiBao.Pages
{
    public sealed partial class MainContentPage : Page
    {
        public MainContentViewModel VM { get { return DataContext as MainContentViewModel; } }

        public MainContentPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            LLQNotifier.Default.Register(this);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            WebViewUtil.AddWebViewWithBinding(MainContent, VM, "MainHtmlContent");

            if (Config.UIStatus == AppUIStatus.List)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            CurrentUIStatus = Config.UIStatus;
            VM.RequestStoryData();
            LLQNotifier.Default.Notify(new OpenNewStoryEvent());
        }

        public Visibility ToolBarVisibility { get { return Config.IsSinglePageStatus(CurrentUIStatus) ? Visibility.Visible : Visibility.Collapsed; } }

        public AppUIStatus CurrentUIStatus { get; set; }

        [SubscriberCallback(typeof(StoryEvent))]
        private void Subscriber(StoryEvent param)
        {
            switch (param.Type)
            {
                case StoryEventType.Comment:
                    if (Config.IsSinglePageStatus(CurrentUIStatus))
                    {
                        Frame rootFrame = App.GetWindowFrame();
                        if (rootFrame == null)
                            return;

                        rootFrame.Navigate(typeof(CommentPage));
                    }
                    break;
                case StoryEventType.Share:
                   // WebViewUtil._webViewInstance.Navigate(new System.Uri("http://service.weibo.com/share/share.php?appkey=2615126550&title=%E5%88%BB%E5%A5%87%EF%BC%88Kitsch%EF%BC%89%E6%98%AF%E4%BB%80%E4%B9%88%EF%BC%9F%E5%A6%82%E4%BD%95%E5%85%8B%E6%9C%8D%E5%88%BB%E5%A5%87%EF%BC%9F+-+%E5%9B%9E%E7%AD%94%E4%BD%9C%E8%80%85%EF%BC%9A%E5%8A%A8%E6%9C%BA%E5%9C%A8%E6%9D%AD%E5%B7%9E+http%3A%2F%2Fzhihu.com%2Fquestion%2F27039705%2Fanswer%2F35506915%3Futm_campaign%3Dwebshare%26utm_source%3Dweibo%26utm_medium%3Dzhihu%EF%BC%88%E6%83%B3%E7%9C%8B%E6%9B%B4%E5%A4%9A%EF%BC%9F%E4%B8%8B%E8%BD%BD%E7%9F%A5%E4%B9%8E+App%EF%BC%9Ahttp%3A%2F%2Fweibo.com%2Fp%2F100404711598%EF%BC%89"));
                    break;
            }
        }
    }
}
