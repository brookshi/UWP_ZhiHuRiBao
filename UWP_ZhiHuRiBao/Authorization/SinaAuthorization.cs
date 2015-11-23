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
        public readonly static SinaAuthorization Instance = new SinaAuthorization();

        static SinaAuthorization()
        {
            SdkData.AppKey = "2626289114";
            SdkData.AppSecret = "d0b05d8a84f64b2ef509dc1934f7c3a1";
            SdkData.RedirectUri = "http://sns.whalecloud.com/sina2/callback";
        }

        private ClientOAuth oauth = new ClientOAuth();

        public string Token
        {
            get
            {
                return null;
            }
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
                if(loginCallback != null)
                {
                    loginCallback(isSuccess, response);
                }
            };
            oauth.BeginOAuth();
        }

        public void Logout(Action<bool, object> logoutCallback)
        {
        }
    }
}
