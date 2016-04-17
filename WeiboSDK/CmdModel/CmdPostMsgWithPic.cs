using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiboSDKForWinRT
{ 
    /// <summary>
    /// 发送图片状态参数类.
    /// </summary>
    public sealed class CmdPostMsgWithPic : ISdkCmdBase
    {
        private string status = string.Empty;

        public string Status
        {
            get { return status; }
            set { status = value; }
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

        private string picPath = string.Empty;

        public string PicPath
        {
            get { return picPath; }
            set { picPath = value; }
        }

    }
}
