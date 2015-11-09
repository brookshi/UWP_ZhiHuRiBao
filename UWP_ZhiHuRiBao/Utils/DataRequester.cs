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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPHttp;

namespace Brook.ZhiHuRiBao.Utils
{
    public class DataRequester
    {
        public static Task<MainData> GetStories(string before)
        {
            return RequestDataForStory<MainData>("", before, Urls.Stories);
        }

        public static Task<MainData> GetLatestStories()
        {
            return RequestDataForStory<MainData>("", "", Urls.LatestStories);
        }

        public static Task<MainContent> RequestStoryContent(string storyId)
        {
            return RequestDataForStory<MainContent>(storyId, "", Urls.StoryContent);
        }

        public static Task<CommentInfo> RequestCommentInfo(string storyId)
        {
            return RequestDataForStory<CommentInfo>(storyId, Urls.CommentInfo);
        }
       
        public static Task<Comments> RequestLongComment(string id, string before)
        {
            return RequestDataForStory<Comments>(id, before, string.IsNullOrEmpty(before) ? Urls.LongComment : Urls.LongComment_More);
        }

        public static Task<Comments> RequestShortComment(string id, string before)
        {
            return RequestDataForStory<Comments>(id, before, string.IsNullOrEmpty(before) ? Urls.ShortComment : Urls.ShortComment_More);
        }

        public static Task<Categories> RequestCategory()
        {
            return XPHttpClient.DefaultClient.GetAsync<Categories>(Urls.BaseUrl + Urls.Categories, null);
        }

        public static Task<T> RequestDataForStory<T>(string id, string before, string functionUrl)
        {
            var httpParam = XPHttpClient.DefaultClient.RequestParamBuilder
                .AddUrlSegements("id", id ?? "")
                .AddUrlSegements("before", before ?? "");

            return XPHttpClient.DefaultClient.GetAsync<T>(functionUrl, httpParam);
        }

        public static Task<T> RequestDataForStory<T>(string id, string functionUrl)
        {
            var httpParam = XPHttpClient.DefaultClient.RequestParamBuilder
                .AddUrlSegements("id", id ?? "");

            return XPHttpClient.DefaultClient.GetAsync<T>(functionUrl, httpParam);
        }
    }
}
