/*
 * Author: hljie.
 * 
 * Description: 新浪微博OAuth2.0授权。
 *  
 *  采用官方授权页方式，包括了授权Token的过期验证。
 * 
 * Date: 2013.1.30
 * 
 * 
 * ===============================================================================
 * Copyright (c) Sina. 
 * All rights reserved.
 * ===============================================================================
 */
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Storage;

namespace WeiboSDKForWinRT
{
    /// <summary>
    /// 授权登陆类，Token过期检查.推荐使用OAuth2.0授权登陆.
    /// </summary>
    public sealed class ClientOAuth
    {
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        /// <summary>
        /// 授权回调.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event OAuth2LoginBack LoginCallback;

        /// <summary>
        /// 是否用户授权(包含授权过期的状态).
        /// </summary>
        public bool IsAuthorized { get; set; }

        /// <summary>
        /// 包含检查Token是否过期.
        /// </summary>
        public ClientOAuth()
        {
            CheckTokenValid();
        }

        #region 官方授权页方式授权.
        /// <summary>
        /// 授权登陆.
        /// </summary>
        public void BeginOAuth()
        {
            // 必要参数检查.
            if (string.IsNullOrEmpty(SdkData.AppKey) ||
                string.IsNullOrEmpty(SdkData.AppSecret) ||
                string.IsNullOrEmpty(SdkData.RedirectUri))
            {

                SdkAuthError err = new SdkAuthError();
                err.errCode = SdkErrCode.XPARAM_ERR;
                err.errMessage = Constants.SdkMsg.MissParam;

                if (null != LoginCallback)
                    LoginCallback(false, err, null);
                return;
            }

            GetAuthorizeCode();
        }

        /// <summary>
        /// 授权获取authorize_code.
        /// </summary>
        private async void GetAuthorizeCode()
        {
            string oauthUrl = string.Format("{0}/oauth2/authorize?client_id={1}&response_type=code&redirect_uri={2}&display=mobile"
                    , Constants.ServerUrl2_0, SdkData.AppKey, SdkData.RedirectUri);

            Uri startUri = new Uri(oauthUrl, UriKind.Absolute);
            Uri endUri = new Uri(SdkData.RedirectUri, UriKind.Absolute);

            // 调出认证页面.
            var authenResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startUri, endUri);

            switch (authenResult.ResponseStatus)
            {
                case WebAuthenticationStatus.Success:
                    {
                        string authorize_code = string.Empty;
                        var data = authenResult.ResponseData;

                        authorize_code = SdkUility.GetQueryParameter(data, "code");

                        if (string.IsNullOrEmpty(authorize_code) == false)
                        {
                            Authorize(authorize_code);
                        }
                    }
                    break;
                case WebAuthenticationStatus.UserCancel:
                    {
                        SdkAuthError err = new SdkAuthError();
                        err.errCode = SdkErrCode.USER_CANCEL;
                        err.errMessage = Constants.SdkMsg.OAuthUserCanceled;

                        if (null != LoginCallback)
                            LoginCallback(false, err, null);
                    }
                    break;
                case WebAuthenticationStatus.ErrorHttp:
                default:
                    {
                        SdkAuthError err = new SdkAuthError();
                        err.errCode = SdkErrCode.NET_UNUSUAL;
                        err.errMessage = Constants.SdkMsg.NetException;

                        if (null != LoginCallback)
                            LoginCallback(false, err, null);
                    }
                    break;
            }
        }

        /// <summary>
        /// authorize_code换取AccesToken.
        /// </summary>
        /// <param name="auth_code"></param>
        private async void Authorize(string auth_code)
        {
            RestClient client = new RestClient();
            client.BaseUrl = Constants.ServerUrl2_0;

            RestRequest request = new RestRequest();
            request.Resource = "/oauth2/access_token";
            request.Method = Method.POST;

            request.AddParameter("client_id", SdkData.AppKey);
            request.AddParameter("client_secret", SdkData.AppSecret);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("redirect_uri", SdkData.RedirectUri);
            request.AddParameter("code", auth_code);

            IRestResponse result = await client.ExecuteAsync(request);
            RestResponse response = result as RestResponse;
            if (response != null)
            {
                HandleResult(response);
            }
        }

        #endregion

        /// <summary>
        /// 处理授权结果.
        /// </summary>
        /// <param name="response"></param>
        private void HandleResult(RestResponse response)
        {
            SdkAuthError err = new SdkAuthError();
            string responseStr = response.Content;
            if (response.StatusCode != (int)HttpStatusCode.OK || response.ErrorException != null)
            {
                if (response.ContentLength == 0D)
                {
                    err.errCode = SdkErrCode.NET_UNUSUAL;
                    if (null != LoginCallback)
                        LoginCallback(false, err, null);
                    return;
                }
                // 解析错误信息.
                OAuthErrRes errRes = SerializeOAuthResult<OAuthErrRes>(responseStr);

                err.errCode = SdkErrCode.SERVER_ERR;
                err.specificCode = errRes.ErrorCode;
                err.errMessage = errRes.errDes;

                if (null != LoginCallback)
                    LoginCallback(false, err, null);
            }
            else
            {
                err.errCode = SdkErrCode.SUCCESS;
                SdkAuth2Res oauthResult = SerializeOAuthResult<SdkAuth2Res>(responseStr);

                // 保存AccessToken.
                SdkData.AccessToken = oauthResult.AccessToken;
                localSettings.Values[Constants.ACCESSTOKEN] = oauthResult.AccessToken;
                localSettings.Values[Constants.EXPIREDTIME] = long.Parse(oauthResult.ExpriesIn);
                localSettings.Values[Constants.LASTAUTHTIME] = Unix2DateTime.GetUnixTimestamp();
                if (oauthResult.RefreshToken != null)
                {
                    localSettings.Values[Constants.REFRESHTOKEN] = oauthResult.RefreshToken;
                }

                if (null != LoginCallback)
                    LoginCallback(true, err, oauthResult);
            }
        }

        /// <summary>
        /// 检查AccessToken是否过期.
        /// </summary>
        private void CheckTokenValid()
        {
            try
            {
                if (localSettings.Values.ContainsKey(Constants.ACCESSTOKEN) == false)
                {
                    IsAuthorized = false;
                }
                else
                {
                    if (localSettings.Values.ContainsKey(Constants.EXPIREDTIME))
                    {
                        var expiredTime = (long)localSettings.Values[Constants.EXPIREDTIME];
                        var lastOAuthTime = (long)localSettings.Values[Constants.LASTAUTHTIME];
                        var nowTime = Unix2DateTime.GetUnixTimestamp();

                        if (nowTime >= lastOAuthTime + expiredTime)
                        {
                            IsAuthorized = false;
                        }
                        else
                        {
                            SdkData.AccessToken = localSettings.Values[Constants.ACCESSTOKEN] as string;
                            IsAuthorized = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                IsAuthorized = false;
            }
        }

        /// <summary>
        /// 解析授权返回结果.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        private T SerializeOAuthResult<T>(string responseStr) where T : class
        {
            T result;
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(UnicodeEncoding.UTF8.GetBytes(responseStr)))
            {
                result = ser.ReadObject(ms) as T;
            }
            return result;
        }
    }
}
