using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiboSDKForWinRT
{
    /// <summary>
    /// 微博SDK自定义的ErrCode.
    /// </summary>
    public enum SdkErrCode
    {
        /// <summary>
        /// 参数错误.
        /// </summary>
        XPARAM_ERR = -1,
        /// <summary>
        /// 成功.
        /// </summary>
        SUCCESS = 0,
        /// <summary>
        /// 网络不可用.
        /// </summary>
        NET_UNUSUAL,
        /// <summary>
        /// 服务器返回异常.
        /// </summary>
        SERVER_ERR,
        /// <summary>
        /// 访问超时.
        /// </summary>
        TIMEOUT,
        /// <summary>
        /// 用户请求被取消.
        /// </summary>
        USER_CANCEL
    }

    /// <summary>
    /// 请求类型.
    /// </summary>
    public enum SdkRequestType
    {
        NULL_TYPE = -1,
        /// <summary>
        /// 获取下行数据集(timeline)(cmdNormalMessages)
        /// </summary>
        FRIENDS_TIMELINE = 0,     
        /// <summary>
        /// 发送微博(cmdPostMessage)
        /// </summary>
        POST_MESSAGE,              
        /// <summary>
        /// 发送带图片微博(cmdPostMsgWithPic)
        /// </summary>
        POST_MESSAGE_PIC,   
        /// <summary>
        /// 关注某用户(cmdFriendShip)
        /// </summary>
        FRIENDSHIP_CREATE,          
        /// <summary>
        /// 取消关注(cmdFriendShip)
        /// </summary>
        FRIENDSHIP_DESDROY,        
        /// <summary>
        /// 获取两个用户关系的详细情况(cmdFriendShip)
        /// </summary>
        FRIENDSHIP_SHOW,           
        /// <summary>
        /// @用户时的联想建议 (cmdAtUsers)
        /// </summary>
        AT_USERS,                   
        /// <summary>
        /// 获取用户发布的微博消息列表(cdmUserTimeline)
        /// </summary>
        USER_TIMELINE,              
        /// <summary>
        /// 短链(cmdShortenUrl)
        /// </summary>
        SHORTEN_URL,                
        /// <summary>
        /// 除了上述其他API(继承自SdkCmdBase的自定义的参数类)
        /// </summary>
        OTHER_API                    
    }

    /// <summary>
    /// 鉴权方式.
    /// </summary>
    public enum EumAuth
    {
        OAUTH1_0 = 0,
        OAUTH2_0
    }

}
