using Brook.ZhiHuRiBao.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiboSDKForWinRT;
using Windows.Storage;

namespace Brook.ZhiHuRiBao.Authorization
{
    [AuthoAttribution(LoginType.Sina)]
    public class SinaAuthorization : IAuthorize
    {
        public const string SinaTokenKey = "SinaToken";

        public readonly static SinaAuthorization Instance = new SinaAuthorization();

        private ClientOAuth oauth = new ClientOAuth();

        static SinaAuthorization()
        {
            SdkData.AppKey = "2626289114";
            SdkData.AppSecret = "d0b05d8a84f64b2ef509dc1934f7c3a1";
            SdkData.RedirectUri = "http://sns.whalecloud.com/sina2/callback";
        }

        public SinaAuthorization()
        {
            string token;
            if(StorageUtil.TryGet(SinaTokenKey, out token))
            {
                Token = token;
            }
        }

        public string Token
        {
            get;
            private set;
        }

        public bool IsIsAuthorized
        {
            get
            {
                return oauth.IsAuthorized;
            }
        }

        public void Login(Action<bool, object> loginCallback)
        {
            oauth.LoginCallback += (isSuccess, err, response) =>
            {
                if(isSuccess)
                {
                    Token = response.AccessToken;
                    StorageUtil.Add(SinaTokenKey, Token);
                }
                if(loginCallback != null)
                {
                    loginCallback(isSuccess, response);
                }
            };
            oauth.BeginOAuth();
        }

        public void Logout(Action<bool, object> logoutCallback)
        {
            Token = null;
            StorageUtil.Remove(SinaTokenKey);
        }
    }
}
