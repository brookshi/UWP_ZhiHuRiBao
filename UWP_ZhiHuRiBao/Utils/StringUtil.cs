#region License
//   Copyright 2015 Brook Shi
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License. 
#endregion

using Brook.ZhiHuRiBao.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Brook.ZhiHuRiBao.Utils
{
    public static class StringUtil
    {
        static ResourceLoader _resLoader = new ResourceLoader();

        public static string GetString(string id)
        {
            return _resLoader.GetString(id);
        }

        public static string GetStoryGroupName(string currentDate)
        {
            var date = DateTime.ParseExact(currentDate, "yyyyMMdd", null);
            if (date.Date.Equals(DateTime.Now.Date))
                return GetString("LatestNews");

            return date.Month + GetString("Month") + date.Day + GetString("Day") + " " + DateToWeek(date);
        }

        public static string GetCommentGroupName(CommentType type, string count)
        {
            return count + GetString("CommentItem") + (type == CommentType.Long ? GetString("LongComment") : GetString("ShortComment"));
        }

        public static string DateToWeek(DateTime date)
        {
            switch(date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return GetString("Monday");
                case DayOfWeek.Tuesday:
                    return GetString("Tuesday");
                case DayOfWeek.Wednesday:
                    return GetString("Wednesday");
                case DayOfWeek.Thursday:
                    return GetString("Thursday");
                case DayOfWeek.Friday:
                    return GetString("Friday");
                case DayOfWeek.Saturday:
                    return GetString("Saturday");
                case DayOfWeek.Sunday:
                    return GetString("Sunday");
                default:
                    return string.Empty;
            }
        }
    }
}
