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
using Brook.ZhiHuRiBao.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using XPHttp;
using XPHttp.Serializer;

namespace Brook.ZhiHuRiBao.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _currentDate;
        private CommentType _currCommentType = CommentType.Long;

        private readonly ObservableCollectionExtended<Story> _storyDataList = new ObservableCollectionExtended<Story>();

        public ObservableCollectionExtended<Story> StoryDataList { get { return _storyDataList; } }

        private List<TopStory> _topStoryList = new List<TopStory>();

        public List<TopStory> TopStoryList { get { return _topStoryList; } set { if (value != _topStoryList) { _topStoryList = value; Notify("TopStoryList"); } } }

        private readonly ObservableCollectionExtended<GroupComments> _commentList = new ObservableCollectionExtended<GroupComments>();

        public ObservableCollectionExtended<GroupComments> CommentList { get { return _commentList; } }


        private string _htmlSource = string.Empty;
        public string HtmlSource
        {
            get { return _htmlSource; }
            set
            {
                if (value != _htmlSource)
                {
                    _htmlSource = value;
                    Notify("HtmlSource");
                }
            }
        }

        private bool _isRefreshContent = false;
        public bool IsRefreshContent
        {
            get { return _isRefreshContent; }
            set
            {
                if(value != _isRefreshContent)
                {
                    _isRefreshContent = value;
                    Notify("IsRefreshContent");
                }
            }
        }

        private int _mode = 0;
        public int Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        public string FirstStoryId
        {
            get { return StoryDataList.First(o=>!Misc.IsGroupItem(o.type))?.id.ToString() ?? null; }
        }

        public string LastCommentId
        {
            get { return CommentList.Last().LastOrDefault()?.id.ToString() ?? null; }
        }

        public string CurrentStoryId { get; set; }

        public override void Init()
        {
        }

        public async Task Refresh()
        {
            ResetStorys();
            await RequestMainList(false);

            if (StoryDataList.Count < 20)
            {
                await LoadMore();
            }
        }

        public async Task LoadMore()
        {
            await RequestMainList(true);
        }

        protected void ResetStorys()
        {
            _currentDate = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
            StoryDataList.Clear();
            TopStoryList.Clear();
        }

        private async Task RequestMainList(bool isLoadingMore)
        {
            MainData storyData = null;

            if (isLoadingMore)
            {
                storyData = await DataRequester.GetStories(_currentDate);
            }
            else
            {
                ResetStorys();
                storyData = await DataRequester.GetLatestStories();
                if(storyData != null)
                {
                    TopStoryList = storyData.top_stories;
                    CurrentStoryId = storyData.stories.First().id.ToString();
                }
            }
            if (storyData == null)
                return;

            _currentDate = storyData.date;

            StoryDataList.Add(new Story() { title = StringUtil.GetStoryGroupName(_currentDate), type = Misc.Group_Name_Type });
            StoryDataList.AddRange(storyData.stories);
        }

        public async void RequestMainContent(string storyId)
        {
            IsRefreshContent = true;
            var content = await DataRequester.RequestStoryContent(storyId);
            if(content != null)
            {
                HtmlSource = Html.Constructor(content);
            }
            IsRefreshContent = false;
        }

        public async Task RequestComments(string storyId, bool isLoadingMore)
        {
            if (!isLoadingMore)
            {
                ResetComments();
                await InitCommentInfo(storyId);
            }

            if (_currCommentType == CommentType.Long)
            {
                await RequestLongComments(storyId, isLoadingMore);
            }
            else if (_currCommentType == CommentType.Short)
            {
                await RequestShortComments(storyId);
            }
        }

        public void ResetComments()
        {
            CommentList.Clear();
            _currCommentType = CommentType.Long;
        }

        private async Task RequestLongComments(string storyId, bool isLoadingMore)
        {
            var longComment = await DataRequester.RequestLongComment(storyId, LastCommentId);
            if (longComment == null)
                return;

            CommentList.First().AddRange(longComment.comments);

            if (longComment == null || longComment.comments.Count < 20)
            {
                await RequestShortComments(storyId);
            }
        }

        private async Task RequestShortComments(string storyId)
        {
            if(_currCommentType == CommentType.Long)
            {
                _currCommentType = CommentType.Short;
            }
            var shortComment = await DataRequester.RequestShortComment(storyId, _currCommentType == CommentType.Long ? null : LastCommentId);
            if (shortComment == null)
                return;

            CommentList.Last().AddRange(shortComment.comments);
        }

        private async Task InitCommentInfo(string storyId)
        {
            var commentInfo = await DataRequester.RequestCommentInfo(storyId);
            if (commentInfo == null)
                return;

            if(CommentList.Count == 0)
            {
                CommentList.Add(new GroupComments() { GroupName = StringUtil.GetCommentGroupName(CommentType.Long, commentInfo.long_comments.ToString()) });
                CommentList.Add(new GroupComments() { GroupName = StringUtil.GetCommentGroupName(CommentType.Short, commentInfo.short_comments.ToString()) });
            }
        }
    }
}
