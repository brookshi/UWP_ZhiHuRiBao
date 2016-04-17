using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiboSDKForWinRT
{
    public interface ISdkCmdBase
    {
    }

    /// <summary>
    /// 自定义接口参数类型.
    /// </summary>
    public interface ICustomCmdBase : ISdkCmdBase
    {
        /// <summary>
        /// 自定义配置参数到RestRequest请求.
        /// </summary>
        /// <param name="request"></param>
        void ConvertToRequestParam(RestRequest request);

    }
}
