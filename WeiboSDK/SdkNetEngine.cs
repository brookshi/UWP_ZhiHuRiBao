/*
 * Author: hljie.
 * 
 * Description: 新浪微博Open API一般数据请求.
 * 
 *  主要包括参数处理，数据处理和异常结果处理。
 * 
 * Date: 2013.1.30
 * 
 * 
 * ===============================================================================
 * Copyright (c) Sina. 
 * All rights reserved.
 * ===============================================================================
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net.Http;
using Windows.Storage;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using RestSharp;
using System.IO;

namespace WeiboSDKForWinRT
{
    /// <summary>
    /// 负责处理各种请求微博.
    /// </summary>
    public sealed class SdkNetEngine
    {
        #region 成员变量
        private static RestClient m_Client = new RestClient();
        private static SdkResponse mSdkResponse;
        #endregion

        /// <summary>
        /// 构造实例.
        /// </summary>
        public SdkNetEngine()
        {
            // 初始化AccessToken.
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            SdkData.AccessToken = localSettings.Values[Constants.ACCESSTOKEN] as string;
        }

        /// <summary>
        /// 组参.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<bool> ConfigParams(RestRequest request, SdkRequestType type, ISdkCmdBase data)
        {
            Action<string> errAction = (e1) =>
            {
                mSdkResponse.content = e1;
                mSdkResponse.errCode = SdkErrCode.XPARAM_ERR;
            };

            if (null == request)
            {
                errAction(Constants.SdkMsg.SDKInnerErr);
                return false;
            }
            m_Client.BaseUrl = Constants.PublicApiUrl;
            if (SdkData.AccessToken != null)
                request.AddParameter("access_token", SdkData.AccessToken);
            else
            {
                errAction(Constants.SdkMsg.TokenNull);
                return false;
            }

            switch (type)
            {
                case SdkRequestType.POST_MESSAGE:
                    {
                        CmdPostMessage message = null;
                        if (data is CmdPostMessage)
                            message = data as CmdPostMessage;
                        else
                        {
                            errAction(Constants.SdkMsg.ParamTypeErr);
                            return false;
                        }
                        request.Method = Method.POST;
                        request.Resource = Constants.WeiboAPI.PostMsg;

                        if (message.Status.Length > 0)
                            request.AddParameter("status", message.Status);
                        if (message.ReplyId.Length > 0)
                            request.AddParameter("in_reply_to_status_id", message.ReplyId);
                        if (message.Lat.Length > 0)
                            request.AddParameter("lat", message.Lat);
                        if (message.Long.Length > 0)
                            request.AddParameter("long", message.Long);
                        if (message.Annotations.Length > 0)
                            request.AddParameter("annotations", message.Annotations);
                    }
                    break;
                case SdkRequestType.POST_MESSAGE_PIC:
                    {
                        CmdPostMsgWithPic message = null;
                        if (data is CmdPostMsgWithPic)
                            message = data as CmdPostMsgWithPic;
                        else
                        {
                            errAction(Constants.SdkMsg.ParamTypeErr);
                            return false;
                        }
                        request.Method = Method.POST;
                        request.Resource = Constants.WeiboAPI.PostMsgPic;

                        request.AddParameter("status", message.Status);

                        if (message.Lat.Length > 0)
                            request.AddParameter("lat", message.Lat);
                        if (message.Long.Length > 0)
                            request.AddParameter("long", message.Long);

                        if (0 == message.PicPath.Length)
                        {
                            errAction(Constants.SdkMsg.PicPathEmpty);
                            return false;
                        }

                        string picType = System.IO.Path.GetExtension(message.PicPath);
                        string picName = System.IO.Path.GetFileName(message.PicPath);

                        await request.AddFileAsync("pic", message.PicPath);
                    }
                    break;

                case SdkRequestType.FRIENDS_TIMELINE:
                    {
                        CmdTimelines message = null;
                        if (data is CmdTimelines)
                            message = data as CmdTimelines;
                        else
                        {
                            errAction(Constants.SdkMsg.ParamTypeErr);
                            return false;
                        }
                        request.Resource = Constants.WeiboAPI.FriendTimeline;
                        request.Method = Method.GET;
                        if (message.SinceID.Length > 0)
                        {
                            request.AddParameter("since_id", message.SinceID);
                        }
                        if (message.MaxID.Length > 0)
                        {
                            request.AddParameter("max_id", message.MaxID);
                        }
                        if (message.Count.Length > 0)
                        {
                            request.AddParameter("count", message.Count);
                        }
                        if (message.Page.Length > 0)
                            request.AddParameter("page", message.Page);
                        if (message.BaseApp.Length > 0)
                            request.AddParameter("base_app", message.BaseApp);
                        if (message.Feature.Length > 0)
                            request.AddParameter("feature", message.Feature);
                    }
                    break;
                case SdkRequestType.USER_TIMELINE:
                    {
                        CmdUserTimeline message = null;
                        if (data is CmdUserTimeline)
                            message = data as CmdUserTimeline;
                        else
                        {
                            errAction(Constants.SdkMsg.ParamTypeErr);
                            return false;
                        }
                        request.Resource = Constants.WeiboAPI.UserTimeline;
                        request.Method = Method.GET;

                        if (message.UserId.Length > 0)
                            request.AddParameter("user_id", message.UserId);
                        if (message.ScreenName.Length > 0)
                            request.AddParameter("screen_name", message.ScreenName);
                        if (message.SinceID.Length > 0)
                        {
                            request.AddParameter("since_id", message.SinceID);
                        }
                        if (message.MaxID.Length > 0)
                        {
                            request.AddParameter("max_id", message.MaxID);
                        }
                        if (message.Count.Length > 0)
                        {
                            request.AddParameter("count", message.Count);
                        }

                        if (message.Page.Length > 0)
                            request.AddParameter("page", message.Page);
                        if (message.BaseApp.Length > 0)
                            request.AddParameter("base_app", message.BaseApp);
                        if (message.Feature.Length > 0)
                            request.AddParameter("feature", message.Feature);

                    }
                    break;
                case SdkRequestType.FRIENDSHIP_CREATE:
                    {
                        CmdFriendShip message = null;
                        if (data is CmdFriendShip)
                            message = data as CmdFriendShip;
                        else
                        {
                            errAction(Constants.SdkMsg.ParamTypeErr);
                            return false;
                        }
                        request.Resource = Constants.WeiboAPI.CreateFriendShip;
                        request.Method = Method.POST;

                        if (message.UserId.Length > 0)
                            request.AddParameter("user_id", message.UserId);
                        if (message.ScreenName.Length > 0)
                            request.AddParameter("screen_name", message.ScreenName);

                    }
                    break;
                case SdkRequestType.FRIENDSHIP_DESDROY:
                    {
                        CmdFriendShip message = null;
                        if (data is CmdFriendShip)
                            message = data as CmdFriendShip;
                        else
                        {
                            errAction(Constants.SdkMsg.ParamTypeErr);
                            return false;
                        }
                        request.Resource = Constants.WeiboAPI.DestroyFriendShip;
                        request.Method = Method.POST;

                        if (message.UserId.Length > 0)
                            request.AddParameter("user_id", message.UserId);
                        if (message.ScreenName.Length > 0)
                            request.AddParameter("screen_name", message.ScreenName);
                    }
                    break;
                case SdkRequestType.FRIENDSHIP_SHOW:
                    {
                        CmdFriendShip message = null;
                        if (data is CmdFriendShip)
                            message = data as CmdFriendShip;
                        else
                        {
                            errAction(Constants.SdkMsg.ParamTypeErr);
                            return false;
                        }
                        request.Resource = Constants.WeiboAPI.ShowFriendShip;
                        request.Method = Method.GET;

                        if (message.SourceId.Length > 0)
                            request.AddParameter("source_id", message.SourceId);
                        if (message.SourceScreenName.Length > 0)
                            request.AddParameter("source_screen_name", message.SourceScreenName);

                        if (message.UserId.Length > 0)
                            request.AddParameter("target_id", message.UserId);
                        if (message.ScreenName.Length > 0)
                            request.AddParameter("target_screen_name", message.ScreenName);
                    }
                    break;

                case SdkRequestType.AT_USERS:
                    {
                        CmdAtUsers atUsers = null;
                        if (data is CmdAtUsers)
                            atUsers = data as CmdAtUsers;
                        else
                        {
                            errAction(Constants.SdkMsg.ParamTypeErr);
                            return false;
                        }
                        request.Resource = Constants.WeiboAPI.AtUser;
                        request.Method = Method.GET;
                        if (atUsers.Keyword.Length > 0)
                            request.AddParameter("q", atUsers.Keyword);
                        if (atUsers.Count.Length > 0)
                            request.AddParameter("count", atUsers.Count);
                        if (atUsers.Range.Length > 0)
                            request.AddParameter("range", atUsers.Range);
                        if (atUsers.Type.Length > 0)
                            request.AddParameter("type", atUsers.Type);

                    }
                    break;
                case SdkRequestType.SHORTEN_URL:
                    {
                        CmdShortenUrl shortUrl = null;
                        if (data is CmdShortenUrl)
                            shortUrl = data as CmdShortenUrl;
                        else
                        {
                            errAction(Constants.SdkMsg.ParamTypeErr);
                            return false;
                        }
                        request.Resource = Constants.WeiboAPI.ShortenUrl;
                        request.Method = Method.GET;
                        if (shortUrl.OriginalUrl.Length > 0)
                            request.AddParameter("url_long", shortUrl.OriginalUrl);

                    }
                    break;
                case SdkRequestType.OTHER_API:
                    {
                        ICustomCmdBase customData = null;
                        if (data is ICustomCmdBase)
                            customData = data as ICustomCmdBase;
                        else
                        {
                            errAction(Constants.SdkMsg.ParamTypeErr);
                            return false;
                        }
                        customData.ConvertToRequestParam(request);
                    }
                    break;
                default:
                    {
                        errAction(Constants.SdkMsg.RequestNotSupppt);
                        return false;
                    }
            }
            request.AddParameter("curtime", DateTime.Now.Ticks.ToString());
            return true;
        }

        /// <summary>
        /// 处理数据返回.
        /// </summary>
        /// <param name="response"></param>
        private void HandleEx(RestResponse response)
        {
            try
            {
                if (response.StatusCode == (int)HttpStatusCode.RequestTimeout ||
                    response.StatusCode == (int)HttpStatusCode.GatewayTimeout)
                {
                    mSdkResponse.errCode = SdkErrCode.TIMEOUT;
                    mSdkResponse.content = Constants.SdkMsg.ConnectTimeOut;

                }
                //未知异常(自定义)
                else if (response.StatusCode != (int)HttpStatusCode.OK || null != response.ErrorException)
                {
                    bool isUserCanceled = false;
                    mSdkResponse.errCode = SdkErrCode.NET_UNUSUAL;
                    mSdkResponse.content = Constants.SdkMsg.NetException;

                    if (response.ErrorException is WebException)
                    {
                        WebException ex = response.ErrorException as WebException;
                        if (WebExceptionStatus.RequestCanceled == ex.Status)
                        {
                            mSdkResponse.errCode = SdkErrCode.USER_CANCEL;
                            mSdkResponse.content = Constants.SdkMsg.WebRequestCanceled;
                            isUserCanceled = true;
                        }
                    }

                    if (!isUserCanceled)
                    {
                        try
                        {
                            ErrorRes resObject = null;
                            if (response.Request.Resource.Contains(".xml") || response.Request.Resource.Contains(".XML"))
                            {
                                // Note: 推荐json格式;
                            }
                            else
                            {
                                DataContractJsonSerializer ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(ErrorRes));
                                using (MemoryStream ms = new MemoryStream(UnicodeEncoding.UTF8.GetBytes(response.Content)))
                                {
                                    resObject = ser.ReadObject(ms) as ErrorRes;
                                }
                            }

                            if (null != resObject && resObject is ErrorRes)
                            {
                                mSdkResponse.errCode = SdkErrCode.SERVER_ERR;
                                mSdkResponse.specificCode = resObject.ErrCode;
                                mSdkResponse.content = resObject.ErrInfo;
                            }
                            else
                                throw new Exception();
                        }
                        catch//如果没有error_code这个节点...
                        {
                            //不是xml
                            //网络异常时统一错误：NET_UNUSUAL
                            if (response.StatusCode == (int)HttpStatusCode.NotFound)
                            {
                                mSdkResponse.errCode = SdkErrCode.NET_UNUSUAL;
                                mSdkResponse.content = Constants.SdkMsg.NetException;
                            }
                            else
                            {
                                mSdkResponse.errCode = SdkErrCode.SERVER_ERR;
                                mSdkResponse.specificCode = response.StatusCode.ToString();
                                mSdkResponse.content = response.Content;
                            }
                        }
                    }
                }
                else
                {
                    mSdkResponse.content = response.Content;
                }
            }
            catch (Exception e)
            {
                mSdkResponse.errCode = SdkErrCode.SERVER_ERR;
                mSdkResponse.content = Constants.SdkMsg.ServerError;

            }
        }

        /// <summary>
        /// 普通数据请求
        /// </summary>
        /// <param name="type">请求数据类型</param>
        /// <param name="data">参数包(SdkCmdBase的子类)</param>
        /// <returns>请求返回数据包括返回数据和错误信息</returns>
        public IAsyncOperation<SdkResponse> RequestCmd(SdkRequestType type, ISdkCmdBase data)
        {
            return System.Runtime.InteropServices.WindowsRuntime.AsyncInfo.Run<SdkResponse>((token) =>
                Task.Run<SdkResponse>(async () =>
                {
                    RestRequest request = new RestRequest();
                    mSdkResponse = new SdkResponse();

                    bool retValue = ConfigParams(request, type, data).Result;

                    if (false != retValue)
                    {
                        if (null != data && !string.IsNullOrEmpty(SdkData.AccessToken))
                        {
                            request.AddHeader("Authorization", string.Format("OAuth2 {0}", SdkData.AccessToken));
                        }
                        else
                        {
                            //如需鉴权传入OAuth2.0的accessToken
                            request.AddParameter("source", SdkData.AppKey);
                        }

                        IRestResponse response = await m_Client.ExecuteAsync(request);
                        HandleEx(response as RestResponse);
                    }
                    else if (mSdkResponse.errCode == SdkErrCode.XPARAM_ERR &&
                            mSdkResponse.content == Constants.SdkMsg.TokenNull)
                    {
                        // 重新授权.
                        ClientOAuth oauth = new ClientOAuth();
                        oauth.BeginOAuth();
                    }
                    return mSdkResponse;
                }, token));
        }
    }
}
