using Brook.ZhiHuRiBao.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brook.ZhiHuRiBao.ViewModels
{
    public class StorageInfo
    {
        public ZhiHuAuthoInfo ZhiHuAuthoInfo { get; set; }

        public LoginType LoginType { get; set; }

        public bool IsCommentPanelOpen { get; set; }
    }
}
