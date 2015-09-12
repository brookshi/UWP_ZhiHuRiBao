using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brook.ZhiHuRiBao.Utils
{
    public class Config
    {
        public static int MinRowCountForList = 20;

        public static double ScreenHeight { get; set; }

        public static double ScreenWidth { get; set; }

        public static int GetMaxRowCount(double rowHeight)
        {
            return Math.Max(MinRowCountForList, (int)(Math.Max(ScreenHeight, ScreenWidth) / rowHeight));
        }

        public static int GetMaxRowCountForMainList()
        {
            return GetMaxRowCount(100);
        }

        public static int CommentPageRowCount {
            get { return MinRowCountForList; }
        }
    }
}
