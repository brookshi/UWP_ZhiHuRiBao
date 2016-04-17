using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiboSDKForWinRT
{ 
    /// <summary>
    /// 短链参数类.
    /// </summary>
    public sealed class CmdShortenUrl : ISdkCmdBase
    {
        private string originalUrl = string.Empty;

        public string OriginalUrl
        {
            get { return originalUrl; }
            set { originalUrl = value; }
        }

    }
}
