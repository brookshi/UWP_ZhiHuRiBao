using Brook.ZhiHuRiBao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brook.ZhiHuRiBao.Authorization
{
    public interface IAuthorize
    {
        void Login(Action<bool, object> loginCallback);

        void Logout();

        LoginData LoginData { get; }

        bool IsAuthorized { get; }
    }
}
