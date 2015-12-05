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

using Brook.ZhiHuRiBao.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brook.ZhiHuRiBao.Utils
{
    public static class Urls
    {
        public const string LocalUrlPrefx = "http://daily.zhihu.com";

        public const string BaseUrl = "http://news-at.zhihu.com/api/4/";

        public const string Login = "login";

        public const string Categories = "themes";

        public const string CategoryStories = "theme/{categoryid}/before/{before}";

        public const string CategoryLatestStories = "theme/{categoryid}";

        public const string Stories = "stories/before/{before}";

        public const string LatestStories = "stories/latest";

        public const string StoryContent = "story/{storyid}";

        public const string StoryExtraInfo = "story-extra/{storyid}";

        public const string LongComment = "story/{storyid}/long-comments";

        public const string LongComment_More = "story/{storyid}/long-comments/before/{before}";

        public const string ShortComment = "story/{storyid}/short-comments";

        public const string ShortComment_More = "story/{storyid}/short-comments/before/{before}";

        public const string SendComment = "news/{storyid}/comment";

        public const string Vote = "vote/story/{storyid}";

        public const string Favorite = "favorite/{storyid}";

        public static string GetStoriesUrl(int categoryId)
        {
            return categoryId == Misc.Default_Category_Id ? Stories : CategoryStories;
        }

        public static string GetLatestStoriesUrl(int categoryId)
        {
            return categoryId == Misc.Default_Category_Id ? LatestStories : CategoryLatestStories;
        }
    }
}
