using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Brook.ZhiHuRiBao.Common
{
    public class ToolBarTrigger : StateTriggerBase
    {
        public ToolBarHost TargetToolBarHost { get; set; } = ToolBarHost.MainPage;

        public ToolBarHost CurrentToolBarHost
        {
            get { return (ToolBarHost)GetValue(CurrentToolBarHostProperty); }
            set { SetValue(CurrentToolBarHostProperty, value); }
        }
        public static readonly DependencyProperty CurrentToolBarHostProperty =
            DependencyProperty.Register("CurrentToolBarHost", typeof(ToolBarHost), typeof(ToolBarTrigger), new PropertyMetadata(ToolBarHost.MainPage, (s, e)=>
            {
                var trigger = (ToolBarTrigger)s;
                trigger.SetActive((ToolBarHost)e.NewValue == trigger.TargetToolBarHost);
            }));
    }
}
