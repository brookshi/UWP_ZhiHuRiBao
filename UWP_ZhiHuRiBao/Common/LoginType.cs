using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brook.ZhiHuRiBao.Common
{
    public enum LoginType
    {
        None = 0,
        Sina,
        QQ,
    }

    public static class LoginTypeClass
    {
        public const string NoneLoginType = "None";
        public const string SinaLoginType = "sina";
        public const string QQLoginType = "qq";

        public static string Convert(this LoginType loginType)
        {
            switch(loginType)
            {
                case LoginType.Sina:
                    return SinaLoginType;
                case LoginType.QQ:
                    return QQLoginType;
                default:
                    throw new NotSupportedException();
            }
        }

        public static LoginType ToEnum(string loginType)
        {
            switch(loginType)
            {
                case SinaLoginType:
                    return LoginType.Sina;
                case QQLoginType:
                    return LoginType.QQ;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
