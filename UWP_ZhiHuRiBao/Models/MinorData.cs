using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brook.ZhiHuRiBao.Models
{
    public class MinorData
    {
        public bool subscribed { get; set; }
        public List<Story> stories { get; set; }
        public string description { get; set; }
        public string background { get; set; }
        public int color { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string image_source { get; set; }
    }
}
