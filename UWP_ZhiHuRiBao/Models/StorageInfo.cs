using Brook.ZhiHuRiBao.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brook.ZhiHuRiBao.Models
{
    public class StorageInfo
    {
        public ZhiHuAuthoInfo ZhiHuAuthoInfo { get; set; }

        public LoginType LoginType { get; set; } = LoginType.None;

        public bool IsCommentPanelOpen { get; set; } = true;

        public bool IsZhiHuAuthoVaild() { return ZhiHuAuthoInfo != null && !string.IsNullOrEmpty(ZhiHuAuthoInfo.access_token); }
    }
}
