using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiboSDKForWinRT
{ 
    /// <summary>
    /// 当前登录用户微博列表参数类.
    /// </summary>
    public sealed class CmdUserTimeline : ISdkCmdBase
    {
        private string userId = string.Empty;
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private string screenName = string.Empty;

        public string ScreenName
        {
            get { return screenName; }
            set { screenName = value; }
        }

        private string sinceID = string.Empty;

        public string SinceID
        {
            get { return sinceID; }
            set { sinceID = value; }
        }

        private string maxID = string.Empty;

        public string MaxID
        {
            get { return maxID; }
            set { maxID = value; }
        }

        private string count = string.Empty;

        public string Count
        {
            get { return count; }
            set { count = value; }
        }

        private string page = string.Empty;

        public string Page
        {
            get { return page; }
            set { page = value; }
        }

        private string baseApp = string.Empty;

        public string BaseApp
        {
            get { return baseApp; }
            set { baseApp = value; }
        }

        private string feature = string.Empty;

        public string Feature
        {
            get { return feature; }
            set { feature = value; }
        }

    }
}
