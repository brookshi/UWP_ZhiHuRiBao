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

namespace Brook.ZhiHuRiBao.Pages
{
    public class MainViewModel : ViewModelBase
    {
        private string _currentDate;

        private readonly ObservableCollectionExtended<Story> _storyDataList = new ObservableCollectionExtended<Story>();

        public ObservableCollectionExtended<Story> StoryDataList { get { return _storyDataList; } }

        private List<TopStory> _topStoryList = new List<TopStory>();

        public List<TopStory> TopStoryList { get { return _topStoryList; } set { if (value != _topStoryList) { _topStoryList = value; Notify("TopStoryList"); } } }

        private readonly CommentLoadMoreCollection _commentList = new CommentLoadMoreCollection();

        public CommentLoadMoreCollection CommentList { get { return _commentList; } }


        private string _htmlSource = string.Empty;
        public string HtmlSource
        {
            get { return _htmlSource; }
            set
            {
                if (value !=  _htmlSource)
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

        public override void Init()
        {
        }

        public async Task Refresh()
        {
            Reset();
            IsRefreshContent = true;
            await RequestMainList(false);
            IsRefreshContent = false;
        }

        public async Task LoadMore()
        {
            await RequestMainList(true);
        }

        protected void Reset()
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
                Reset();
                storyData = await DataRequester.GetLatestStories();
                TopStoryList = storyData.top_stories;
            }

            _currentDate = storyData.date;
            StoryDataList.Add(new Story() { title = StringUtil.GetStoryGroupName(_currentDate), type = Misc.Group_Name_Type });
            StoryDataList.AddRange(storyData.stories);

            if (!isLoadingMore)
            {
                AutoDisplayFirstStory(storyData.stories[0].id.ToString());
            }
        }

        private void AutoDisplayFirstStory(string storyId)
        {
            var firstStory = StoryDataList.First();
            if (firstStory == null)
                return;

            RequestMainContent(storyId);
            RefreshComments(storyId);
        }

        public async void RequestMainContent(string id)
        {
            IsRefreshContent = true;
            var content = await DataRequester.RequestStoryContent(id);
            HtmlSource = Html.Constructor(content);
            IsRefreshContent = false;
        }

        public void RefreshComments(string storyId)
        {
            CommentList.Refresh(storyId);
        }
    }
}
