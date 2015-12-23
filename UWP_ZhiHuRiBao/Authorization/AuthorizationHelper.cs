#region License
//   Copyright 2015 Brook Shi
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License. 
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Windows.UI.Xaml;
using Brook.ZhiHuRiBao.Utils;
using Brook.ZhiHuRiBao.ViewModels;
using Brook.ZhiHuRiBao.Common;
using XPHttp;

namespace Brook.ZhiHuRiBao.Authorization
{
    public static class AuthorizationHelper
    {
        public static bool IsLogin = false;
        private readonly static Dictionary<LoginType, IAuthorize> Authorizations = new Dictionary<LoginType, IAuthorize>();

        static AuthorizationHelper()
        {
            var currentAssembly = Application.Current.GetType().GetTypeInfo().Assembly;

            var authorizeTypes = currentAssembly.DefinedTypes.Where(type => type.ImplementedInterfaces.Any(inter => inter == typeof(IAuthorize))).ToList();

            authorizeTypes.ForEach(o => Authorizations[GetLoginType(o)] = GetAuthorization(o));
        }

        private static LoginType GetLoginType(TypeInfo typeInfo)
        {
            return typeInfo.GetCustomAttribute<AuthoAttribution>().LoginType;
        }

        private static IAuthorize GetAuthorization(TypeInfo typeInfo)
        {
            return (IAuthorize)typeInfo.GetDeclaredField("Instance").GetValue(null);
        }

        public static void AutoLogin(Action loginSuccessCallback)
        {
            if (IsLogin)
                return;

            string msg = string.Empty;
            var loginType = StorageUtil.StorageInfo.LoginType;
            if (!CheckLoginType(loginType, out msg))
                return;

            var authorizer = Authorizations[loginType];
            if(!authorizer.IsAuthorized)
                return;

            if (StorageUtil.StorageInfo.IsZhiHuAuthoVaild())
            {
                IsLogin = true;
                SetHttpAuthorization();
                if (loginSuccessCallback != null)
                    loginSuccessCallback();
            }
            else if(Authorizations[loginType].LoginData != null)
            {
                Action<bool, object> callback = (isSuccess, res) => { if(loginSuccessCallback != null) loginSuccessCallback(); };
                LoginZhiHu(loginType, callback);
            }
        }

        public static void Login(LoginType loginType, Action<bool, object> loginCallback)
        {
            if (IsLogin)
                return;

            if (loginCallback == null)
                loginCallback = (b, o) => { };

            string msg = string.Empty;
            if (!CheckLoginType(loginType, out msg))
            {
                loginCallback(false, msg);
                return;
            }
            var authorizer = Authorizations[loginType];

            if (authorizer.IsAuthorized && Authorizations[loginType].LoginData != null)
            {
                LoginZhiHu(loginType, loginCallback);
            }
            else
            {
                try
                { 
                    authorizer.Login((isSuccess, res)=>
                    {
                        if(isSuccess)
                        {
                            LoginZhiHu(loginType, loginCallback);
                        }
                        else
                        {
                            loginCallback(false, StringUtil.GetString("LoginFailed"));
                        }
                    });
                }
                catch(Exception)
                {
                    loginCallback(false, StringUtil.GetString("LoginFailed"));
                }
            }
        }

        private static async void LoginZhiHu(LoginType loginType, Action<bool, object> loginCallback)
        {
            var zhiHuAuthoData = await DataRequester.Login(Authorizations[loginType].LoginData);
            if (zhiHuAuthoData == null)
            {
                loginCallback(false, StringUtil.GetString("LoginZhiHuFailed"));
                return;
            }
            StorageUtil.StorageInfo.LoginType = loginType;
            StorageUtil.StorageInfo.ZhiHuAuthoInfo = zhiHuAuthoData;
            StorageUtil.UpdateStorageInfo();

            IsLogin = true;
            SetHttpAuthorization();
            loginCallback(true, StringUtil.GetString("LoginSuccess"));
        }

        private static void SetHttpAuthorization()
        {
            XPHttpClient.DefaultClient.HttpConfig.SetAuthorization("Bearer", StorageUtil.StorageInfo.ZhiHuAuthoInfo.access_token);
        }

        public static void Logout()
        {
            IsLogin = false;
            string msg;
            var loginType = StorageUtil.StorageInfo.LoginType;

            if (CheckLoginType(loginType, out msg))
            {
                Authorizations[loginType].Logout();
            }
        }

        private static bool CheckLoginType(LoginType loginType, out string msg)
        {
            msg = string.Empty;
            if (loginType == LoginType.None)
            {
                msg = StringUtil.GetString("NoneLoginType");
                return false;
            }

            if (!Authorizations.ContainsKey(loginType))
            {
                msg = StringUtil.GetString("NotSupportLoginType");
                return false;
            }

            return true;
        }
    }
}
