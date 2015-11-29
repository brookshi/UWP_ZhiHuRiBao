using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brook.ZhiHuRiBao.Models
{
    public class StoryExtraInfo
    {
        public int post_reasons { get; set; }
        public int popularity { get; set; }
        public bool favorite { get; set; }
        public int normal_comments { get; set; }
        public int comments { get; set; }
        public int short_comments { get; set; }
        public int vote_status { get; set; }
        public int long_comments { get; set; }
    }
}
