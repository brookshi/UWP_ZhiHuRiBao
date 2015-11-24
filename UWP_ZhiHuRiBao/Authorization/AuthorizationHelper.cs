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

namespace Brook.ZhiHuRiBao.Authorization
{
    public static class AuthorizationHelper
    {
        private static LoginType CurrentLoginType { get; set; } = LoginType.None;

        public readonly static Dictionary<LoginType, IAuthorize> Authorizations = new Dictionary<LoginType, IAuthorize>();

        static AuthorizationHelper()
        {
            string loginType;
            if(StorageUtil.TryGet(StorageUtil.CurrentLoginTypeKey, out loginType))
            {
                CurrentLoginType = LoginTypeClass.ToEnum(loginType);
            }

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

        public static void AutoLogin(Action<bool, object> loginCallback)
        {
            if (!Authorizations.ContainsKey(CurrentLoginType))
                throw new NotSupportedException();

            Authorizations[CurrentLoginType].Login(loginCallback);
        }

        public static void Login(LoginType loginType, Action<bool, object> loginCallback)
        {
            if (loginType == LoginType.None)
            {
                if()
            }
        }
    }
}
