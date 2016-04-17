using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;

namespace WeiboSDKForWinRT
{
    /// <summary>
    /// OAuth2授权回调.
    /// </summary>
    /// <param name="isSucess">是否成功</param>
    /// <param name="err">错误信息</param>
    /// <param name="response">返回数据</param>
    public delegate void OAuth2LoginBack(bool isSucess, SdkAuthError err, SdkAuth2Res response);

    /// <summary>
    /// 请求返回数据类.
    /// </summary>
    public sealed class SdkResponse
    {
        private SdkErrCode _errCode;
        /// <summary>
        /// 微博SDK定义错误类型.
        /// </summary>
        public SdkErrCode errCode
        {
            get { return _errCode; }
            set { _errCode = value; }
        }

        private string _specificCode;

        public string specificCode
        {
            get { return _specificCode; }
            set { _specificCode = value; }
        }

        private string _content;

        public string content
        {
            get { return _content; }
            set { _content = value; }
        }
    }

    /// <summary>
    /// 授权结果信息.
    /// </summary>
    public sealed class SdkAuthError
    {
        private SdkErrCode _errCode;

        public SdkErrCode errCode
        {
            get { return _errCode; }
            set { _errCode = value; }
        }

        private string _specificCode;

        public string specificCode
        {
            get { return _specificCode; }
            set { _specificCode = value; }
        }
        private string _errMessage;

        public string errMessage
        {
            get { return _errMessage; }
            set { _errMessage = value; }
        }
    }

    /// <summary>
    /// OAuth2返回结果.
    /// </summary>
    [DataContract]
    public sealed class SdkAuth2Res
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "remind_in")]
        public string RemindIn { get; set; }

        [DataMember(Name = "expires_in")]
        public string ExpriesIn { get; set; }

        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        [DataMember(Name = "uid")]
        public string Uid { get; set; }
    }

    /// <summary>
    /// 授权失败时返回的对象.
    /// </summary>
    [DataContract]
    public sealed class OAuthErrRes
    {
        [DataMember(Name = "error")]
        public string Error { get; set; }

        [DataMember(Name = "error_code")]
        public string ErrorCode { get; set; }

        [DataMember(Name = "error_description")]
        public string errDes { get; set; }
    }

    /// <summary>
    /// 授权失败时返回的对象(外部接口).
    /// </summary>
    [XmlRoot("hash")]
    [DataContract]
    public sealed class ErrorRes
    {
        [XmlElement("request")]
        [DataMember(Name = "request")]
        public string Request { get; set; }

        [XmlElement("error_code")]
        [DataMember(Name = "error_code")]
        public string ErrCode { get; set; }

        [XmlElement("error")]
        [DataMember(Name = "error")]
        public string ErrInfo { get; set; }

    }
}
