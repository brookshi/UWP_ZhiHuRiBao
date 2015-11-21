using Brook.ZhiHuRiBao.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.ViewManagement;

namespace Brook.ZhiHuRiBao.Utils
{
    public class Config
    {
        public static int MinWidth_UIStatus_All { get { return 1300; } }

        public static int MinWidth_UIStatus_ListAndContent { get { return 1000; } }

        public static int MinWidth_UIStatus_List { get { return 0; } }

        public static double ScreenWidth
        {
            get
            {
                return ApplicationView.GetForCurrentView().VisibleBounds.Width;
            }
        }

        public static AppUIStatus UIStatus
        {
            get
            {
                if (ScreenWidth >= MinWidth_UIStatus_All)
                    return AppUIStatus.All;
                else if (ScreenWidth >= MinWidth_UIStatus_ListAndContent && ScreenWidth < MinWidth_UIStatus_All)
                    return AppUIStatus.ListAndContent;
                else
                    return AppUIStatus.List;
            }
        }

        public static bool IsSinglePage { get { return UIStatus == AppUIStatus.List; } }

        public static bool IsSinglePageStatus(AppUIStatus uiStatus)
        {
            return uiStatus == AppUIStatus.List;
        }

        public static bool IsPageSwitched(Size previousSize, Size newSize)
        {
            return previousSize.Width < MinWidth_UIStatus_ListAndContent && newSize.Width > MinWidth_UIStatus_ListAndContent;
        }
    }
}
