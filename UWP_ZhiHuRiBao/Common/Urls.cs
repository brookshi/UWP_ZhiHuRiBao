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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brook.ZhiHuRiBao.Common
{
    public static class Urls
    {
        public const string BaseUrl = "http://news-at.zhihu.com/api/4/";

        public const string Stories = "stories/before/{before}";

        public const string LatestStories = "stories/latest";

        public const string StoryContent = "story/{id}";

        public const string CommentInfo = "story-extra/{id}";

        public const string LongComment = "story/{id}/long-comments";

        public const string LongComment_More = "story/{id}/long-comments/before/{before}";

        public const string ShortComment = "story/{id}/short-comments";

        public const string ShortComment_More = "story/{id}/short-comments/before/{before}";
    }
}
