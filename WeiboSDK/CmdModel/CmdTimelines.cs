using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiboSDKForWinRT
{
    /// <summary>
    /// 好友微博列表参数类.
    /// </summary>
    public sealed class CmdTimelines : ISdkCmdBase
    {
        private string _id = string.Empty;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _sinceID = string.Empty;
        public string SinceID
        {
            get { return _sinceID; }
            set { _sinceID = value; }
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

        private string feature = string.Empty;
        public string Feature
        {
            get { return feature; }
            set { feature = value; }
        }

        private string baseApp = string.Empty;
        public string BaseApp
        {
            get { return baseApp; }
            set { baseApp = value; }
        }
    }
}
