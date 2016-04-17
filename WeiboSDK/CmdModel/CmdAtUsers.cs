using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiboSDKForWinRT
{    
    /// <summary>
    /// @好友参数类.
    /// </summary>
    public sealed class CmdAtUsers : ISdkCmdBase
    {
        private string _keyword = string.Empty;
        /// <summary>
        /// 搜索的关键字。必须进行URL_encoding。UTF-8编码
        /// </summary>
        public string Keyword
        {
            get { return _keyword; }
            set { _keyword = value; }
        }


        private string _count = string.Empty;
        /// <summary>
        /// 每页返回结果数。默认10
        /// </summary>
        public string Count
        {
            get { return _count; }
            set { _count = value; }
        }

        private string _type = string.Empty;
        /// <summary>
        /// 1代表粉丝，0代表关注人
        /// </summary>  
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _range = string.Empty;
        /// <summary>
        /// 0代表只查关注人，1代表只搜索当前用户对关注人的备注，2表示都查. 默认为2.
        /// </summary>
        public string Range
        {
            get { return _range; }
            set { _range = value; }
        }

    }

}
