using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiboSDKForWinRT
{ 
    /// <summary>
    /// 好友关系参数类.
    /// </summary>
    public sealed class CmdFriendShip : ISdkCmdBase
    {
        //源用户(如果不填，则默认取当前登录用户)
        private string _sourceId = string.Empty;

        public string SourceId
        {
            get { return _sourceId; }
            set { _sourceId = value; }
        }

        private string _sourceScreenName = string.Empty;

        public string SourceScreenName
        {
            get { return _sourceScreenName; }
            set { _sourceScreenName = value; }
        }

        //目标用户
        private string _userId = string.Empty;

        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        private string _screenName = string.Empty;

        public string ScreenName
        {
            get { return _screenName; }
            set { _screenName = value; }
        }

    }
}
