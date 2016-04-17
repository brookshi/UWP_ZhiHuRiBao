using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiboSDKForWinRT
{
    /// <summary>
    /// 数据参数.
    /// </summary>
    public sealed class SdkData
    {
        /// <summary>
        /// 授权之后的回调地址.
        /// </summary>
        static public string RedirectUri { get; set; }

        /// <summary>
        /// 微博开放平台申请的Appkey.
        /// </summary>
        static public string AppKey { get; set; }

        /// <summary>
        /// 微博开放平台申请的AppSecret.
        /// </summary>
        static public string AppSecret { get; set; }

        static internal string UserAgent { get; set; }
        //默认OAuth2.0
        static readonly internal EumAuth AuthOption = EumAuth.OAUTH2_0;

        private static string _accessToken;
        /// <summary>
        /// 授权之后的AccessToken
        /// </summary>
        internal static string AccessToken
        {
            get { return _accessToken; }
            set { _accessToken = value; }
        }
    }
}
