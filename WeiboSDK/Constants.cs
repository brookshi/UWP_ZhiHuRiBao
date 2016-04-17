using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiboSDKForWinRT
{
    internal class Constants
    {
        internal const string ServerUrl2_0 = "https://api.weibo.com";
        internal const string PublicApiUrl = "https://api.weibo.com/2";
        internal const string XAuthUrl = "http://api.t.sina.com.cn";

        internal const string ACCESSTOKEN = "access_token";
        internal const string EXPIREDTIME = "expires_in";
        internal const string LASTAUTHTIME = "last_oauth_time";
        internal const string REFRESHTOKEN = "refresh_token";

        /// <summary>
        /// 微博开放平台API.
        /// </summary>
        internal class WeiboAPI
        {
            internal const string PostMsgPic = "/statuses/upload.json";
            internal const string FriendTimeline = "/statuses/friends_timeline.json";
            internal const string UserTimeline = "/statuses/user_timeline.json";
            internal const string PostMsg = "/statuses/update.json";
            internal const string CreateFriendShip = "/friendships/create.json";
            internal const string DestroyFriendShip = "/friendships/destroy.json";
            internal const string ShowFriendShip = "/friendships/show.json";
            internal const string AtUser = "/search/suggestions/at_users.json";
            internal const string ShortenUrl = "/short_url/shorten.json";
        }

        /// <summary>
        /// SDK提示信息.
        /// </summary>
        internal class SdkMsg
        {
            internal const string ConnectTimeOut = "连接超时";
            internal const string NetException = "网络异常";
            internal const string WebRequestCanceled = "网络请求被取消";
            internal const string ServerError = "服务器返回信息异常";
            internal const string SDKInnerErr = "发生内部错误";
            internal const string TokenNull = "Access_Token为空";
            internal const string ParamTypeErr = "参数类型错误";
            internal const string RequestNotSupppt = "此请求尚不支持.";
            internal const string OAuthUserCanceled = "用户取消授权";
            internal const string MissParam = "缺少必要参数";
            internal const string PicPathEmpty = "图片地址为空";
        }
    }
}
