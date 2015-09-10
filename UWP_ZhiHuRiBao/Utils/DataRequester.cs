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
using Brook.ZhiHuRiBao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPHttp;

namespace Brook.ZhiHuRiBao.Utils
{
    public class DataRequester
    {
        public static void RequestStories(string before, Action<MainData> onSuccess)
        {
            RequestDataForStory("", before, Urls.Stories, onSuccess);
        }

        public static void RequestStoryContent(string id, Action<MainContent> onSuccess)
        {
            RequestDataForStory(id, "", Urls.StoryContent, onSuccess);
        }

        public static void RequestLongComment(string id, string before, Action<Comments> onSuccess)
        {
            RequestDataForStory(id, before, string.IsNullOrEmpty(before) ? Urls.LongComment : Urls.LongComment_More, onSuccess);
        }

        public static void RequestShortComment(string id, string before, Action<Comments> onSuccess)
        {
            RequestDataForStory(id, before, string.IsNullOrEmpty(before) ? Urls.ShortComment : Urls.ShortComment_More, onSuccess);
        }

        public static void RequestDataForStory<T>(string id, string before, string functionUrl, Action<T> onSuccess)
        {
            var httpParam = XPHttpClient.DefaultClient.RequestParamBuilder
                .AddUrlSegements("id", id)
                .AddUrlSegements("before", before);
            XPHttpClient.DefaultClient.GetAsync(functionUrl, httpParam, new XPResponseHandler<T>()
            {
                OnSuccess = (response, content) => onSuccess(content),
            });
        }
    }
}
