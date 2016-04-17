using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiboSDKForWinRT
{  
    /// <summary>
    /// 发送状态参数类.
    /// </summary>
    public sealed class CmdPostMessage : ISdkCmdBase
    {
        private string status = string.Empty;

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private string replyId = string.Empty;

        public string ReplyId
        {
            get { return replyId; }
            set { replyId = value; }
        }

        private string lat = string.Empty;

        public string Lat
        {
            get { return lat; }
            set { lat = value; }
        }

        private string _long = string.Empty;

        public string Long
        {
            get { return _long; }
            set { _long = value; }
        }

        private string annotations = string.Empty;

        public string Annotations
        {
            get { return annotations; }
            set { annotations = value; }
        }

    }
}
