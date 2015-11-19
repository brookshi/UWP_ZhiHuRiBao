using Windows.UI.Xaml;

namespace Brook.ZhiHuRiBao.Common
{
    public class ToolBarTrigger : StateTriggerBase
    {
        public ToolBarHost TargetToolBarHost { get; set; } = ToolBarHost.MainPage;

        public ToolBarHost CurrentToolBarHost { get; set; }

        public double MaxWidth { get; set; }

        private FrameworkElement _targetElement;
        public FrameworkElement TargetElement
        {
            get { return _targetElement; }
            set
            {
                _targetElement = value;
                _targetElement.SizeChanged += (s, e)=> SetActive(e.NewSize.Width < MaxWidth && CurrentToolBarHost == TargetToolBarHost);
            }
        }
    }
}
