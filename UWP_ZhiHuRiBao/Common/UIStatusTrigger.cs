using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Brook.ZhiHuRiBao.Common
{
    public class UIStatusTrigger : StateTriggerBase
    {
        public AppUIStatus UIStatus
        {
            get { return (AppUIStatus)GetValue(UIStatusProperty); }
            set { SetValue(UIStatusProperty, value); }
        }
        public static readonly DependencyProperty UIStatusProperty =
            DependencyProperty.Register("UIStatus", typeof(AppUIStatus), typeof(UIStatusTrigger), new PropertyMetadata(AppUIStatus.List, (s, e)=>
            {
                (s as UIStatusTrigger).SetActive((AppUIStatus)e.NewValue == AppUIStatus.List);
            }));
    }
}
